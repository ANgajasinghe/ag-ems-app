using PasswordGenerator;

namespace ag.ems.application.Common.Helpers;

public static class PasswordHelper
{
    public static string GeneratePassword()
        => new Password(8).IncludeSpecial("@#").IncludeUppercase().IncludeNumeric().IncludeLowercase().Next();
}