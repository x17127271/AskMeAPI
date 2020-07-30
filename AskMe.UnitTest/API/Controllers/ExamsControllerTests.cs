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
    public class ExamsControllerTests
    {
        private readonly Mock<IExamService> _examService;
        private readonly Mock<IMapper> _mapper;
        private readonly ExamsController _sut;

        public ExamsControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _examService = new Mock<IExamService>();
            _sut = new ExamsController(_mapper.Object, _examService.Object);
        }

        [Fact]
        public async Task GetExams_WithUserId_ReturnsExamList()
        {
            //Arrange
            var userId = AutoFaker.Generate<int>();
            var exams = AutoFaker.Generate<List<Exam>>();
            _examService.Setup(x => x.GetExams(It.IsAny<int>()))
                .ReturnsAsync(exams);

            //Act
            var result = await _sut.GetExams(userId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(exams);
        }

        [Fact]
        public async Task GetExam_WithExamId_ReturnsExistingExam()
        {
            //Arrange
            var examId = AutoFaker.Generate<int>();
            var exam = AutoFaker.Generate<Exam>();
            var examDto = AutoFaker.Generate<ExamDto>();
            _examService.Setup(x => x.GetExamById(It.IsAny<int>()))
                .ReturnsAsync(exam);
            _mapper.Setup(x => x.Map<ExamDto>(exam))
                .Returns(examDto);

            //Act
            var result = await _sut.GetExam(examId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(examDto);
        }

        [Fact]
        public async Task GetExam_WithNotExistingExamId_ReturnsNotFound()
        {
            //Arrange
            var examId = AutoFaker.Generate<int>();
            var exam = (Exam)null;
            _examService.Setup(x => x.GetExamById(It.IsAny<int>()))
                .ReturnsAsync(exam);

            //Act
            var result = await _sut.GetExam(examId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            ((NotFoundResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
