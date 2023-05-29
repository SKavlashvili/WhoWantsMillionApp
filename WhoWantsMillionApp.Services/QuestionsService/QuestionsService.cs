using Mapster;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhoWantsMillionApp.Services.DBModels;
using WhoWantsMillionApp.Services.Exceptions;
using WhoWantsMillionApp.Services.Models;

namespace WhoWantsMillionApp.Services
{
    public class QuestionsService : IQuestionService
    {
        private IConfiguration _configuration;
        private static JsonSerializerOptions _serializeOpetions;
        private string _questionsPath;
        static QuestionsService()
        {
            _serializeOpetions = new JsonSerializerOptions() { WriteIndented = true };
        }
        public QuestionsService(IConfiguration configs)
        {
            this._questionsPath = configs.GetValue<string>("DB:QuestionsTable");
            this._configuration = configs;
        }

        public async Task<string> AddQuestion(AddQuestionRequestModel question,CancellationToken token)
        {
            List<QuestionEntity> entities = await GetAllQuestions(token);
            entities.Add(question.Adapt<QuestionEntity>(QuestionRequestModelToEverything.QuestionsRequestModelToQuestionsEntity));
            
            await File.WriteAllTextAsync(_questionsPath, JsonSerializer.Serialize(entities, _serializeOpetions), token);

            return entities[entities.Count - 1].ID;
        }

        public async Task<List<QuestionEntity>> GetAllQuestions(CancellationToken token)
        {
            string FullTable = await File.ReadAllTextAsync(_questionsPath, token);
            List<QuestionEntity> questions; 
            if(string.IsNullOrEmpty(FullTable)) questions = new List<QuestionEntity>();
            else questions = JsonSerializer.Deserialize<List<QuestionEntity>>(FullTable);
            return questions;
        }

        public async Task<string> GetRandomQuestions(int amount, CancellationToken token)
        {
            List<QuestionEntity> questions = await GetAllQuestions(token);
            if (questions.Count < amount) throw new NotEnoughQuestionsException();
            ShuffleList(questions);
            StringBuilder RandomQuestions = new StringBuilder("");
            for(int i = 0; i <  amount - 1; i++)
            {
                RandomQuestions.Append(questions[i].ID);
                RandomQuestions.Append(',');
            }
            RandomQuestions.Append(questions[amount - 1].ID);
            return RandomQuestions.ToString();
        }
        public async Task<QuestionEntity> GetQuestion(string questionID, CancellationToken token)
        {
            List<QuestionEntity> questionEntities = await GetAllQuestions(token);
            QuestionEntity question = questionEntities.Single(q => q.ID.Equals(questionID));
            List<string> answers = question.MultipleChoiceAnswers.ToList();
            ShuffleList(answers);
            question.MultipleChoiceAnswers = answers.ToArray();//Shuffled
            return question;
        }
        public async Task<string> GetCorrectAnswer(string questionID, CancellationToken token)
        {
            QuestionEntity entity = await GetQuestion(questionID,token);
            return entity.CorrectAnswer;
        }
        public static void ShuffleList<T>(List<T> values)
        {
            Random random = new Random();
            for(int i = 0; i < values.Count; i++)
            {
                int randomIndex = random.Next(0, values.Count);
                Swap(values, i, randomIndex);
            }
        }

        public static void Swap<T>(List<T> collection, int index1, int index2)
        {
            T tempElement = collection[index1];
            collection[index1] = collection[index2];
            collection[index2] = tempElement;
        }
    }
}
