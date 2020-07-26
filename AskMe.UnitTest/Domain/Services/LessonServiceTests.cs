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
    public class LessonServiceTests
    {
        private readonly Mock<IAskMeRepository> _askmeRepository;
        private readonly LessonService _sut;

        public LessonServiceTests()
        {
            _askmeRepository = new Mock<IAskMeRepository>();
            _sut = new LessonService(_askmeRepository.Object);
        }

        [Fact]
        public async Task Create_ReturnsNewLesson()
        {
            //Arrange
            var lesson = AutoFaker.Generate<Lesson>();
            var subjectId = AutoFaker.Generate<int>();
            _askmeRepository.Setup(x => x.AddLesson(It.IsAny<Lesson>(), It.IsAny<int>()))
                .ReturnsAsync(lesson);

            //Act
            var result = await _sut.Create(lesson, subjectId);

            //Assert
            result.Should().BeOfType<Lesson>();
            result.Should().BeEquivalentTo(lesson);
        }

        [Fact]
        public async Task GetLessonById_ReturnsExistingLessonAnswer()
        {
            //Arrange
            var lesson = AutoFaker.Generate<Lesson>();
            var lessonId = AutoFaker.Generate<int>();
            _askmeRepository.Setup(x => x.GetLessonById(It.IsAny<int>()))
                .ReturnsAsync(lesson);

            //Act
            var result = await _sut.GetLessonById(lessonId);

            //Assert
            result.Should().BeOfType<Lesson>();
            result.Should().BeEquivalentTo(lesson);
        }

        [Fact]
        public async Task GetLessons_ReturnsLessonList()
        {
            //Arrange
            var subjectId = AutoFaker.Generate<int>();
            var lessons = AutoFaker.Generate<List<Lesson>>();
            _askmeRepository.Setup(x => x.GetLessons(It.IsAny<int>()))
                .ReturnsAsync(lessons);

            //Act
            var result = await _sut.GetLessons(subjectId);

            //Assert
            result.Should().BeOfType<List<Lesson>>();
            result.Should().BeEquivalentTo(lessons);
        }

        [Fact]
        public async Task UpdateLesson_ReturnsLessonUpdated()
        {
            //Arrange
            var lesson = AutoFaker.Generate<Lesson>();
            _askmeRepository.Setup(x => x.UpdateLesson(It.IsAny<Lesson>()))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateLesson(lesson);

            //Assert
            result.Should().BeTrue();
        }
    }
}
