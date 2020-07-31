using AskMe.API.Controllers;
using AskMe.API.Models;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AutoBogus;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AskMe.UnitTest.API.Controllers
{
    public class QuestionsControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IQuestionService> _questionService;
        private readonly QuestionsController _sut;

        public QuestionsControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _questionService = new Mock<IQuestionService>();
            _sut = new QuestionsController(_mapper.Object, _questionService.Object);
        }

        [Fact]
        public void QuestionsController_Inherits_ControllerBase()
        {
            typeof(QuestionsController).Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public void QuestionsController_DecoratedWithAutorizeAttribute()
        {
            typeof(QuestionsController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void QuestionsController_DecoratedWithRouteAttribute()
        {
            typeof(QuestionsController).Should().BeDecoratedWith<RouteAttribute>(a => a.Template == "api/lessons/{lessonId}/questions");
        }

        [Fact]
        public void QuestionsController_DecoratedWithApiControllerAttribute()
        {
            typeof(QuestionsController).Should().BeDecoratedWith<ApiControllerAttribute>();
        }

        [Fact]
        public async Task GetQuestions_WithLessonId_RetunrsQuestionsList()
        {
            //Arrange
            var lessonId = AutoFaker.Generate<int>();
            var questions = AutoFaker.Generate<List<Question>>();
            var questionsDto = AutoFaker.Generate<List<QuestionDto>>();
            _questionService.Setup(x => x.GetQuestions(It.IsAny<int>()))
                .ReturnsAsync(questions);
            _mapper.Setup(x => x.Map<List<QuestionDto>>(questions))
                .Returns(questionsDto);

            //Act
            var result = await _sut.GetQuestions(lessonId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(questionsDto);
        }

        [Fact]
        public async Task GetQuestion_WithExistingQuestionId_RetunrsQuestionsList()
        {
            //Arrange
            var questionId = AutoFaker.Generate<int>();
            var question = AutoFaker.Generate<Question>();
            var questionDto = AutoFaker.Generate<QuestionDto>();
            _questionService.Setup(x => x.GetQuestionById(It.IsAny<int>()))
                .ReturnsAsync(question);
            _mapper.Setup(x => x.Map<QuestionDto>(question))
                .Returns(questionDto);


            //Act
            var result = await _sut.GetQuestion(questionId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(questionDto);
        }

        [Fact]
        public async Task GetQuestion_WithNoExistingQuestionId_RetunrsNotFound()
        {
            //Arrange
            var questionId = AutoFaker.Generate<int>();
            var lesson = (Question)null;
            _questionService.Setup(x => x.GetQuestionById(It.IsAny<int>()))
                .ReturnsAsync(lesson);

            //Act
            var result = await _sut.GetQuestion(questionId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            ((NotFoundResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateQuestion_ReturnsNewQuestion()
        {
            //Arrange
            var questionForCreation = AutoFaker.Generate<QuestionForCreationDto>();
            var question = AutoFaker.Generate<Question>();
            var questionDto = AutoFaker.Generate<QuestionDto>();
            _questionService.Setup(x => x.Create(It.IsAny<Question>(), It.IsAny<int>()))
                .ReturnsAsync(question);
            _mapper.Setup(x => x.Map<Question>(questionForCreation))
                .Returns(question);
            _mapper.Setup(x => x.Map<QuestionDto>(question))
                .Returns(questionDto);

            //Action
            var result = await _sut.CreateQuestion(questionForCreation);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(questionDto);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsQuestionUpdated()
        {
            //Arrange
            var question = AutoFaker.Generate<Question>();
            var questionDto = AutoFaker.Generate<QuestionDto>();
            _mapper.Setup(x => x.Map<Question>(questionDto))
                .Returns(question);
            _questionService.Setup(x => x.UpdateQuestion(question))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateQuestion(questionDto);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
