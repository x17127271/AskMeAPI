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
    public class SubjectsControllerTests
    {
        private readonly Mock<ISubjectService> _subjectService;
        private readonly Mock<IMapper> _mapper;
        private readonly SubjectsController _sut;

        public SubjectsControllerTests()
        {
            _subjectService = new Mock<ISubjectService>();
            _mapper = new Mock<IMapper>();
            _sut = new SubjectsController(_mapper.Object, _subjectService.Object);
        }

        [Fact]
        public async Task GetSubjects_WithSujectId_RetunrsSubjectsList()
        {
            //Arrange
            var userId = AutoFaker.Generate<int>();
            var subjects = AutoFaker.Generate<List<Subject>>();
            _subjectService.Setup(x => x.GetSubjects(It.IsAny<int>()))
                .ReturnsAsync(subjects);

            //Act
            var result = await _sut.GetSubjects(userId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(subjects);
        }

        [Fact]
        public async Task GetSubject_WithExistingSubjectId_RetunrsSubjectsList()
        {
            //Arrange
            var userId = AutoFaker.Generate<int>();
            var subject = AutoFaker.Generate<Subject>();
            var subjectDto = AutoFaker.Generate<SubjectDto>();
            _subjectService.Setup(x => x.GetSubjectById(It.IsAny<int>()))
                .ReturnsAsync(subject);
            _mapper.Setup(x => x.Map<SubjectDto>(subject))
                .Returns(subjectDto);


            //Act
            var result = await _sut.GetSubject(userId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(subjectDto);
        }

        [Fact]
        public async Task GetLesson_WithNoExistingLessonId_RetunrsNotFound()
        {
            //Arrange
            var userId = AutoFaker.Generate<int>();
            var lesson = (Subject)null;
            _subjectService.Setup(x => x.GetSubjectById(It.IsAny<int>()))
                .ReturnsAsync(lesson);

            //Act
            var result = await _sut.GetSubject(userId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            ((NotFoundResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateLesson_ReturnsNewLesson()
        {
            //Arrange
            var subjectForCreation = AutoFaker.Generate<SubjectForCreationDto>();
            var subject = AutoFaker.Generate<Subject>();
            var subjectDto = AutoFaker.Generate<SubjectDto>();
            _subjectService.Setup(x => x.Create(It.IsAny<Subject>(), It.IsAny<int>()))
                .ReturnsAsync(subject);
            _mapper.Setup(x => x.Map<Subject>(subjectForCreation))
                .Returns(subject);
            _mapper.Setup(x => x.Map<SubjectDto>(subject))
                .Returns(subjectDto);

            //Action
            var result = await _sut.CreateSubject(subjectForCreation);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(subjectDto);
        }

        [Fact]
        public async Task UpdateLesson_ReturnsLessonUpdated()
        {
            //Arrange
            var subject = AutoFaker.Generate<Subject>();
            var subjectDto = AutoFaker.Generate<SubjectDto>();
            _mapper.Setup(x => x.Map<Subject>(subjectDto))
                .Returns(subject);
            _subjectService.Setup(x => x.UpdateSubject(subject))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateSubject(subjectDto);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
