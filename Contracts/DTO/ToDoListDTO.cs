using System;

namespace Contracts.DTO
{
    public class ToDoListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public int TasksCount { get; set; }
        public int CompletedTasksCount { get; set; }
    }
}