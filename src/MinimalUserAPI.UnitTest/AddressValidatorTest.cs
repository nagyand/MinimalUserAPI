using FluentValidation.TestHelper;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Validations;
using MinimalUserAPI.UnitTest.TestData;

namespace MinimalUserAPI.UnitTest;
public class AddressValidatorTest
{
    private readonly Address validAddress;
    private readonly AddressValidator validator;

    public AddressValidatorTest()
    {
        validAddress = UserTestData.GetUser().Address;
        validator = new();
    }

    [Fact]
    public void AddressValidator_ValidAddress_Test()
    {
        //Arrange / Act
        var validationResult = validator.TestValidate(validAddress);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Street);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.City);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Suite);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.ZipCode);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Geo);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("Test11")]
    [InlineData("Test;")]
    [InlineData("1111")]
    public void AddressValidator_InvalidAddressStreet_Test(string invalidStreet)
    {
        //Arrange
        var invalidAddress = validAddress with { Street = invalidStreet };

        // Act
        var validationResult = validator.TestValidate(invalidAddress);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(s => s.Street);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.City);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Suite);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.ZipCode);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Geo);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("Test11")]
    [InlineData("Test;")]
    [InlineData("1111")]
    public void AddressValidator_InvalidAddressCity_Test(string invalidCity)
    {
        //Arrange
        var invalidAddress = validAddress with { City = invalidCity };

        // Act
        var validationResult = validator.TestValidate(invalidAddress);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Street);
        validationResult.ShouldHaveValidationErrorFor(s => s.City);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Suite);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.ZipCode);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Geo);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("Suite Suite")]
    [InlineData("Apt. Suite")]
    [InlineData("1234")]
    [InlineData("Test")]
    public void AddressValidator_InvalidAddressSuite_Test(string invalidSuite)
    {
        //Arrange
        var invalidAddress = validAddress with { Suite = invalidSuite };

        // Act
        var validationResult = validator.TestValidate(invalidAddress);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Street);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.City);
        validationResult.ShouldHaveValidationErrorFor(s => s.Suite);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.ZipCode);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Geo);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("1-1")]
    [InlineData("343256")]
    [InlineData("32-75")]
    public void AddressValidator_InvalidAddressZipCode_Test(string invalidZipCode)
    {
        //Arrange
        var invalidAddress = validAddress with { ZipCode = invalidZipCode };

        // Act
        var validationResult = validator.TestValidate(invalidAddress);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Street);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.City);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Suite);
        validationResult.ShouldHaveValidationErrorFor(s => s.ZipCode);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Geo);
    }

    [Fact]
    public void AddressValidator_InvalidAddressGeo_Test()
    {
        //Arrange
        var invalidAddress = validAddress with { Geo = null! };

        // Act
        var validationResult = validator.TestValidate(invalidAddress);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Street);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.City);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Suite);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.ZipCode);
        validationResult.ShouldHaveValidationErrorFor(s => s.Geo);
    }
}
