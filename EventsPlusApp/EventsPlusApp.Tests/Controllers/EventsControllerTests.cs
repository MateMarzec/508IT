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
    public class EventsControllerTests
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
                databaseContext.Owners.AddRange(owners());
                databaseContext.Locations.AddRange(locations());
                databaseContext.Events.AddRange(events());
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
        private List<Event> events()
        {
            return new List<Event>{
                new Event{ ID = 1, Title="TestTitle1", Type="Concert1", DateAndTime=DateTime.Parse("02/18/2021 14:00:00"), LocationID=locations().Single( s => s.Name == "TestName1").ID, ManagerID = managers().Single( s => s.FirstName == "TestName1").ID},
                new Event{ ID = 2, Title="TestTitle2", Type="Concert2", DateAndTime=DateTime.Parse("02/18/2021 14:00:00"), LocationID=locations().Single( s => s.Name == "TestName2").ID, ManagerID = managers().Single( s => s.FirstName == "TestName2").ID}
            };
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfOwners()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var eventssController = new EventsController(dbContext);
            //Act
            var result = await eventssController.Index_defualt();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Event>>(
              viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsViewResultWithOwnerModel()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var eventssController = new EventsController(dbContext);

            //Act
            var result = await eventssController.Details(1);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Event>(
            viewResult.ViewData.Model);
            Assert.Equal("TestName1", model.Manager.FirstName);
            Assert.Equal("TestName1", model.Location.Name);
            Assert.Equal(DateTime.Parse("02/18/2021 14:00:00"), model.DateAndTime);
            Assert.Equal("Concert1", model.Type);
            Assert.Equal("TestTitle1", model.Title);
            Assert.Equal(1, model.ID);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public async Task Edit_ReturnsHttpNotFoundWhenClubIdNotFound(int EventID)
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var eventssController = new EventsController(dbContext);
            //Act
            var result = await eventssController.Edit(EventID);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
    }
}
