namespace WhoWantsMillionApp.Services.Exceptions
{
    public class NotEnoughQuestionsException : BaseServiceException
    {
        public NotEnoughQuestionsException() : base("there are not enough questions, please add more questions",400)
        {

        }
    }
}
