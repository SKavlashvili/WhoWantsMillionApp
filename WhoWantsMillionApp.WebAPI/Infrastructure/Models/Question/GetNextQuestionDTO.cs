namespace WhoWantsMillionApp.WebAPI.Infrastructure.Models.Question
{
    public class GetNextQuestionDTO
    {
        public string Question { get; set; }
        public string[] MultiplChoiceAnswers { get; set; }
        public string TokenForNextQuestion { get; set; }
    }
}
