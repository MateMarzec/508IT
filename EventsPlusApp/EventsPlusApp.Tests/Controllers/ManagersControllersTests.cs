using EventsPlusApp.Controllers;
using EventsPlusApp.Data;
using EventsPlusApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EventsPlusApp.Tests.Controllers
{
    public class ManagersControllerTests
    {
        private async Task<ApplicationDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Users.CountAsync() <= 0)
            {
                databaseContext.Managers.AddRange(managers());
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        private List<Manager> managers()
        {
            return new List<Manager>{
                new Manager{ ID = 1, FirstName="TestName1", LastName="TestLName1", PhoneNumber="111111111"},
                new Manager{ ID = 2, FirstName="TestName2", LastName="TestLName2", PhoneNumber="222222222"}
            };
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfOwners()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var managerssController = new ManagersController(dbContext);
            //Act
            var result = await managerssController.Index_defualt();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Manager>>(
              viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsViewResultWithOwnerModel()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var managerssController = new ManagersController(dbContext);

            //Act
            var result = await managerssController.Details(1);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Manager>(
            viewResult.ViewData.Model);
            Assert.Equal("111111111", model.PhoneNumber);
            Assert.Equal("TestLName1", model.LastName);
            Assert.Equal("TestName1", model.FirstName);
            Assert.Equal(1, model.ID);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public async Task Edit_ReturnsHttpNotFoundWhenClubIdNotFound(int ManagerID)
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var managerssController = new ManagersController(dbContext);
            //Act
            var result = await managerssController.Edit(ManagerID);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
    }
}
