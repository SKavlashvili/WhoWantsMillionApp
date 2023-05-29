using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WhoWantsMillionApp.WebAPI.Infrastructure.Extensions;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddServices();//Custom Services


#region AuthSettings
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer((JwtBearerOptions options) =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey
            (
                Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:SecretKey"))
            ),
            ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
            RoleClaimType = "Role" //This will be the name of the role parameter
        };
    });
#endregion


var app = builder.Build();

app.UseGlobalErrorHandler();

app.MapControllers();

#region AddingAuthMiddlewares
app.UseAuthentication();
app.UseAuthorization();
#endregion


app.Run();
