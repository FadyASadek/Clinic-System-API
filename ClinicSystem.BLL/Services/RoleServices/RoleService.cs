using ClinicSystem.DAL.DTOs;
using ClinicSystem.DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicSystem.BLL.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<ApplicationUser> userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    public async Task<Result<AddRoleDto>> AddRole(AddRoleDto addrole)
    {
        var roleExists = await roleManager.RoleExistsAsync(addrole.RoleName);
        if (roleExists)
            return Result<AddRoleDto>.Failure("Role already exists.");

        var result = await roleManager.CreateAsync(new IdentityRole(addrole.RoleName));

        if (result.Succeeded)
            return Result<AddRoleDto>.Success(addrole);

       var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result<AddRoleDto>.Failure("Failed to create role: " + errors);
    }

    public async Task<Result<AssignRoleDto>> AssignRole(AssignRoleDto assignRoleDto)
    {
        var user = await userManager.FindByIdAsync(assignRoleDto.UserId);
        if (user == null)
           return Result<AssignRoleDto>.Failure("User not found.");

        var roleExists = await roleManager.RoleExistsAsync(assignRoleDto.RoleName);
        if (!roleExists)
            return Result<AssignRoleDto>.Failure("Role does not exist.");

        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);

        var result = await userManager.AddToRoleAsync(user, assignRoleDto.RoleName);
            if(result.Succeeded)
            return Result<AssignRoleDto>.Success(assignRoleDto );

        return Result<AssignRoleDto>.Failure("Failed to assign role.");
    }

    public async Task<Result<IEnumerable<AllRoleDto>>> GetAllRoles()
    {
        var roles = await roleManager.Roles
            .Select(r => new AllRoleDto { RoleName = r.Name! })
            .ToListAsync();

        return Result<IEnumerable<AllRoleDto>>.Success(roles);
    }
}

