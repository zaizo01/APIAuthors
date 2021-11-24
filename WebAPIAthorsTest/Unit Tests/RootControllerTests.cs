using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIAthorsTest.Mocks;
using WebAPIAutores.Controllers.V1;

namespace WebAPIAthorsTest.Unit_Tests
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task IsUserIsAdminGetNineLinks()
        {
            // Preparation
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Result = AuthorizationResult.Success();
            var rootController = new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();

            // Execution
            var result = await rootController.Get();

            // Verification
            Assert.AreEqual(9, result.Value.Count());
        }
        
        [TestMethod]
        public async Task IsUserNotIsAdminGetSevenLinks()
        {
            // Preparation
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Result = AuthorizationResult.Failed();
            var rootController = new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();

            // Execution
            var result = await rootController.Get();

            // Verification
            Assert.AreEqual(7, result.Value.Count());
        }
        
        [TestMethod]
        public async Task IsUserNotIsAdminGetSevenLinksUsingMoqLibrary()
        {
            // Preparation
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));
            
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockURLHelper = new Mock<IUrlHelper>();
            mockURLHelper.Setup(x =>
            x.Link(It.IsAny<string>(),
            It.IsAny<object>()))
                .Returns(string.Empty);
           
            var rootController = new RootController(mockAuthorizationService.Object);
            rootController.Url = new UrlHelperMock();

            // Execution
            var result = await rootController.Get();

            // Verification
            Assert.AreEqual(7, result.Value.Count());
        }
    }
}
