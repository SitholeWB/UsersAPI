using Contracts;
using Microsoft.EntityFrameworkCore;
using Models.Commands;
using Models.Entities;
using Models.Exceptions;
using NSubstitute;
using NUnit.Framework;
using Services.DataLayer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Tests
{
    public class UsersServiceTests
    {

        [Test]
        public async Task AddUserAsync_GivenValidInput_ShouldAddUser()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);

            //Act
            var results = await service.AddUserAsync(new AddUserCommand { Email = "user@zululand.co.za", Name = "Welcome", Surname = "Sithole", Gender = "Male" });

            //Assert
            Assert.AreEqual(results.Name, "Welcome");
            Assert.AreEqual(results.Surname, "Sithole");
            Assert.AreEqual(results.Gender, "Male");
            Assert.AreEqual(results.Email, "user@zululand.co.za");
        }
        [Test]
        public async Task AddUserAsync_GivenPassword_ShouldAddUserAndEncryptPassword()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("TestDecrypted");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("TestEncrypted");
            var service = new UsersService(dbContext, cryptoService);

            //Act
            var results = await service.AddUserAsync(new AddUserCommand { Email = "user@zululand.co.za", Password = "pass123", Name = "Welcome", Surname = "Sithole", Gender = "Male" });

            //Assert
            Assert.AreEqual(results.Password, "TestEncrypted");
            Assert.AreEqual(results.Name, "Welcome");
            Assert.AreEqual(results.Surname, "Sithole");
            Assert.AreEqual(results.Gender, "Male");
            Assert.AreEqual(results.Email, "user@zululand.co.za");
        }

        [Test]
        public async Task AddUserAsync_GivenUsersAlreadyExistsOnDatabase_ShouldNotSetSUPER_ADMIN_Role()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);
            await service.AddUserAsync(new AddUserCommand { Email = "user1@zululand.co.za", Name = "Welcome", Surname = "Sithole", Gender = "Male" });

            //Act
            var results = await service.AddUserAsync(new AddUserCommand { Email = "user2@zululand.co.za", Name = "Welcome", Surname = "Sithole", Gender = "Male" });

            //Assert
            Assert.AreEqual(results.Role, "GENERAL");
            Assert.AreEqual(results.Name, "Welcome");
            Assert.AreEqual(results.Surname, "Sithole");
            Assert.AreEqual(results.Gender, "Male");
            Assert.AreEqual(results.Email, "user2@zululand.co.za");
        }

        [Test]
        public async Task AddUserAsync_GivenIsFirstUserOnDatabase_ShouldAddUserAndSetSuperAdmin()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);

            //Act
            var results = await service.AddUserAsync(new AddUserCommand { Email = "user@zululand.co.za", Name = "Welcome", Surname = "Sithole", Gender = "Male" });

            //Assert
            Assert.AreEqual(results.Name, "Welcome");
            Assert.AreEqual(results.Surname, "Sithole");
            Assert.AreEqual(results.Gender, "Male");
            Assert.AreEqual(results.Role, "SUPER_ADMIN");
            Assert.AreEqual(results.Email, "user@zululand.co.za");
        }

        [Test]
        public async Task AddUserAsync_GivenEmailAreadyExist_ShouldThrowUserException()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);
            await service.AddUserAsync(new AddUserCommand { Email = "user@zululand.co.za", Name = "Welcome", Surname = "Sithole", Gender = "Male" });

            //Act & Assert
            Assert.ThrowsAsync<UserException>(() => service.AddUserAsync(new AddUserCommand { Email = "user@zululand.co.za" }));
        }


        [Test]
        public async Task GetUsersAsync_GivenTwoUsers_ShouldReturnTwoUsers()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);
            var user1 = await service.AddUserAsync(new AddUserCommand { Email = "user1@zululand.co.za", Name = "Welcome One", Surname = "Sithole", Gender = "Male" });
            var user2 = await service.AddUserAsync(new AddUserCommand { Email = "user2@zululand.co.za", Name = "Welcome Two", Surname = "Sithole", Gender = "Male" });

            //Act
            var results = await service.GetUsersAsync();

            //Assert
            Assert.AreEqual(results.Count(), 2);
            Assert.IsTrue(results.Any(a => a.Id == user1.Id));
            Assert.IsTrue(results.Any(a => a.Email == user1.Email));
        }

        [Test]
        public async Task GetUserByEmailAsync_GivenThreeUsers_ShouldReturnUserForGivenEmail()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);
            var user1 = await service.AddUserAsync(new AddUserCommand { Email = "user1@zululand.co.za", Name = "Welcome One", Surname = "Sithole", Gender = "Male" });
            await service.AddUserAsync(new AddUserCommand { Email = "user2@zululand.co.za", Name = "Welcome Two", Surname = "Sithole", Gender = "Male" });
            await service.AddUserAsync(new AddUserCommand { Email = "user3@zululand.co.za", Name = "Welcome Three", Surname = "Sithole", Gender = "Male" });

            //Act
            var results = await service.GetUserByEmailAsync(user1.Email);

            //Assert
            Assert.AreEqual(results.Id, user1.Id);
            Assert.AreEqual(results.Email, user1.Email);
        }

        [Test]
        public async Task GetUserByIdAsync_GivenThreeUsers_ShouldReturnUserForGivenId()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);
            var user1 = await service.AddUserAsync(new AddUserCommand { Email = "user1@zululand.co.za", Name = "Welcome One", Surname = "Sithole", Gender = "Male" });
            await service.AddUserAsync(new AddUserCommand { Email = "user2@zululand.co.za", Name = "Welcome Two", Surname = "Sithole", Gender = "Male" });
            await service.AddUserAsync(new AddUserCommand { Email = "user3@zululand.co.za", Name = "Welcome Three", Surname = "Sithole", Gender = "Male" });

            //Act
            var results = await service.GetUserAsync(user1.Id);

            //Assert
            Assert.AreEqual(results.Id, user1.Id);
            Assert.AreEqual(results.Email, user1.Email);
        }

        [Test]
        public async Task UpdateUserAsync_GivenThreeUsers_ShouldUpdateUserForGivenId()
        {
            //Arrange
            var dbContext = GetDatabaseContext();
            var cryptoService = Substitute.For<ICryptoEngineService>();
            cryptoService.Decrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            cryptoService.Encrypt(Arg.Any<string>()).ReturnsForAnyArgs<string>("test");
            var service = new UsersService(dbContext, cryptoService);
            var user1 = await service.AddUserAsync(new AddUserCommand { Email = "user1@zululand.co.za", Name = "Welcome One", Surname = "Sithole", Gender = "Male" });
            await service.AddUserAsync(new AddUserCommand { Email = "user2@zululand.co.za", Name = "Welcome Two", Surname = "Sithole", Gender = "Male" });
            await service.AddUserAsync(new AddUserCommand { Email = "user3@zululand.co.za", Name = "Welcome Three", Surname = "Sithole", Gender = "Male" });

            //Act
            var results = await service.UpdateUserAsync(new User { Id = user1.Id, Email = "user1@zululand.co.za", Name = "UpdatedName", Surname = "UpdateSurname", Role = "ADMIN" });
            var resultsUpdated = await service.GetUserAsync(user1.Id);

            //Assert

            //Returned User should be updated
            Assert.AreEqual(results.Email, user1.Email);
            Assert.AreEqual(results.Name, "UpdatedName");
            Assert.AreEqual(results.Surname, "UpdateSurname");
            Assert.AreEqual(results.Role, "ADMIN");

            //Get User by Id should return Updated User
            Assert.AreEqual(resultsUpdated.Email, user1.Email);
            Assert.AreEqual(resultsUpdated.Name, "UpdatedName");
            Assert.AreEqual(resultsUpdated.Surname, "UpdateSurname");
            Assert.AreEqual(resultsUpdated.Role, "ADMIN");
        }

        private UsersDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new UsersDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

    }
}