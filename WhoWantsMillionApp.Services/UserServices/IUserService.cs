using WhoWantsMillionApp.Services.Models;

namespace WhoWantsMillionApp.Services
{
    public interface IUserService
    {
        Task<string> RegisterUser(UserRequestModel newUser, CancellationToken token);

        Task<bool> UserExists(Func<UserEntity,bool> predicate, CancellationToken token);

        Task<List<UserEntity>> GetAllUsers(CancellationToken token);

        Task<string> GetUserIDWhere(Func<UserEntity, bool> predicate, CancellationToken token);

        Task<decimal> AddMoney(string UserID, int levelsAmount, CancellationToken token);
    }
}
