using FluentValidation;
using WhoWantsMillionApp.WebAPI.Infrastructure.Auth;
using WhoWantsMillionApp.WebAPI.Infrastructure.Validations;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models;
using WhoWantsMillionApp.WebAPI.Infrastructure.Validations.User;
using WhoWantsMillionApp.Services;
using WhoWantsMillionApp.WebAPI.Infrastructure.Validations;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            //Auth
            services.AddSingleton<IJwt, Jwt>();
            
            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionsService>();
            services.AddScoped<IGamesService, GamesService>();
            
            //Validators
            services.AddSingleton<IValidator<UserRegistrationDTO>, UserRegistrationDTOValidator>();
            services.AddSingleton<IValidator<UserLoginDTO>, UserLoginDTOValidator>();
            services.AddSingleton<IValidator<AddingQuestionDTO>, AddingQuestionDTOValidator>();
        }
    }
}
