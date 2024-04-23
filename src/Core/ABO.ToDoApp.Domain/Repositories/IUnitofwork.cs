namespace ABO.ToDoApp.Domain.Repositories;

public interface IUnitofwork
{
    ITodoListRepository TodoListRepository { get; }
    Task SaveAsync();
}
