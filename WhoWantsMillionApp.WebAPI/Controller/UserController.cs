using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using WhoWantsMillionApp.WebAPI.Infrastructure.Auth;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models;
using WhoWantsMillionApp.Services;
using WhoWantsMillionApp.Services.Models;
using System.Security.Claims;
using Mapster;
using WhoWantsMillionApp.WebAPI.Infrastructure.Mappers;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models.Question;

namespace WhoWantsMillionApp.WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private IJwt _jwtMethods;
        private IQuestionService _questionsService;
        private IConfiguration _configuration;
        public UserController(IJwt jwtMethods, IQuestionService questionsService, IConfiguration configs)
        {
            this._configuration = configs;
            this._jwtMethods = jwtMethods;
            _questionsService = questionsService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddQuestion([FromBody] AddingQuestionDTO question,
            [FromServices] IValidator<AddingQuestionDTO> validator, CancellationToken token)
        {
            ValidationResult res = await validator.ValidateAsync(question,token);
            if (!res.IsValid) return BadRequest(res.Errors);
            string userID = this._jwtMethods.GetPropertyFromJWT<string>(Request.Headers["Authorization"], "ID");
            AddQuestionRequestModel newQuestion = question.Adapt<AddQuestionRequestModel>(AddingQuestionDTOToEverything.AddingQuestionDTOToAddingQuestionRequestModel);
            newQuestion.OwnerID = userID;
            return Ok(await _questionsService.AddQuestion(newQuestion, token));
        }

        [HttpGet("[action]")]//This action method returns GameStarterToken
        public async Task<IActionResult> StartGame(CancellationToken token)
        {
            string UserID = _jwtMethods.GetPropertyFromJWT<string>(Request.Headers["Authorization"], "ID");
            int TimeDuration = _configuration.GetValue<int>("GameSettings:TimeForOneQuestionInMinutes");
            int levelsAmount = _configuration.GetValue<int>("GameSettings:LevelsAmount");
            string RandomQuestionIDs = await _questionsService.GetRandomQuestions(levelsAmount,token);
            Claim[] claims =
            {
                new Claim("UserID", UserID),
                new Claim("Role", "InGameUser"),
                new Claim("CurrentLevel", "0"),
                new Claim("GameID", Guid.NewGuid().ToString()),
                new Claim("RandomQuestionIDs",RandomQuestionIDs)
            };
            GetNextQuestionDTO questionDTO = (await _questionsService.GetQuestion(RandomQuestionIDs.Split(',')[0], token))
                        .Adapt<GetNextQuestionDTO>(AddingQuestionDTOToEverything.QuestionEntityToNextQuestionDTO);
            questionDTO.TokenForNextQuestion = _jwtMethods.GenerateJWTToken(TimeDuration, claims);
            return Ok(questionDTO);
        }

    }
}
