using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AskMe.Domain.Services;
using AutoBogus;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AskMe.UnitTest.Domain.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IAskMeRepository> _askmeRepository;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _askmeRepository = new Mock<IAskMeRepository>();
            _sut = new UserService(_askmeRepository.Object);
        }

        [Fact]
        public async Task GetUserById_ReturnsExistingUser()
        {
            //Arrange
            var userId = AutoFaker.Generate<int>();
            var user = AutoFaker.Generate<User>();
            _askmeRepository.Setup(x => x.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(user);

            //Act
            var result = await _sut.GetUserById(userId);

            //Assert
            result.Should().BeOfType<User>();
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetUsers_ReturnsUserList()
        {
            //Arrange
            var users = AutoFaker.Generate<List<User>>();
            _askmeRepository.Setup(x => x.GetUsers())
                .ReturnsAsync(users);

            //Act
            var result = await _sut.GetUsers();

            //Assert
            result.Should().BeOfType<List<User>>();
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task Authenticate_WithUserNameEmpty_ReturnsNull()
        {
            //Arrange
            var userName = string.Empty;
            var password = AutoFaker.Generate<string>();
            
            //Act
            var result = await _sut.Authenticate(userName,password);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Authenticate_WithUserNotExist_ReturnsNull()
        {
            //Arrange
            var userName = AutoFaker.Generate<string>();
            var password = AutoFaker.Generate<string>();
            var user = (User)null;
            _askmeRepository.Setup(x => x.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(user);

            //Act
            var result = await _sut.Authenticate(userName, password);

            //Assert
            result.Should().BeNull();
        }        
    }
}
