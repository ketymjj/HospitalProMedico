using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _tokenService;

    public AuthService(IRepository<User> users, IPasswordHasher passwordHasher, IJwtTokenService tokenService)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _users.Query().FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAppException("Email ou senha inválidos.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAppException("Usuário inativo. Contate o administrador.");
        }

        var token = _tokenService.GenerateToken(user, out var expiresAt);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = user.ToDto()
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (await _users.ExistsAsync(u => u.Email.ToLower() == request.Email.ToLower()))
        {
            throw new BusinessRuleException("Já existe um usuário cadastrado com este email.");
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = UserRole.RECEPCAO,
            IsActive = true
        };

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user, out var expiresAt);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = user.ToDto()
        };
    }
}
