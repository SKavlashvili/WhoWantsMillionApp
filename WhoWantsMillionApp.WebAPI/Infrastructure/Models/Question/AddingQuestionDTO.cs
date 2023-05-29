namespace WhoWantsMillionApp.WebAPI.Infrastructure.Models
{
    public class AddingQuestionDTO
    {
        public string Question { get; set; }
        public string[] MultipleChoiceAnswers { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
