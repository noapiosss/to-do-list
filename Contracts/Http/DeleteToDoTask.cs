namespace Contracts.Http
{
    public class DeleteToDoTaskRequest
    {
        public int TaskId { get; init; }
    }

    public class DeleteToDoTaskResponse
    {
        public bool Success { get; init; }
        public bool ToDoListExists { get; init; }
    }
}