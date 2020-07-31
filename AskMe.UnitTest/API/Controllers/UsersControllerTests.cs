using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AskMeAPI.Controllers;
using AskMeAPI.Helpers;
using AskMeAPI.Models;
using AutoBogus;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace AskMe.UnitTest.API.Controllers
{
    public class UsersControllerTests
    {
        public readonly Mock<IMapper> _mapper;
        public readonly Mock<IUserService> _userService;
        public readonly Mock<IOptions<AppSettings>> _appSetings;
        public readonly UsersController _sut;

        public UsersControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _userService = new Mock<IUserService>();
            _appSetings = new Mock<IOptions<AppSettings>>();
            _sut = new UsersController(_mapper.Object, _userService.Object, _appSetings.Object);
        }

        [Fact]
        public void UsersController_Inherits_ControllerBase()
        {
            typeof(UsersController).Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public void UsersController_DecoratedWithAutorizeAttribute()
        {
            typeof(UsersController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void UsersController_DecoratedWithRouteAttribute()
        {
            typeof(UsersController).Should().BeDecoratedWith<RouteAttribute>(a => a.Template == "api/[controller]");
        }

        [Fact]
        public void UsersController_DecoratedWithApiControllerAttribute()
        {
            typeof(UsersController).Should().BeDecoratedWith<ApiControllerAttribute>();
        }

        [Fact]
        public async Task GetUser_WithUserId_ReturnsExistingUser()
        {
            //Arrange
            var userId = AutoFaker.Generate<int>();
            var user = AutoFaker.Generate<User>();
            var userDto = AutoFaker.Generate<UserDto>();
            _userService.Setup(x => x.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(user);
            _mapper.Setup(x => x.Map<UserDto>(user))
                .Returns(userDto);

            //Act
            var result = await _sut.GetUser(userId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task GetUser_WithNotExistingUserId_ReturnsNotFound()
        {
            //Arrange
            var userId = AutoFaker.Generate<int>();
            var user = (User)null;           
            _userService.Setup(x => x.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(user);
           
            //Act
            var result = await _sut.GetUser(userId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            ((NotFoundResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }


    }
}
