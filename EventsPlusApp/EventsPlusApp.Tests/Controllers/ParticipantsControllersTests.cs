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
    public class ParticipantsControllerTests
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
                databaseContext.Participants.AddRange(participants());
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
        private List<Participant> participants()
        {
            return new List<Participant>{
                new Participant{ ID = 1, FirstName="TestName1", LastName="TestLName1", PhoneNumber="111111111", EventID=events().Single( s => s.Title == "TestTitle1").ID},
                new Participant{ ID = 2, FirstName="TestName2", LastName="TestLName2", PhoneNumber="222222222", EventID=events().Single( s => s.Title == "TestTitle2").ID}
            };
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfOwners()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var participantssController = new ParticipantsController(dbContext);
            //Act
            var result = await participantssController.Index_defualt();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Participant>>(
              viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsViewResultWithOwnerModel()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var participantssController = new ParticipantsController(dbContext);

            //Act
            var result = await participantssController.Details(1);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Participant>(
            viewResult.ViewData.Model);
            Assert.Equal("TestTitle1", model.Event.Title);
            Assert.Equal("111111111", model.PhoneNumber);
            Assert.Equal("TestLName1", model.LastName);
            Assert.Equal("TestName1", model.FirstName);
            Assert.Equal(1, model.ID);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(4)]
        public async Task Edit_ReturnsHttpNotFoundWhenClubIdNotFound(int ParticipantID)
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var participantssController = new ParticipantsController(dbContext);
            //Act
            var result = await participantssController.Edit(ParticipantID);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
    }
}
