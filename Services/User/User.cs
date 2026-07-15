using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTemplates.Services.User
{
    public class User : IUserService {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<User> _logger;

        public User(ApplicationDBContext context, ILogger<User> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<Models.User> Login(Models.User user) {
            if(user == null || user.Email == null || user.Password == null) return null;
            user.Password = HashText.GetHashString(user.Password);
            return await _context.Users.Where(u => (u.Email == user.Email) && (u.Password == user.Password)).FirstOrDefaultAsync();
        }

        public async Task<Models.User> AddUser(Models.User user) {

            if(user == null) return null;

            try
            {
                //Hash the password to store it in database
                user.Password = HashText.GetHashString(user.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;

            } catch (DbUpdateException dbue) {
                //Duplicate entry exception handler
                if(dbue.InnerException != null) {
                    string errorMsg = dbue.InnerException.Message.ToLower();
                    int idxDuplicateEntryError = errorMsg.IndexOf("duplicate entry");
                    int idxNullValue = errorMsg.IndexOf("cannot be null");

                    if(idxDuplicateEntryError >= 0) {
                        int startIdx = errorMsg.IndexOf('\'', idxDuplicateEntryError);
                        int endIdx = errorMsg.IndexOf('\'', startIdx + 1);
                        string duplicateEntryValue = errorMsg.Substring(startIdx, endIdx - startIdx);
                        throw new DBException($"There is an entry with the same value for {duplicateEntryValue}", DBExceptionType.DuplicateEntry);
                    } else if(idxNullValue >= 0) {
                        throw new DBException("Ensure you are not missing values to create a user", DBExceptionType.NullValue);
                    }
                }
                throw new DBException($"Unknown Error", DBExceptionType.UnknownException);
            }
        }

        public async Task<Models.User> GetUserByUsername(string username) {
            try {
                Models.User user = await _context.Users.FromSqlRaw("SELECT userId, username, email, password FROM users WHERE MATCH(username, email) AGAINST ({0} IN NATURAL LANGUAGE MODE)", username)
                .FirstAsync();

                if(user == null) return user;

                return user;
            } catch(Exception err) {
                _logger.LogError(err, "Error getting user by username: {Username}", username);
                return null;
            }

        }
    }
}