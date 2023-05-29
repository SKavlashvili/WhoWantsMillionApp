namespace WhoWantsMillionApp.Services.Exceptions
{
    public class YouHaveNotStartedGameException : BaseServiceException
    {
        public YouHaveNotStartedGameException() : base("game is not started",400)
        {

        }
    }
}
