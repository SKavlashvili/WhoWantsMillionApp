namespace WhoWantsMillionApp.Services.Exceptions
{
    public class GameHasAlreadyFinishedException : BaseServiceException
    {
        public GameHasAlreadyFinishedException() : base("This game is already finished",400)
        {

        }
    }
}
