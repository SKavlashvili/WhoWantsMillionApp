using Mapster;
using System.ComponentModel.Design;
using WhoWantsMillionApp.Services.Models;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Mappers
{
    public static class UserRegistrationDTOToEverything
    {
        public static TypeAdapterConfig UserRegistrationDTOToUserRequestModel;

        static UserRegistrationDTOToEverything()
        {
            UserRegistrationDTOToUserRequestModel = new TypeAdapterConfig();

            UserRegistrationDTOToUserRequestModel.ForType<UserRegistrationDTO, UserRequestModel>()
                .Map(destination => destination.UserName, source => source.UserName)
                .Map(destination => destination.Password, source => source.Password);
        }
    }
}
