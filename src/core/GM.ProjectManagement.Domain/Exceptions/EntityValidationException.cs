namespace GM.ProjectManagement.Domain.Exceptions;

public class EntityValidationException(string? message) : Exception(message)
{}
