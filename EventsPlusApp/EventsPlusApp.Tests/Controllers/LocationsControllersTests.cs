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
    public class LocationsControllerTests
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
                databaseContext.Owners.AddRange(owners());
                databaseContext.Locations.AddRange(locations());
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        private List<Owner> owners()
        {
            return new List<Owner>{
                new Owner{ ID = 1, FirstName="TestName1", LastName="TestLName1", PhoneNumber="111111111"},
                new Owner{ ID = 2, FirstName="TestName2", LastName="TestLName2", PhoneNumber="222222222"}
            };
        }

        private List<Location> locations()
        {
            return new List<Location>{
                new Location{ ID = 1, MaximumNumberofParticipants=111, Name="TestName1", PostCode="111111", Address="TestAddress1", City="TestCity1", Country="TestCountry1", OwnerID = owners().Single( s => s.FirstName == "TestName1").ID},
                new Location{ ID = 2, MaximumNumberofParticipants=222, Name="TestName2", PostCode="222222", Address="TestAddress2", City="TestCity2", Country="TestCountry2", OwnerID = owners().Single( s => s.FirstName == "TestName2").ID}
            };
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfOwners()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var locationssController = new LocationsController(dbContext);
            //Act
            var result = await locationssController.Index_defualt();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Location>>(
              viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsViewResultWithOwnerModel()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var locationssController = new LocationsController(dbContext);

            //Act
            var result = await locationssController.Details(1);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Location>(
            viewResult.ViewData.Model);
            Assert.Equal("TestName1", model.Owner.FirstName);
            Assert.Equal("TestCountry1", model.Country);
            Assert.Equal("TestCity1", model.City);
            Assert.Equal("TestAddress1", model.Address);
            Assert.Equal("111111", model.PostCode);
            Assert.Equal("TestName1", model.Name);
            Assert.Equal(111, model.MaximumNumberofParticipants);
            Assert.Equal(1, model.ID);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public async Task Edit_ReturnsHttpNotFoundWhenClubIdNotFound(int LocationID)
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var locationssController = new LocationsController(dbContext);
            //Act
            var result = await locationssController.Edit(LocationID);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
    }
}
