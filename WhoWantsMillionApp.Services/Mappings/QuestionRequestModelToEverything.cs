using Mapster;
using WhoWantsMillionApp.Services.DBModels;
using WhoWantsMillionApp.Services.Models;

namespace WhoWantsMillionApp.Services
{
    public static class QuestionRequestModelToEverything
    {
        public static TypeAdapterConfig QuestionsRequestModelToQuestionsEntity;

        static QuestionRequestModelToEverything()
        {
            QuestionsRequestModelToQuestionsEntity = new TypeAdapterConfig();

            QuestionsRequestModelToQuestionsEntity.ForType<AddQuestionRequestModel, QuestionEntity>()
                .Map((QuestionEntity dest) => dest.ID, (AddQuestionRequestModel src) => Guid.NewGuid().ToString())
                .Map(dest => dest.Question, src => src.Question)
                .Map(dest => dest.MultipleChoiceAnswers, src => src.MultipleChoiceAnswers)
                .Map(dest => dest.OwnerID, src => src.OwnerID)
                .Map(dest => dest.CorrectAnswer, src => src.CorrectAnswer);
        }
    }
}
