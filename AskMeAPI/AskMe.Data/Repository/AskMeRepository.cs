using AskMe.Data.DbContexts;
using AskMe.Data.Entities;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Data.Repository
{
    public class AskMeRepository : IAskMeRepository, IDisposable
    {
        private readonly AskMeDbContext _context;
        private readonly IMapper _mapper;

        public AskMeRepository(
            AskMeDbContext context,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool Save()
        {
            return (_context.SaveChanges() > 0);
        }

        public async Task<User> GetUserById(int userId)
        {            
            var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId).ConfigureAwait(false);
            return _mapper.Map<User>(entity);
        }

        public async Task<List<User>> GetUsers()
        {
            var entities = await _context.Users.ToListAsync().ConfigureAwait(false);
            return _mapper.Map<List<User>>(entities);
        }

        public async ValueTask<User> AddUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var entity = _mapper.Map<UserEntity>(user);

            var entityCreated = await _context.Users.AddAsync(entity).ConfigureAwait(false);
            Save();
            return _mapper.Map<User>(entityCreated.Entity);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName).ConfigureAwait(false);

            return _mapper.Map<User>(entity);
        }

        public bool UserExists(int userId) => _context.Users.Find(userId) != null;


        public async Task<Subject> AddSubject(Subject subject, int userId)
        {
            if(!UserExists(userId))
            {
                throw new ArgumentNullException(nameof(User));
            }
            
            subject.UserId = userId;

            var entity = _mapper.Map<SubjectEntity>(subject);

            var entityCreated = await _context.Subjects.AddAsync(entity).ConfigureAwait(false);
            Save();
            return _mapper.Map<Subject>(entityCreated.Entity);
        }

        public async Task<Subject> GetSubjectById(int subjectId)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(subject => subject.Id == subjectId).ConfigureAwait(false);
            if(subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return _mapper.Map<Subject>(subject);
        }

        public async Task<List<Subject>> GetSubjects(int userId)
        {
            if (!UserExists(userId))
            {
                throw new ArgumentNullException(nameof(User));
            }

            var subjects = await _context.Subjects.Where(subject => subject.User.Id == userId).ToListAsync().ConfigureAwait(false);
            
            return _mapper.Map<List<Subject>>(subjects);
        }
    }
}
