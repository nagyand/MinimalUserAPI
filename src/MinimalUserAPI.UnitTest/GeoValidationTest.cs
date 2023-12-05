using FluentValidation.TestHelper;
using MinimalUserAPI.Application.Entity;
using MinimalUserAPI.Application.Validations;

namespace MinimalUserAPI.UnitTest;
public class GeoValidationTest
{
    private readonly Geo validGeo;
    private readonly GeoValidator geoValidator;

    public GeoValidationTest()
    {
        validGeo = new Geo() 
        {
            Lat = 56.0,
            Lng = -123.657
        };
        geoValidator = new ();
    }

    [Fact]
    public void GeoValidator_ValidGeo_Test()
    {
        //Arrange / Act
        var validationResult = geoValidator.TestValidate(validGeo);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Lng);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Lat);
    }

    [Theory]
    [InlineData(-123.543)]
    [InlineData(321.4564)]
    public void GeoValidator_InvalidLat_Test(double invalidLat)
    {
        //Arrange
        var invalidGeo = validGeo with { Lat = invalidLat };

        // Act
        var validationResult = geoValidator.TestValidate(invalidGeo);

        //Assert
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Lng);
        validationResult.ShouldHaveValidationErrorFor(s => s.Lat);
    }

    [Theory]
    [InlineData(-191.543)]
    [InlineData(193.4564)]
    public void GeoValidator_InvalidLng_Test(double invalidLng)
    {
        //Arrange
        var invalidGeo = validGeo with { Lng = invalidLng };

        // Act
        var validationResult = geoValidator.TestValidate(invalidGeo);

        //Assert
        validationResult.ShouldHaveValidationErrorFor(s => s.Lng);
        validationResult.ShouldNotHaveValidationErrorFor(s => s.Lat);
    }
}
