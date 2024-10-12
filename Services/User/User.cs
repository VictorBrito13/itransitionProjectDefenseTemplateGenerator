using ItransitionTemplates.Data;
using ItransitionTemplates.Models;
using ItransitionTemplates.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;

namespace ItransitionTemplates.Services.User
{
    public class User : IUserService {
        private readonly ApplicationDBContext _context;

        public User(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<Models.User> Login(Models.User user) {
            if(user == null || user.Email == null || user.Password == null) return null;
            user.Password = HashText.GetHashString(user.Password);
            return await _context.Users.Where(u => (u.Email == user.Email) && (u.Password == user.Password)).FirstOrDefaultAsync();
        }

        public async Task<string> AddUser(Models.User user) {

            if(user == null) return "There is no user to add";

            try
            {
                //Hash the password to store it in database
                user.Password = HashText.GetHashString(user.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "User added successfully";

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
    }
}