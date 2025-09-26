using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Responses
{
    /// <summary>
    /// Respuesta estandarizada para APIs.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }           // true si todo salió bien
        public string Message { get; set; } = "";   // mensaje de éxito o error
        public T? Data { get; set; }                // contenido de la respuesta
        public int? TotalRows { get; set; }         // útil para listados/paginación
        public IEnumerable<string>? Errors { get; set; } // detalles de errores (validación, etc.)

        public static ApiResponse<T> Ok(T data, string message = "")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };

        }

        public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null)
        {
            return new ApiResponse<T> { Success = false, Message = message, Errors = errors };

        }
    }
}
