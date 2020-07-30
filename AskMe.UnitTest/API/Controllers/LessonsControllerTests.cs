using AskMe.API.Controllers;
using AskMe.API.Models;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AutoBogus;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AskMe.UnitTest.API.Controllers
{
    public class LessonsControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILessonService> _lessonService;
        private readonly LessonsController _sut;

        public LessonsControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _lessonService = new Mock<ILessonService>();
            _sut = new LessonsController(_mapper.Object, _lessonService.Object);
        }

        [Fact]
        public async Task GetLessons_WithSujectId_RetunrsLessonsList()
        {
            //Arrange
            var subjectId = AutoFaker.Generate<int>();
            var lessons = AutoFaker.Generate<List<Lesson>>();
            _lessonService.Setup(x => x.GetLessons(It.IsAny<int>()))
                .ReturnsAsync(lessons);

            //Act
            var result = await _sut.GetLessons(subjectId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(lessons);
        }

        [Fact]
        public async Task GetLesson_WithExistingLessonId_RetunrsLessonsList()
        {
            //Arrange
            var lessonId = AutoFaker.Generate<int>();
            var lesson = AutoFaker.Generate<Lesson>();
            var lessonDto = AutoFaker.Generate<LessonDto>();
            _lessonService.Setup(x => x.GetLessonById(It.IsAny<int>()))
                .ReturnsAsync(lesson);
            _mapper.Setup(x => x.Map<LessonDto>(lesson))
                .Returns(lessonDto);
                

            //Act
            var result = await _sut.GetLesson(lessonId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(lessonDto);
        }

        [Fact]
        public async Task GetLesson_WithNoExistingLessonId_RetunrsNotFound()
        {
            //Arrange
            var lessonId = AutoFaker.Generate<int>();
            var lesson = (Lesson)null;
            _lessonService.Setup(x => x.GetLessonById(It.IsAny<int>()))
                .ReturnsAsync(lesson);         

            //Act
            var result = await _sut.GetLesson(lessonId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            ((NotFoundResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateLesson_ReturnsNewLesson()
        {
            //Arrange
            var lessonForCreation = AutoFaker.Generate<LessonForCreationDto>();
            var lesson = AutoFaker.Generate<Lesson>();
            var lessonDto = AutoFaker.Generate<LessonDto>();
            _lessonService.Setup(x => x.Create(It.IsAny<Lesson>(), It.IsAny<int>()))
                .ReturnsAsync(lesson);
            _mapper.Setup(x => x.Map<Lesson>(lessonForCreation))
                .Returns(lesson);
            _mapper.Setup(x => x.Map<LessonDto>(lesson))
                .Returns(lessonDto);

            //Action
            var result = await _sut.CreateLesson(lessonForCreation);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(lessonDto);
        }

        [Fact]
        public async Task UpdateLesson_ReturnsLessonUpdated()
        {
            //Arrange
            var lesson = AutoFaker.Generate<Lesson>();
            var lessonDto = AutoFaker.Generate<LessonDto>();
            _mapper.Setup(x => x.Map<Lesson>(lessonDto))
                .Returns(lesson);
            _lessonService.Setup(x => x.UpdateLesson(lesson))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateLesson(lessonDto);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
