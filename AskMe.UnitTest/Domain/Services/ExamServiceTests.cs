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
    public class ExamServiceTests
    {
        private readonly Mock<IAskMeRepository> _askmeRepository;
        private readonly ExamService _sut;

        public ExamServiceTests()
        {
            _askmeRepository = new Mock<IAskMeRepository>();
            _sut = new ExamService(_askmeRepository.Object);
        }

        [Fact]
        public async Task AddExamQuestion_AddQuestionToExam_ReturnsNewExamQuestion()
        {
            //Assert
            var examId = AutoFaker.Generate<int>();
            var questionId = AutoFaker.Generate<int>();
            var examQuestion = AutoFaker.Generate<ExamQuestion>();
            _askmeRepository.Setup(x => x.AddExamQuestion(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(examQuestion);

            //Act
            var result = await _sut.AddExamQuestion(examId, questionId);

            //Assert
            result.Should().BeOfType<ExamQuestion>();
            result.Should().BeEquivalentTo(examQuestion);
        }

        [Fact]
        public async Task GetExamById_ReturnsExistingExam()
        {
            //Assert
            var examId = AutoFaker.Generate<int>();
            var exam = AutoFaker.Generate<Exam>();
            _askmeRepository.Setup(x => x.GetExamById(It.IsAny<int>()))
                .ReturnsAsync(exam);

            //Act
            var result = await _sut.GetExamById(examId);

            //Assert
            result.Should().BeOfType<Exam>();
            result.Should().BeEquivalentTo(exam);
        }

        [Fact]
        public async Task GetExams_ReturnsExistingExams()
        {
            //Assert
            var userId = AutoFaker.Generate<int>();
            var exams = AutoFaker.Generate<List<Exam>>();
            _askmeRepository.Setup(x => x.GetExams(It.IsAny<int>()))
                .ReturnsAsync(exams);

            //Act
            var result = await _sut.GetExams(userId);

            //Assert
            result.Should().BeOfType<List<Exam>>();
            result.Should().BeEquivalentTo(exams);
        }

        [Fact]
        public async Task GetExamQuestions_ReturnsExistingExam()
        {
            //Assert
            var examId = AutoFaker.Generate<int>();
            var examQuestions = AutoFaker.Generate<ExamQuestions>();
            _askmeRepository.Setup(x => x.GetExamQuestions(It.IsAny<int>()))
                .ReturnsAsync(examQuestions);

            //Act
            var result = await _sut.GetExamQuestions(examId);

            //Assert
            result.Should().BeOfType<ExamQuestions>();
            result.Should().BeEquivalentTo(examQuestions);
        }
    }
}
