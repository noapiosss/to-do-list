using System;

namespace Contracts.Http
{
    public class UpdateTaskStatusRequest
    {
        public int TaskId { get; init; }
    }

    public class UpdateTaskStatusResponse
    {
        public DateTime CompletionDateTime { get; init; }
        public bool Success { get; init; }
        public bool ToDoListExists { get; init; }
    }
}