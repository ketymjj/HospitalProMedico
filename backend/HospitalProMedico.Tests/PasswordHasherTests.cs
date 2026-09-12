using HospitalProMedico.Infrastructure.Services;
using Xunit;

namespace HospitalProMedico.Tests;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void Hash_ShouldProduceDifferentValue_ThanPlainText()
    {
        var hash = _hasher.Hash("Admin@123");

        Assert.NotEqual("Admin@123", hash);
        Assert.True(hash.Length > 20);
    }

    [Fact]
    public void Verify_ShouldReturnTrue_ForCorrectPassword()
    {
        var hash = _hasher.Hash("Admin@123");

        Assert.True(_hasher.Verify("Admin@123", hash));
    }

    [Fact]
    public void Verify_ShouldReturnFalse_ForIncorrectPassword()
    {
        var hash = _hasher.Hash("Admin@123");

        Assert.False(_hasher.Verify("WrongPassword", hash));
    }
}
