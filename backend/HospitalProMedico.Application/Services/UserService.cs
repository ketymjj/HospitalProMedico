using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Interfaces;

namespace HospitalProMedico.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _users;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IRepository<User> users, IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<PagedResult<UserDto>> GetAllAsync(PaginationParams pagination)
    {
        var query = _users.Query().OrderBy(u => u.Name).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(u => u.Name.ToLower().Contains(term) || u.Email.ToLower().Contains(term));
        }

        var result = await query.Select(u => u).ToPagedResultAsync(pagination);

        return new PagedResult<UserDto>
        {
            Items = result.Items.Select(u => u.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _users.GetByIdAsync(id) ?? throw new NotFoundException("Usuário", id);
        return user.ToDto();
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        if (await _users.ExistsAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
        {
            throw new BusinessRuleException("Já existe um usuário cadastrado com este email.");
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            Role = dto.Role,
            DoctorId = dto.DoctorId,
            IsActive = true
        };

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return user.ToDto();
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _users.GetByIdAsync(id) ?? throw new NotFoundException("Usuário", id);

        if (await _users.ExistsAsync(u => u.Id != id && u.Email.ToLower() == dto.Email.ToLower()))
        {
            throw new BusinessRuleException("Já existe um usuário cadastrado com este email.");
        }

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Role = dto.Role;
        user.IsActive = dto.IsActive;
        user.DoctorId = dto.DoctorId;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.PasswordHash = _passwordHasher.Hash(dto.Password);
        }

        _users.Update(user);
        await _users.SaveChangesAsync();

        return user.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _users.GetByIdAsync(id) ?? throw new NotFoundException("Usuário", id);
        _users.Remove(user);
        await _users.SaveChangesAsync();
    }
}
