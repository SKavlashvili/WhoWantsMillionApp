using Microsoft.Extensions.Configuration;
using System.Text.Json;
using WhoWantsMillionApp.Services.Models;
using WhoWantsMillionApp.Services.Exceptions;
using Mapster;

namespace WhoWantsMillionApp.Services
{
    public class UserService : IUserService
    {
        private static JsonSerializerOptions _serializeOpetions;
        private IConfiguration _configs;
        static UserService()
        {
            _serializeOpetions = new JsonSerializerOptions() { WriteIndented = true };
        }
        public UserService(IConfiguration configs)
        {
            this._configs = configs;
        }
        public async Task<string> RegisterUser(UserRequestModel newUser, CancellationToken token)
        {
            if (await UserExists(u => u.UserName.Equals(newUser.UserName), token)) 
                throw new UserAlreadyExistsException();

            Task<List<UserEntity>> usersTask = GetAllUsers(token);

            UserEntity newUserInstance = newUser.Adapt<UserEntity>(UserRequestModelToEverything.UserRequestModelToUserEntity);
            
            List<UserEntity> users = await usersTask;

            users.Add(newUserInstance);

            string newTable = JsonSerializer.Serialize(users,_serializeOpetions);

            await File.WriteAllTextAsync(_configs.GetValue<string>("DB:UserTable"), newTable, token);
            
            return newUserInstance.ID;
        }

        public async Task<bool> UserExists(Func<UserEntity, bool> predicate, CancellationToken token)
        {
            List<UserEntity> users = await GetAllUsers(token);
            foreach(UserEntity user in users)
            {
                if(predicate.Invoke(user)) return true;
            }
            return false;
        }

        public async Task<List<UserEntity>> GetAllUsers(CancellationToken token)
        {
            string? userTablePath = _configs.GetValue<string>("DB:UserTable");

            string usersData = await File.ReadAllTextAsync(userTablePath,token);

            if (string.IsNullOrEmpty(usersData)) return new List<UserEntity>();

            return JsonSerializer.Deserialize<List<UserEntity>>(usersData);

        }


        public async Task<string> GetUserIDWhere(Func<UserEntity, bool> predicate, CancellationToken token)
        {
            List<UserEntity> users = await GetAllUsers(token);
            foreach(UserEntity user in users)
            {
                if (predicate.Invoke(user)) return user.ID;
            }
            throw new UserNotFoundException();
        }

        public async Task<decimal> AddMoney(string UserID, int levelsAmount, CancellationToken token)
        {
            List<UserEntity> users = await GetAllUsers(token);
            UserEntity user = users.Single(u => u.ID.Equals(UserID));
            decimal amount = 0;
            for(int i = 1; i <= levelsAmount; i++)
            {
               amount += _configs.GetValue<int>("GameSettings:Question" + i);
            }
            user.UserBalance += amount;
            string jsonTextUsers = JsonSerializer.Serialize(users, _serializeOpetions);
            await File.WriteAllTextAsync(_configs.GetValue<string>("DB:UserTable"), jsonTextUsers, token);
            return amount;
        }
    }
}
