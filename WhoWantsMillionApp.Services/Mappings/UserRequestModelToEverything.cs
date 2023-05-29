using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWantsMillionApp.Services.Models;

namespace WhoWantsMillionApp.Services
{
    public static class UserRequestModelToEverything
    {
        public static TypeAdapterConfig UserRequestModelToUserEntity;

        static UserRequestModelToEverything()
        {
            UserRequestModelToUserEntity = new TypeAdapterConfig();

            UserRequestModelToUserEntity.ForType<UserRequestModel, UserEntity>()
                .Map(dest => dest.ID, src => Guid.NewGuid().ToString())
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.UserName, src => src.Password);
        }
    }
}
