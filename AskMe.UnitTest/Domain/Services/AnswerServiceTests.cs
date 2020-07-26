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
    public class AnswerServiceTests
    {
        private readonly Mock<IAskMeRepository> _askmeRepository;
        private readonly AnswerService _sut;

        public AnswerServiceTests()
        {
            _askmeRepository = new Mock<IAskMeRepository>();
            _sut = new AnswerService(_askmeRepository.Object);
        }

        [Fact]
        public async Task Create_ReturnsNewAnswer()
        {
            //Arrange
            var answer = AutoFaker.Generate<Answer>();
            var questionId = AutoFaker.Generate<int>();
            _askmeRepository.Setup(x => x.AddAnswer(It.IsAny<Answer>(), It.IsAny<int>()))
                .ReturnsAsync(answer);

            //Act
            var result = await _sut.Create(answer, questionId);

            //Assert
            result.Should().BeOfType<Answer>();
            result.Should().BeEquivalentTo(answer);
        }

        [Fact]
        public async Task GetAnswerById_ReturnsExistingAnswer()
        {
            //Arrange
            var answer = AutoFaker.Generate<Answer>();
            var answerId = AutoFaker.Generate<int>();
            _askmeRepository.Setup(x => x.GetAnswerById(It.IsAny<int>()))
                .ReturnsAsync(answer);

            //Act
            var result = await _sut.GetAnswerById(answerId);

            //Assert
            result.Should().BeOfType<Answer>();
            result.Should().BeEquivalentTo(answer);
        }

        [Fact]
        public async Task GetAnswers_ReturnsAnswerList()
        {
            //Arrange
            var questionId = AutoFaker.Generate<int>();
            var answers = AutoFaker.Generate<List<Answer>>();
            _askmeRepository.Setup(x => x.GetAnswers(It.IsAny<int>()))
                .ReturnsAsync(answers);

            //Act
            var result = await _sut.GetAnswers(questionId);

            //Assert
            result.Should().BeOfType<List<Answer>>();
            result.Should().BeEquivalentTo(answers);
        }

        [Fact]
        public async Task UpdateAnswer_ReturnsAnswerUpdated()
        {
            //Arrange
            var answer = AutoFaker.Generate<Answer>();
            _askmeRepository.Setup(x => x.UpdateAnswer(It.IsAny<Answer>()))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateAnswer(answer);

            //Assert
            result.Should().BeTrue();
        }
    }
}
