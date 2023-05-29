using WhoWantsMillionApp.Services.DBModels;
using WhoWantsMillionApp.Services.Models;

namespace WhoWantsMillionApp.Services
{
    public interface IQuestionService
    {
        public Task<string> AddQuestion(AddQuestionRequestModel question, CancellationToken token);
        public Task<List<QuestionEntity>> GetAllQuestions(CancellationToken token);

        public Task<string> GetRandomQuestions(int amount, CancellationToken token);

        public Task<QuestionEntity> GetQuestion(string questionID, CancellationToken token);

        public Task<string> GetCorrectAnswer(string questionID, CancellationToken token);
    }
}
