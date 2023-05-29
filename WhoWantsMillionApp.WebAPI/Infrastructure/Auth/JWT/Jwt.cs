using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WhoWantsMillionApp.WebAPI.Infrastructure.Auth
{
    public class Jwt : IJwt
    {
        private IConfiguration _configuration;
        public Jwt(IConfiguration configs)
        {
            _configuration = configs;
        }
        public string GenerateJWTToken(int TimeDurationMinutes, params Claim[] claims)
        {
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:SecretKey")));
            string issuer = _configuration.GetValue<string>("JWT:Issuer");
            string audience = _configuration.GetValue<string>("JWT:Audience");
            SigningCredentials SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: issuer,
                    audience: audience,
                    signingCredentials: SigningCredentials,
                    expires: DateTime.Now.AddMinutes(TimeDurationMinutes),
                    claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public byte[] ConvertFromBase64ToBytes(string input)
        {
            if (String.IsNullOrWhiteSpace(input)) return null;
            try
            {
                string working = input.Replace('-', '+').Replace('_', '/'); ;
                while (working.Length % 4 != 0)
                {
                    working += '=';
                }
                return Convert.FromBase64String(working);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetClaimsAsJson(string token)
        {
            string ClaimsBase64 = token.Split('.')[1];
            string ClaimsAsJson = Encoding.UTF8.GetString(ConvertFromBase64ToBytes(ClaimsBase64));
            return ClaimsAsJson;
        }

        public T GetPropertyFromJWT<T>(string token, string propertyName)
        {
            string ClaimsAsJson = GetClaimsAsJson(token);
            IConfiguration ClaimsObject = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(ClaimsAsJson)))
                .Build();
            return ClaimsObject.GetValue<T>(propertyName);
        }
    }
}
