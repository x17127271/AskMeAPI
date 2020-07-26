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
    public class QuestionServiceTests
    {
        private readonly Mock<IAskMeRepository> _askmeRepository;
        private readonly QuestionService _sut;
        public QuestionServiceTests()
        {
            _askmeRepository = new Mock<IAskMeRepository>();
            _sut = new QuestionService(_askmeRepository.Object);
        }


        [Fact]
        public async Task Create_ReturnsQuestionLesson()
        {
            //Arrange
            var question = AutoFaker.Generate<Question>();
            var lessonId = AutoFaker.Generate<int>();
            _askmeRepository.Setup(x => x.AddQuestion(It.IsAny<Question>(), It.IsAny<int>()))
                .ReturnsAsync(question);

            //Act
            var result = await _sut.Create(question, lessonId);

            //Assert
            result.Should().BeOfType<Question>();
            result.Should().BeEquivalentTo(question);
        }

        [Fact]
        public async Task GetQuestionById_ReturnsExistingQuestionAnswer()
        {
            //Arrange
            var question = AutoFaker.Generate<Question>();
            var questionId = AutoFaker.Generate<int>();
            _askmeRepository.Setup(x => x.GetQuestionById(It.IsAny<int>()))
                .ReturnsAsync(question);

            //Act
            var result = await _sut.GetQuestionById(questionId);

            //Assert
            result.Should().BeOfType<Question>();
            result.Should().BeEquivalentTo(question);
        }

        [Fact]
        public async Task GetQuestions_ReturnsQuestionList()
        {
            //Arrange
            var lessonId = AutoFaker.Generate<int>();
            var questions = AutoFaker.Generate<List<Question>>();
            _askmeRepository.Setup(x => x.GetQuestions(It.IsAny<int>()))
                .ReturnsAsync(questions);

            //Act
            var result = await _sut.GetQuestions(lessonId);

            //Assert
            result.Should().BeOfType<List<Question>>();
            result.Should().BeEquivalentTo(questions);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsQuestionUpdated()
        {
            //Arrange
            var question = AutoFaker.Generate<Question>();
            _askmeRepository.Setup(x => x.UpdateQuestion(It.IsAny<Question>()))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateQuestion(question);

            //Assert
            result.Should().BeTrue();
        }
    }
}
