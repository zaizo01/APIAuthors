using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entities;
using WebAPIAutores.Filters;
using WebAPIAutores.Utilities;

namespace WebAPIAutores.Controllers.V2
{
    //[Route("api/v2/autores")]
    [Route("api/autores")]
    [HeaderPresentAttribute("x-version", "2")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context, IMapper mapper, 
            IConfiguration configuration, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "GetAllAuthorsv2")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
        public async Task<ActionResult<List<AutorGetDTO>>> GetAll()
        {
            var autores = await context.Autores.ToListAsync();
            autores.ForEach(autor => autor.Name = autor.Name.ToUpper());
            return mapper.Map<List<AutorGetDTO>>(autores);
        }

        [HttpGet("{id:int}", Name = "GetAuthorByIdv2")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> GetById(int id)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(autorDB => autorDB.Id == id);
            if (autor == null) return NotFound();
            var authorDto = mapper.Map<AutorDTOConLibros>(autor);
            return authorDto;
        }

        

        [HttpGet("{name}", Name = "GetAuthorByNamev2")]
        public async Task<ActionResult<List<AutorGetDTO>>> GetByName(string name)
        {
            var autores = await context.Autores.Where(autorDB => autorDB.Name.Contains(name)).ToListAsync();
            return mapper.Map<List<AutorGetDTO>>(autores);
        }

        [HttpPost(Name = "PostAuthorsv2")]
        public async Task<ActionResult> Post([FromBody] AutorDTO autorDTO)
        {
            var authorExist = await context.Autores.AnyAsync(x => x.Name == autorDTO.Name);
            if (authorExist) return BadRequest($"El {autorDTO.Name} ya existe.");
            var autor = mapper.Map<Autor>(autorDTO);
            context.Add(autor);
            await context.SaveChangesAsync();
            var autorDto = mapper.Map<AutorGetDTO>(autor);
            return CreatedAtRoute("GetAutorv2", new { id = autor.Id }, autorDto);
        }

        [HttpPut("{id:int}", Name = "PutAuthorv2")]
        public async Task<ActionResult> Put(AutorPostDTO autorDto, int id)
        {
            var autorExiste = await context.Autores.AnyAsync(x => x.Id == id);
            if (!autorExiste) return NotFound();
            var autor = mapper.Map<Autor>(autorDto);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteAuthorv2")]
        public async Task<ActionResult> Delete(int id)
        {
            var authorExist = await context.Autores.AnyAsync(x => x.Id == id);
            if (!authorExist) return NotFound();
            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
