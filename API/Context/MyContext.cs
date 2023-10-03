﻿using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.context
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options): base(options) 
        { 

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
    }
}
