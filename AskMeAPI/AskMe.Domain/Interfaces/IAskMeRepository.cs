using AskMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IAskMeRepository
    {
        bool Save();
        ValueTask<User> AddUser(User user);
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUserName(string userName);
        Task<List<User>> GetUsers();
        Task<Subject> AddSubject(Subject subject, int userId);
        Task<Subject> GetSubjectById(int subjectId);
        Task<List<Subject>> GetSubjects(int userId);
    }
}
