using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhoWantsMillionApp.Services.Exceptions;
using WhoWantsMillionApp.Services.Models;

namespace WhoWantsMillionApp.Services
{
    public class GamesService : IGamesService
    {
        private static JsonSerializerOptions _options;
        private string _gamesTablePath;
        private IUserService _usersService;
        private IConfiguration _configs;
        public GamesService(IConfiguration configs, IUserService usersService)
        {
            _configs = configs;
            _options = new JsonSerializerOptions() {  WriteIndented = true };
            _usersService = usersService;
            _gamesTablePath = configs.GetValue<string>("DB:GamesTable");
        }
        public async Task AddFinishedGame(string GameID, CancellationToken token)
        {
            List<string> Games = await GetFinishedGames(token);
            Games.Add(GameID);
            string newFileText = JsonSerializer.Serialize(Games,_options);
            await File.WriteAllTextAsync(_gamesTablePath, newFileText,token);
        }

        public async Task<bool> GameFinished(string gameID, CancellationToken token)
        {
            List<string> Games = await GetFinishedGames(token);
            return Games.Contains(gameID);
        }

        public async Task GameLost(string GameID, CancellationToken token)
        {
            await AddFinishedGame(GameID, token);
            throw new GameLostException();
        }

        public async Task GameWon(int levelsAmount, string GameID, string userID, CancellationToken token)
        {
            await AddFinishedGame(GameID, token);
            decimal amount = await _usersService.AddMoney(userID,levelsAmount, token);
            throw new GameWonException(amount, JsonSerializer.Serialize(await GetLeaderBoard(10, token)));
        }

        public async Task<List<string>> GetFinishedGames(CancellationToken token)
        {
            string FullFile = await File.ReadAllTextAsync(_gamesTablePath, token);
            if (string.IsNullOrEmpty(FullFile)) return new List<string>();
            else return JsonSerializer.Deserialize<List<string>>(FullFile);
        }

        public void GameFinishedException()
        {
            throw new GameHasAlreadyFinishedException();
        }

        public async Task<List<string>> GetLeaderBoard(int topN,CancellationToken token)
        {
            List<UserEntity> users = await _usersService.GetAllUsers(token);
            users.Sort((x, y) => x.UserBalance.CompareTo(y.UserBalance));
            if (topN > users.Count) return users.Select(u => u.UserName).ToList();
            return users.GetRange(0, topN).Select(u => u.UserName).ToList();
        }

        public void YouHaveNotStartedGameException()
        {
            throw new YouHaveNotStartedGameException();
        }
    }
}
