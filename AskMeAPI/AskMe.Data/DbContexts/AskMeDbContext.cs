using AskMe.Data.Entities;
using AskMe.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Data.DbContexts
{
    public class AskMeDbContext : DbContext
    {
        public AskMeDbContext(DbContextOptions<AskMeDbContext> options)
            : base(options)
        {

        }

        //datasabe sets
        public DbSet<UserEntity> Users { get; set; }

        
    }
}
