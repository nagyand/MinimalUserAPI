using FluentValidation.TestHelper;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Validations;
using MinimalUserAPI.UnitTest.TestData;

namespace MinimalUserAPI.UnitTest;
public class CompanyValidatorTest
{
    private readonly Company validCompany;
    private readonly CompanyValidator companyValidator;

    public CompanyValidatorTest()
    {
        validCompany = UserTestData.GetUser().Company;
        companyValidator = new();
    }

    [Fact]
    public void CompanyValidator_ValidCompany_Test()
    {
        //Arrange / Act
        var validationResult = companyValidator.TestValidate(validCompany);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.CatchPhrase);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Bs);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("Test and 1234 Bt.")]
    [InlineData("12345;")]
    public void CompanyValidator_InvalidCompanyName_Test(string invalidCompanyName)
    {
        //Arrange
        var invalidCompany = validCompany with { Name = invalidCompanyName };

        // Act
        var validationResult = companyValidator.TestValidate(invalidCompany);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.CatchPhrase);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Bs);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("Test and 1234 Bt.")]
    [InlineData("12345;")]
    public void CompanyValidator_InvalidCompanyCatchPhrase_Test(string invalidCompanyCatchPhrase)
    {
        //Arrange
        var invalidCompany = validCompany with { CatchPhrase = invalidCompanyCatchPhrase };

        // Act
        var validationResult = companyValidator.TestValidate(invalidCompany);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldHaveValidationErrorFor(s => s.CatchPhrase);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Bs);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("Test and 1234 Bt.")]
    [InlineData("12345;")]
    public void CompanyValidator_InvalidCompanyBs_Test(string invalidCompanyBs)
    {
        //Arrange
        var invalidCompany = validCompany with { Bs = invalidCompanyBs };

        // Act
        var validationResult = companyValidator.TestValidate(invalidCompany);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.CatchPhrase);
        validationResult.ShouldHaveValidationErrorFor(s => s.Bs);
    }
}
