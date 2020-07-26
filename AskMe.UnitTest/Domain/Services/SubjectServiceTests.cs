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
    public class SubjectServiceTests
    {
        private readonly Mock<IAskMeRepository> _askmeRepository;
        private readonly SubjectService _sut;

        public SubjectServiceTests()
        {
            _askmeRepository = new Mock<IAskMeRepository>();
            _sut = new SubjectService(_askmeRepository.Object);
        }

        [Fact]
        public async Task Create_ReturnsNewSubject()
        {
            //Arrange
            var subject = AutoFaker.Generate<Subject>();
            var userId = AutoFaker.Generate<int>();

            _askmeRepository.Setup(x => x.AddSubject(It.IsAny<Subject>(),It.IsAny<int>()))
                .ReturnsAsync(subject);

            //Act
            var result = await _sut.Create(subject,userId);

            //Assert
            result.Should().BeOfType<Subject>();
            result.Should().BeEquivalentTo(subject);
        }

        [Fact]
        public async Task GetSubjectById_ReturnExistingSubject()
        {
            //Arrange
            var subject = AutoFaker.Generate<Subject>();
            var subjectId = AutoFaker.Generate<int>();
            _askmeRepository.Setup(x => x.GetSubjectById(It.IsAny<int>()))
                .ReturnsAsync(subject);

            //Act
            var result = await _sut.GetSubjectById(subjectId);

            //Assert
            result.Should().BeOfType<Subject>();
            result.Should().BeEquivalentTo(subject);
        }

        [Fact]
        public async Task GetSubjects_ReturnExistingsubjectList()
        {
            //Arrange
            var subjects = AutoFaker.Generate<List<Subject>>();
            var userId = AutoFaker.Generate<int>();

            _askmeRepository.Setup(x => x.GetSubjects(It.IsAny<int>()))
                .ReturnsAsync(subjects);

            //Act
            var result = await _sut.GetSubjects(userId);

            //Assert
            result.Should().BeOfType<List<Subject>>();
            result.Should().BeEquivalentTo(subjects);
        }

        [Fact]
        public async Task UpdateSubject_ReturnsUpdatedsubject()
        {
            //Arrange
            var subject = AutoFaker.Generate<Subject>();
            _askmeRepository.Setup(x => x.UpdateSubject(It.IsAny<Subject>()))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateSubject(subject);

            //Assert
            result.Should().BeTrue();
        }
    }
}
