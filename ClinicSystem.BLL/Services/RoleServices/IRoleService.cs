using ClinicSystem.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.BLL.Services;

public interface IRoleService
{
    Task<Result<IEnumerable<AllRoleDto>>> GetAllRoles();
    Task<Result<AddRoleDto>> AddRole(AddRoleDto role);
    Task<Result<AssignRoleDto>> AssignRole(AssignRoleDto assignRoleDto);
}
