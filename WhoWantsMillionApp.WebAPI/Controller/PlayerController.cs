using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using WhoWantsMillionApp.Services;
using WhoWantsMillionApp.WebAPI.Infrastructure.Auth;
using WhoWantsMillionApp.WebAPI.Infrastructure.Mappers;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models.Question;

namespace WhoWantsMillionApp.WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "InGameUser")]
    public class PlayerController : ControllerBase
    {
        private IJwt _jwtMethods;
        private IGamesService _gameService;
        private IUserService _userService;
        private IQuestionService _questionsService;
        private IConfiguration _configuration;
        public PlayerController(IJwt jwtMethods, IGamesService gameService, IUserService userService, IQuestionService questionService
                                    ,IConfiguration configs)
        {
            _questionsService = questionService;
            _gameService = gameService;
            _configuration = configs;
            _userService = userService;
            this._jwtMethods = jwtMethods;
        }

        [HttpGet("[action]/{answer}")]
        public async Task<IActionResult> GetNextQuestionAndAnswerCurrent(string answer,CancellationToken token)
        {
            string jwtToken = Request.Headers["Authorization"];
            string GameID = _jwtMethods.GetPropertyFromJWT<string>(jwtToken, "GameID");

            if (await _gameService.GameFinished(GameID, token)) _gameService.GameFinishedException();
            
            string UserID = _jwtMethods.GetPropertyFromJWT<string>(jwtToken, "UserID");
            string RandomQuestionIDs = _jwtMethods.GetPropertyFromJWT<string>(jwtToken, "RandomQuestionIDs");
            string[] questionIDs = RandomQuestionIDs.Split(',');
            int CurrentLevel = Convert.ToInt32(_jwtMethods.GetPropertyFromJWT<string>(jwtToken, "CurrentLevel"));
            string CorrectAnswer = await _questionsService.GetCorrectAnswer(questionIDs[CurrentLevel],token);
            if(!answer.Equals(CorrectAnswer))
            {
                await _gameService.GameLost(GameID,token);
            }
            if(CurrentLevel == questionIDs.Length - 1)
            {
                await _gameService.GameWon(CurrentLevel + 1,GameID,UserID,token);
            }
            int TimeDuration = _configuration.GetValue<int>("GameSettings:TimeForOneQuestionInMinutes");
            GetNextQuestionDTO nextQuestionDTO = (await _questionsService.GetQuestion(questionIDs[CurrentLevel + 1], token))
                        .Adapt<GetNextQuestionDTO>(AddingQuestionDTOToEverything.QuestionEntityToNextQuestionDTO);
            Claim[] claims =
            {
                new Claim("UserID", UserID),
                new Claim("Role", "InGameUser"),
                new Claim("CurrentLevel", (CurrentLevel + 1).ToString()),
                new Claim("GameID", GameID),
                new Claim("RandomQuestionIDs",RandomQuestionIDs)
            };
            nextQuestionDTO.TokenForNextQuestion = _jwtMethods.GenerateJWTToken(TimeDuration, claims);
            return Ok(nextQuestionDTO);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> EndGame(CancellationToken token)
        {
            string jwtToken = Request.Headers["Authorization"];
            string GameID = _jwtMethods.GetPropertyFromJWT<string>(jwtToken, "GameID");
            if (await _gameService.GameFinished(GameID, token)) _gameService.GameFinishedException();
            string UserID = _jwtMethods.GetPropertyFromJWT<string>(jwtToken, "UserID");
            int CurrentLevel = Convert.ToInt32(_jwtMethods.GetPropertyFromJWT<string>(jwtToken, "CurrentLevel"));
            if (CurrentLevel == 0) _gameService.YouHaveNotStartedGameException();
            await _gameService.GameWon(CurrentLevel, GameID, UserID, token);//throws GameWon exception
            return Ok();
        }
        
    }
}
