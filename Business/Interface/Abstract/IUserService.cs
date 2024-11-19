using Entitiy.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface.Abstract
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<List<User>> GetUsers();
        Task<User> GetUserById(int userId);
        Task UpdateUser(User user);
        Task DeleteUser(int userId);
        User UserLog(User user);
        bool CheckCrossCurrencyAuthorization(User user);
        bool CheckNormalCurrencyAuthorization(User user);

    }
}
