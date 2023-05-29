namespace WhoWantsMillionApp.Services.Exceptions
{
    public class GameLostException : BaseServiceException
    {
        public GameLostException() : base("You lost game", 200)
        {

        }
    }
}
