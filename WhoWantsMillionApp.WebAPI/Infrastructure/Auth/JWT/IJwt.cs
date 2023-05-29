using System.Security.Claims;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Auth
{
    public interface IJwt
    {
        string GenerateJWTToken(int TimeDurationMinutes, params Claim[] claims);

        string GetClaimsAsJson(string token);

        public T GetPropertyFromJWT<T>(string token, string propertyName);
    }
}
