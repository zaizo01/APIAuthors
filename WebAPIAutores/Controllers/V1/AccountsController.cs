using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPIAutores.DTOs;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers.V1
{
    [Route("api/v1/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector;
        private readonly IDataProtectionProvider dataProtectionProvider;

        public AccountsController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            SignInManager<IdentityUser> signInManager, IDataProtectionProvider dataProtectionProvider,
            HashService hashService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            dataProtector = dataProtectionProvider.CreateProtector("unique_and_secret_value");
        }

        [HttpPost("UserRegister", Name = "UserRegister")]
        public async Task<ActionResult<ResponseAuthentication>> UserRegister(UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                return await BuildToken(userCredentials);
            } else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("UserLogin", Name = "UserLogin")]
        public async Task<ActionResult<ResponseAuthentication>> UserLogin(UserCredentials userCredentials)
        {
            var result = await signInManager.PasswordSignInAsync(userCredentials.Email,
                userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(userCredentials);
            }
            else
            {
                return BadRequest("Login Failed");
            }
        }
        [HttpGet("RenewToken", Name = "RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ResponseAuthentication>> RenewToken()
        {
            var emailUserClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var userEmail = emailUserClaim.Value;
            var userCredentials = new UserCredentials
            {
                Email = userEmail
            };
            return await BuildToken(userCredentials);
        }

        private async Task<ResponseAuthentication> BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyJwt"])); 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationToken = DateTime.UtcNow.AddYears(1); //AddMinutes(30);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expirationToken,
                signingCredentials: credentials);

            return new ResponseAuthentication()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expirationToken
            };
        }

        [HttpPost("MakeAdminRol", Name = "MakeAdminRol")]
        public async Task<ActionResult> MakeAdminRol(PutAdminDTO putAdminDTO)
        {
            var user = await userManager.FindByEmailAsync(putAdminDTO.Email);
            await userManager.AddClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoveAdminRol", Name = "RemoveAdminRol")]
        public async Task<ActionResult> RemoveAdminRol(PutAdminDTO putAdminDTO)
        {
            var user = await userManager.FindByEmailAsync(putAdminDTO.Email);
            await userManager.RemoveClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [HttpGet("Encrypt")]
        public ActionResult Encrypt()
        {
            var plainText = "Cristopher Zaiz";
            var ciphertext = dataProtector.Protect(plainText);
            var decryptedText = dataProtector.Unprotect(ciphertext);

            return Ok(new
            {
                plainText = plainText,
                ciphertext = ciphertext,
                decryptedText = decryptedText
            });
        }
        
        [HttpGet("EncryptByTime")]
        public ActionResult EncryptByTime()
        {
            var timeLimitedProtector = dataProtector.ToTimeLimitedDataProtector();

            var plainText = "Cristopher Zaiz";
            var ciphertext = timeLimitedProtector.Protect(plainText, lifetime: TimeSpan.FromSeconds(5));
            Thread.Sleep(6000);
            var decryptedText = timeLimitedProtector.Unprotect(ciphertext);

            return Ok(new
            {
                plainText = plainText,
                ciphertext = ciphertext,
                decryptedText = decryptedText
            });
        }

        [HttpGet("hash/{plainText}")]
        public ActionResult BuidHash(string plainText)
        {
            var firstResult = hashService.Hash(plainText);
            var secondResult = hashService.Hash(plainText);

            return Ok(new { 
                plainText = plainText,
                Hash1 = firstResult,
                Hash2 = secondResult
            });
        }

     }
}
