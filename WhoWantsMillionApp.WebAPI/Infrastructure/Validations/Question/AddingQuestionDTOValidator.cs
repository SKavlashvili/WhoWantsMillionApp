using FluentValidation;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Validations
{
    public class AddingQuestionDTOValidator : AbstractValidator<AddingQuestionDTO>
    {
        public AddingQuestionDTOValidator()
        {
            RuleFor(q => q.MultipleChoiceAnswers)
                .Must(answers => answers != null && answers.Length == 4)
                .WithMessage("MultipleChoiseAnswers length should be 4");

            RuleForEach(q => q.MultipleChoiceAnswers)
                .Must((string item) => !string.IsNullOrEmpty(item) && item.Length < 100)
                .WithMessage("item should't be null and it's length should be less then 100");

            RuleFor(q => q.Question)
                .Must(question => !string.IsNullOrEmpty(question) && question.Length < 100)
                .WithMessage("question should't be null and it's length should be less then 100");

            RuleFor(q => q.CorrectAnswer)
                .Must((string item) => !string.IsNullOrEmpty(item) && item.Length < 100)
                .WithMessage("item should't be null and it's length should be less then 100");

        }
    }
}

