using Mapster;
using WhoWantsMillionApp.Services;
using WhoWantsMillionApp.Services.DBModels;
using WhoWantsMillionApp.Services.Models;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models.Question;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Mappers
{
    public static class AddingQuestionDTOToEverything
    {
        public static TypeAdapterConfig AddingQuestionDTOToAddingQuestionRequestModel;
        public static TypeAdapterConfig QuestionEntityToNextQuestionDTO;

        static AddingQuestionDTOToEverything()
        {
            AddingQuestionDTOToAddingQuestionRequestModel = new TypeAdapterConfig();

            AddingQuestionDTOToAddingQuestionRequestModel.ForType<AddingQuestionDTO, AddQuestionRequestModel>()
                .Map((AddQuestionRequestModel destination) => destination.Question, (AddingQuestionDTO source) => source.Question)
                .Map((AddQuestionRequestModel destination) => destination.MultipleChoiceAnswers, (AddingQuestionDTO source) => source.MultipleChoiceAnswers)
                .Map(destination => destination.CorrectAnswer, src => src.CorrectAnswer);

            QuestionEntityToNextQuestionDTO = new TypeAdapterConfig();
            QuestionEntityToNextQuestionDTO.ForType<QuestionEntity, GetNextQuestionDTO>()
                .Map(dest => dest.Question, src => src.Question)
                .Map(dest => dest.MultiplChoiceAnswers, src => src.MultipleChoiceAnswers);

        }
    }
}
