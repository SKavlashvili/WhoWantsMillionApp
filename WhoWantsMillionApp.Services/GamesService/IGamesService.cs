using WhoWantsMillionApp.Services.Models;

namespace WhoWantsMillionApp.Services
{
    public interface IGamesService
    {
        public Task<List<string>> GetFinishedGames(CancellationToken token);

        public Task AddFinishedGame(string GameID, CancellationToken token);

        public Task<bool> GameFinished(string gameID, CancellationToken token);

        public Task GameWon(int levelsAmount, string GameID, string userID, CancellationToken token);
        public Task GameLost(string GameID, CancellationToken token);

        public void GameFinishedException();

        public Task<List<string>> GetLeaderBoard(int topN, CancellationToken token);

        public void YouHaveNotStartedGameException();
    }
}
