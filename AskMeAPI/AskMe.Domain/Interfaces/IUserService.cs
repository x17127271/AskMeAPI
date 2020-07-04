using AskMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        Task<User> GetUserById(int userId);
        Task<List<User>> GetUsers();
    }
}
