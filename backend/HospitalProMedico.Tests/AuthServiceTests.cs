using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Services;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Infrastructure.Repositories;
using HospitalProMedico.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace HospitalProMedico.Tests;

public class AuthServiceTests
{
    private static AuthService CreateService(HospitalProMedico.Infrastructure.Data.AppDbContext context)
    {
        var users = new Repository<User>(context);
        var passwordHasher = new PasswordHasher();
        var jwtSettings = Options.Create(new JwtSettings
        {
            Secret = "Test-Secret-Key-For-Unit-Tests-Only-Not-For-Production-Use",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationMinutes = 60
        });
        var tokenService = new JwtTokenService(jwtSettings);
        return new AuthService(users, passwordHasher, tokenService);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_ForValidCredentials()
    {
        using var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        context.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@hospital.com",
            PasswordHash = hasher.Hash("Admin@123"),
            Role = UserRole.ADMIN,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.LoginAsync(new LoginRequestDto { Email = "admin@hospital.com", Password = "Admin@123" });

        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.Equal("admin@hospital.com", result.User.Email);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_ForWrongPassword()
    {
        using var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        context.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@hospital.com",
            PasswordHash = hasher.Hash("Admin@123"),
            Role = UserRole.ADMIN,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await Assert.ThrowsAsync<UnauthorizedAppException>(() =>
            service.LoginAsync(new LoginRequestDto { Email = "admin@hospital.com", Password = "wrong" }));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorized_ForInactiveUser()
    {
        using var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        context.Users.Add(new User
        {
            Name = "Inactive",
            Email = "inactive@hospital.com",
            PasswordHash = hasher.Hash("Admin@123"),
            Role = UserRole.RECEPCAO,
            IsActive = false
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await Assert.ThrowsAsync<UnauthorizedAppException>(() =>
            service.LoginAsync(new LoginRequestDto { Email = "inactive@hospital.com", Password = "Admin@123" }));
    }
}
