using Adn.Utilities;
using Dapper;
using User.Models;

namespace Adn.Repositories;

public interface IUserRepository
{
  Task<Users> GetByUserName(string UserName);
}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {
        
    }

    public async Task<Users> GetByUserName(string UserName)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}""
        WHERE username = @UserName";

        using (var con = NewConnection) 

         return await con.QuerySingleOrDefaultAsync<Users>(query, new { UserName });


    }
}