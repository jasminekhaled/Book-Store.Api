﻿using Microsoft.EntityFrameworkCore;

namespace Shopping.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }

        public DbSet<User>Users { get; set; }
    }
}
