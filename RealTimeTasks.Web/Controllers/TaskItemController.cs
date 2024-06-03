using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeTasks.Data;
using RealTimeTasks.Web.Models;

namespace RealTimeTasks.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IHubContext<TaskHub> _hub;

        public TaskItemController(IConfiguration configuration, IHubContext<TaskHub> hub)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _hub = hub;
        }

        [HttpGet("getall")]
        public List<TaskItem> GetTaskItems()
        {
            var repo = new TaskItemRepo(_connectionString);
            return repo.GetTaskItems();
        }

        [HttpPost("add")]
        public void AddTask(TaskItem taskItem)
        {
            var repo = new TaskItemRepo(_connectionString);
            repo.AddTaskItem(taskItem);
            _hub.Clients.All.SendAsync("addTask", taskItem);
        }


        [HttpPost("delete")]
        public void DeleteTask(TaskItem taskItem)
        {
            var repo = new TaskItemRepo(_connectionString);
            repo.DeleteTask(taskItem.Id);
        }

        [HttpPost("markasdoing")]
        public void MarkAsDoing(TaskItem taskItem)
        {
            var repo = new TaskItemRepo(_connectionString);
            var userId = GetCurrentUser().Id;
            repo.MarkAsDoing(taskItem, userId);
            var taskItems = GetTaskItems();
            _hub.Clients.All.SendAsync("markAsDoing", taskItems);
        }


        private User GetCurrentUser()
        {
            var userRepo = new UserRepo(_connectionString);
            return userRepo.GetByEmail(User.Identity.Name);
        }

    }

}
