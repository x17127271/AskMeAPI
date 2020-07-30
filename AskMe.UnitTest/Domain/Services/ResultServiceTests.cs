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
    public class ResultServiceTests
    {
        private readonly Mock<IAskMeRepository> _askmeRepository;
        private readonly ResultService _sut;

        public ResultServiceTests()
        {
            _askmeRepository = new Mock<IAskMeRepository>();
            _sut = new ResultService(_askmeRepository.Object);
        }

        [Fact]
        public async Task GetResults_ReturnsExistingResults()
        {
            //Arrange
            var examId = AutoFaker.Generate<int>();
            var examResults = AutoFaker.Generate<List<Result>>();
            _askmeRepository.Setup(x => x.GetResults(It.IsAny<int>()))
                .ReturnsAsync(examResults);

            //Act
            var result = await _sut.GetResults(examId);

            //Assert
            result.Should().BeOfType<List<Result>>();
            result.Should().BeEquivalentTo(examResults);
        }
    }
}
