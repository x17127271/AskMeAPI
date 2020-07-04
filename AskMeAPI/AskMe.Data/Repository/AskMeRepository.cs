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
            var entity = await _context.Users.FindAsync(userId).ConfigureAwait(false);
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
    }
}
