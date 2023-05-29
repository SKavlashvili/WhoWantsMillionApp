namespace WhoWantsMillionApp.Services.DBModels
{
    public class QuestionEntity
    {
        public string ID { get; set; }
        public string OwnerID { get; set; }
        public string Question { get; set; }
        public string[] MultipleChoiceAnswers { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
