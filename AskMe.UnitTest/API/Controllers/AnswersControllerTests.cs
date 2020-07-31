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
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AskMe.UnitTest.API.Controllers
{
    public class AnswersControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IAnswerService> _answerService;
        private readonly AnswersController _sut;

        public AnswersControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _answerService = new Mock<IAnswerService>();
            _sut = new AnswersController(_mapper.Object, _answerService.Object);
        }

        [Fact]
        public void AnswersController_Inherits_ControllerBase()
        {
            typeof(AnswersController).Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public void AnswersController_DecoratedWithAutorizeAttribute()
        {
            typeof(AnswersController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void AnswersController_DecoratedWithRouteAttribute()
        {
            typeof(AnswersController).Should().BeDecoratedWith<RouteAttribute>(a => a.Template == "api/questions/{questionId}/answers");
        }

        [Fact]
        public void AnswersController_DecoratedWithApiControllerAttribute()
        {
            typeof(AnswersController).Should().BeDecoratedWith<ApiControllerAttribute>();
        }

        [Fact]
        public async Task GetAnswers_WithQuestionId_ReturnsAnswerList()
        {
            //Arrange
            var questionId = AutoFaker.Generate<int>();
            var answers = AutoFaker.Generate<List<Answer>>();
            var answersDto = AutoFaker.Generate<List<AnswerDto>>();
            _answerService.Setup(x => x.GetAnswers(It.IsAny<int>()))
                .ReturnsAsync(answers);
            _mapper.Setup(x => x.Map<List<AnswerDto>>(answers))
                .Returns(answersDto);

            //Act
            var result = await _sut.GetAnswers(questionId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(answersDto);
        }

        [Fact]
        public async Task GetLesson_WithExistingLessonId_RetunrsLessonsList()
        {
            //Arrange
            var questionId = AutoFaker.Generate<int>();
            var answer = AutoFaker.Generate<Answer>();
            var answerDto = AutoFaker.Generate<AnswerDto>();
            _answerService.Setup(x => x.GetAnswerById(It.IsAny<int>()))
                .ReturnsAsync(answer);
            _mapper.Setup(x => x.Map<AnswerDto>(answer))
                .Returns(answerDto);


            //Act
            var result = await _sut.GetAnswer(questionId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(answerDto);
        }

        [Fact]
        public async Task GetLesson_WithNoExistingLessonId_RetunrsNotFound()
        {
            //Arrange
            var questionId = AutoFaker.Generate<int>();
            var answer = (Answer)null;
            _answerService.Setup(x => x.GetAnswerById(It.IsAny<int>()))
               .ReturnsAsync(answer);

            //Act
            var result = await _sut.GetAnswer(questionId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            ((NotFoundResult)result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateAnswer_ReturnsNewAnswer()
        {
            //Arrange
            var answerForCreation = AutoFaker.Generate<AnswerForCreationDto>();
            var answer = AutoFaker.Generate<Answer>();
            var answerDto = AutoFaker.Generate<AnswerDto>();
            _answerService.Setup(x => x.Create(It.IsAny<Answer>(), It.IsAny<int>()))
                .ReturnsAsync(answer);
            _mapper.Setup(x => x.Map<Answer>(answerForCreation))
                .Returns(answer);
            _mapper.Setup(x => x.Map<AnswerDto>(answer))
                .Returns(answerDto);

            //Action
            var result = await _sut.CreateAnswer(answerForCreation);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(StatusCodes.Status200OK);
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(answerDto);
        }

        [Fact]
        public async Task UpdateLesson_ReturnsLessonUpdated()
        {
            //Arrange
            var answer = AutoFaker.Generate<Answer>();
            var answerDto = AutoFaker.Generate<AnswerDto>();
            _mapper.Setup(x => x.Map<Answer>(answerDto))
                .Returns(answer);
            _answerService.Setup(x => x.UpdateAnswer(answer))
                .ReturnsAsync(true);

            //Act
            var result = await _sut.UpdateAnswer(answerDto);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
