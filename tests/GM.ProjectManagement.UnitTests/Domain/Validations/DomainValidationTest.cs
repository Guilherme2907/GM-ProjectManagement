using Bogus;
using FluentAssertions;
using GM.ProjectManagement.Domain.Exceptions;
using GM.ProjectManagement.Domain.Validations;

namespace GM.ProjectManagement.UnitTests.Domain.Validations;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var value = Faker.Commerce.ProductName();
        Action action =
            () => DomainValidation.NotNull(value, fieldName);
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNull(value, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"Field {fieldName} cannot be null");
    }


    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"Field {fieldName} cannot be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long");
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MaxLength(target, maxLength, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MaxLength(target, maxLength, fieldName);

        action.Should().NotThrow();
    }


    [Fact(DisplayName = nameof(NotBeforeCurrentDateOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotBeforeCurrentDateOk()
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var value = DateOnly.FromDateTime(DateTime.Now.AddDays(new Random().Next(0, 10)));

        Action action =
            () => DomainValidation.NotBeforeCurrentDate(value, fieldName);

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotBeforeCurrentDateThrowWhenDateIsInvalid))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotBeforeCurrentDateThrowWhenDateIsInvalid()
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var value = DateOnly.FromDateTime(DateTime.Now.AddDays(- (new Random().Next(1, 10))));

        Action action =
            () => DomainValidation.NotBeforeCurrentDate(value, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} must be the current date or later");
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOftests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOftests - 1; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOftests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOftests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOftests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOftests - 1; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random()).Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOftests = 5)
    {
        var faker = new Faker();
        for (int i = 0; i < numberOftests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, maxLength };
        }
    }
}
