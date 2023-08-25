using System;
using System.Collections.Generic;

namespace Contracts.DTO
{
    public class ToDoListTasksPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public ICollection<ToDoTaskDTO> ToDoTaskDTOs { get; set; }
    }
}