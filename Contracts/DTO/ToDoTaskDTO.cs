using System;

namespace Contracts.DTO
{
    public class ToDoTaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? CompletionDateTime { get; set; }
    }
}