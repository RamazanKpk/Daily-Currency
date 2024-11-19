using Business.Interface.Abstract;
using DataAccess;
using Entitiy.Concrate;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Interface.Concrete
{
    public class UserService : IUserService
    {
        private readonly DailyCurrencyContext _db;

        public UserService(DailyCurrencyContext db)
        {
            _db = db;
        }

        public async Task AddUser(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _db.Users.FindAsync(userId);
        }

        public async Task UpdateUser(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }
        public User UserLog(User user)
        {
            var userLog = _db.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            return userLog;
        }
        public bool CheckCrossCurrencyAuthorization(User user)
        {
            if (user == null)
            {
                return false;
            }
            return user.CrossExchangeRatesAuthorization;
        }
        public bool CheckNormalCurrencyAuthorization(User user)
        {
            if (user== null)
            {
                return false;
            }
            return user.NormalExchangeRatesAuthorization;
        }
    }
}
