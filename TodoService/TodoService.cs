using Akka.Actor;
using System;
using TodoDataAccess.DataAccess;
using TodoDataAccess.DataModel;

namespace TodoService
{
    public interface ITodoServiceBusinessLogic : IDisposable
    {
        void AddTodo(string taskName);
    }

    public class TodoServiceBusinessLogic : ITodoServiceBusinessLogic
    {
        private readonly TodoDbContext _dbContext;
        
        public TodoServiceBusinessLogic()
        {
            _dbContext = new TodoDbContext();
        }

        public TodoServiceBusinessLogic(TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TodoServiceBusinessLogic(string connectionStringName)
        {
            _dbContext = new TodoDbContext(connectionStringName);
        }

        public void AddTodo(string taskName)
        {
            // TODO how do we handle duplicates. In theory same old way we always have. 
            _dbContext.Todos.Add(new Todo { TaskName = taskName });
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}
