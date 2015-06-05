﻿using System.Data.Entity;
using TodoDataAccess.DataModel;

namespace TodoDataAccess.DataAccess
{
    public class TodoDbContext : DbContext
    {
        static TodoDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TodoDbContext>());
        }

        public TodoDbContext()
            : base("Name=TodoReference")     
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ValidateOnSaveEnabled = true;
            Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TodoMap());
        }
    }
}
