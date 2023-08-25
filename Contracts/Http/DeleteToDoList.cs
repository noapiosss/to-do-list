namespace Contracts.Http
{
    public class DeleteToDoListRequest
    {
        public int ListId { get; init; }
    }

    public class DeleteToDoListResponse
    {
        public bool Success { get; init; }
        public bool ToDoListExists { get; init; }
    }
}