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
    public class ResultsControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IResultService> _resultService;
        private readonly ResultsController _sut;

        public ResultsControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _resultService = new Mock<IResultService>();
            _sut = new ResultsController(_mapper.Object, _resultService.Object);
        }

        [Fact]
        public async Task GetResults_WithExamId_ReturnsResultList()
        {
            //Arrange
            var examId = AutoFaker.Generate<int>();
            var results = AutoFaker.Generate<List<Result>>();
            var resultsDto = AutoFaker.Generate<List<ResultDto>>();
            _resultService.Setup(x => x.GetResults(It.IsAny<int>()))
                .ReturnsAsync(results);
            _mapper.Setup(x => x.Map<List<ResultDto>>(results))
                .Returns(resultsDto);

            //Act
            var result = await _sut.GetResults(examId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(resultsDto);
        }

        [Fact]
        public async Task ProcessExamResult_ReturnsTrue()
        {
            //Arrange
            var examResultDto = AutoFaker.Generate<List<ExamResultDto>>();
            var examResult = AutoFaker.Generate<List<ExamResult>>();
            _mapper.Setup(x => x.Map<List<ExamResultDto>>(examResult))
               .Returns(examResultDto);
            _resultService.Setup(x => x.ProcessExamResult(It.IsAny<List<ExamResult>>()))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.ProcessExamResult(examResultDto);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(true);
        }
    }
}
