namespace Contracts.Http
{
    public class AddTaskRequest
    {
        public int ListId { get; init; }
        public string TaskName { get; init; }
        public string TaskDescription { get; init; }
    }

    public class AddTaskResponse
    {
        public bool Success { get; init; }
        public bool ToDoListExists { get; init; }
    }
}