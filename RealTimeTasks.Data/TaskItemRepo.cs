using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeTasks.Data
{
    public class TaskItemRepo
    {
        private readonly string _connectionString;

        public TaskItemRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddTaskItem(TaskItem task)
        {
            using var ctx = new TaskDataContext(_connectionString);
            ctx.TaskItems.Add(task);
            ctx.SaveChanges();
        }

        public void DeleteTask(int id)
        {
            using var ctx = new TaskDataContext(_connectionString);
            ctx.TaskItems.Remove(ctx.TaskItems.FirstOrDefault(t => t.Id == id));
            ctx.SaveChanges();
        }

        public List<TaskItem> GetTaskItems()
        {
            using var ctx = new TaskDataContext(_connectionString);
            return ctx.TaskItems.Include(t => t.User).ToList();
        }

        public void MarkAsDoing(TaskItem taskItem, int userId)
        {
            using var ctx = new TaskDataContext(_connectionString);
            if(ctx.TaskItems.Any(t => t.Id == taskItem.Id && t.UserId == 0))
            {
                return;
            }

            ctx.TaskItems.Where(t => t.Id == taskItem.Id)
                .ExecuteUpdate(b => b.SetProperty(u => u.UserId, userId));
        }
    }
}
