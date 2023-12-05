using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.UnitTest.TestData;
using FluentValidation.TestHelper;
using MinimalUserAPI.Application.Validations;

namespace MinimalUserAPI.UnitTest;
public class UserValidationUnitTest
{
    private readonly User validUser;
    private readonly UserValidator userValidator;

    public UserValidationUnitTest()
    {
        validUser = UserTestData.GetUser();
        userValidator = new();
    }

    [Fact]
    public void UserValidator_ValidUserObject_Test()
    {
        //Arrange / Act
        var validationResult = userValidator.TestValidate(validUser);

        validationResult.ShouldNotHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Company);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-16)]
    public void UserValidator_InvalidId_Test(int invalidId)
    {
        //Arrange
        User testUser = validUser with { Id = invalidId };

        // Act
        var validationResult = userValidator.TestValidate(testUser);

        //
        validationResult.ShouldHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Company);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("test34")]
    [InlineData("test_432_test")]
    public void UserValidator_InvalidName_Test(string invalidName)
    {
        //Arrange
        User testUser = validUser with { Name = invalidName };

        // Act
        var validationResult = userValidator.TestValidate(testUser);

        //
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Company);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("test34")]
    [InlineData("test432_test")]
    [InlineData("3452")]
    public void UserValidator_InvalidUserName_Test(string invalidName)
    {
        //Arrange
        User testUser = validUser with { UserName = invalidName };

        // Act
        var validationResult = userValidator.TestValidate(testUser);

        //
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Company);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("test.k")]
    [InlineData("34234.com")]
    public void UserValidator_InvalidWebsites_Test(string invalidWebsite)
    {
        //Arrange
        User testUser = validUser with { Website = invalidWebsite };

        // Act
        var validationResult = userValidator.TestValidate(testUser);

        //
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Company);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("@.com")]
    [InlineData("123456;")]
    [InlineData("test")]
    public void UserValidator_InvalidEmail_Test(string invalidEmail)
    {
        //Arrange
        User testUser = validUser with { Email = invalidEmail };

        // Act
        var validationResult = userValidator.TestValidate(testUser);

        //
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Company);
    }

    [Fact]
    public void UserValidator_InvalidAddress_Test()
    {
        //Arrange
        User testUser = validUser with { Address = null };

        // Act
        var validationResult = userValidator.TestValidate(testUser);

        //
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Company);
    }

    [Fact]
    public void UserValidator_InvalidCompany_Test()
    {
        //Arrange
        User testUser = validUser with { Company = null };

        // Act
        var validationResult = userValidator.TestValidate(testUser);

        //
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Id);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Name);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.UserName);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Website);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Email);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Phone);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Address);
        validationResult.ShouldHaveValidationErrorFor(s => s.Company);
    }

}
