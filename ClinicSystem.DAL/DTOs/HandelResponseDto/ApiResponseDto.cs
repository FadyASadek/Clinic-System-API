using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem;

public class ApiResponseDto<T>
{
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
