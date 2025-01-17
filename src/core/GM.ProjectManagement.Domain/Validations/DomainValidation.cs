using GM.ProjectManagement.Domain.Exceptions;

namespace GM.ProjectManagement.Domain.Validations;

public static class DomainValidation
{
    public static void NotNull(object? obj, string fieldName)
    {
        if (obj is null) 
            throw new EntityValidationException($"Field {fieldName} cannot be null");
    }

    public static void NotNullOrEmpty(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EntityValidationException($"Field {fieldName} cannot be null or empty");
    }
    
    public static void NotBeforeCurrentDate(DateOnly? date, string fieldName)
    {
        if (date < DateOnly.FromDateTime(DateTime.Now))
            throw new EntityValidationException($"{fieldName} must be the current date or later");
    }

    public static void MaxLength(string value, int maxLength, string fieldName)
    {
        if(value.Length > maxLength)
            throw new EntityValidationException($"{fieldName} should be less or equal {maxLength} characters long");
    }

    public static void MinLength(string value, int minLength, string fieldName)
    {
        if (value.Length < minLength)
            throw new EntityValidationException($"{fieldName} should be at least {minLength} characters long");
    }
}
