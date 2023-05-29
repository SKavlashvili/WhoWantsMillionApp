

namespace WhoWantsMillionApp.Services.Exceptions
{
    public class GameWonException : BaseServiceException
    {
        public GameWonException(decimal amount, string leaderBoard) : 
            base($"congragulations, you won {amount} dollars\nLeader board:\n" + leaderBoard,200)
        {

        }
    }
}
