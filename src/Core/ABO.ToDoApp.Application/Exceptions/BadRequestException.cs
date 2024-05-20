namespace ABO.ToDoApp.Application.Exceptions;

public class BadRequestException(string message) : Exception(message) { }