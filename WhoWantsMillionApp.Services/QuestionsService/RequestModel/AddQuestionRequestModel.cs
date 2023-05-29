namespace WhoWantsMillionApp.Services.Models
{
    public class AddQuestionRequestModel
    {
        public string OwnerID { get; set; }
        public string Question { get; set; }
        public string[] MultipleChoiceAnswers { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
