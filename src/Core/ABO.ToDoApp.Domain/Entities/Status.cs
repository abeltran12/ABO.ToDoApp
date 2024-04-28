namespace ABO.ToDoApp.Domain.Entities;

public enum Status
{
    Deleted = 0,
    Active = 1, 
    Completed = 2,
}

public class StatusHelper
{
    public static string GetStatusString(Status status)
    {
        return status switch
        {
            Status.Deleted => "Deleted",
            Status.Active => "Active",
            Status.Completed => "Completed",
            _ => throw new ArgumentException($"Unknown status: {status}"),
        };
    }

    public static int GetStatusInt(string status)
    {
        return status switch
        {
            "Active" => 1,
            "Completed" => 2,
            _ => throw new ArgumentException($"Unknown status: {status}"),
        };
    }
}