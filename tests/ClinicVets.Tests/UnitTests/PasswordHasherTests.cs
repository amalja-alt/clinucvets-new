using ClinicVets.Services;

namespace ClinicVets.Tests.UnitTests;

public class PasswordHasherTests
{
    [Fact]
    public void HashPassword_DoesNotStorePlainTextPassword()
    {
        string hash = PasswordHasher.HashPassword("Secret#1");

        Assert.NotEqual("Secret#1", hash);
        Assert.StartsWith("PBKDF2-SHA256$", hash);
    }

    [Fact]
    public void VerifyPassword_ReturnsTrue_ForMatchingPassword()
    {
        string hash = PasswordHasher.HashPassword("Secret#1");

        Assert.True(PasswordHasher.VerifyPassword("Secret#1", hash));
    }

    [Fact]
    public void VerifyPassword_ReturnsFalse_ForWrongPassword()
    {
        string hash = PasswordHasher.HashPassword("Secret#1");

        Assert.False(PasswordHasher.VerifyPassword("Wrong#12", hash));
    }
}
