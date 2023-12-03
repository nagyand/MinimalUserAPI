using Microsoft.Extensions.Options;
using MinimalUserAPI.Infrastructure.Configurations;

namespace MinimalUserAPI.Infrastructure.Validations;
public class UserDbContextConfigurationValidation : IValidateOptions<UserDbContextConfiguration>
{
    public ValidateOptionsResult Validate(string? name, UserDbContextConfiguration options)
    {
        string errorMessage = string.Empty;
        if (string.IsNullOrEmpty(options.ConnectionString))
        {
            errorMessage += "Connection string is empty.";
        }
        if (string.IsNullOrEmpty(options.DatabaseName))
        {
            errorMessage += "Database name is empty";
        }
        if (string.IsNullOrEmpty(options.CollectionName))
        {
            errorMessage += "Collection name is empty";
        }
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return ValidateOptionsResult.Fail(errorMessage);
        }
        return ValidateOptionsResult.Success;
    }
}
