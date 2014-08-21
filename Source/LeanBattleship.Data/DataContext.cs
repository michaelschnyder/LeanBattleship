﻿using System.Data.Entity;
using LeanBattleship.Model;

namespace LeanBattleship.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DataContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
