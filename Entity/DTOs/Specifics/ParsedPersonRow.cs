using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs.ModelSecurity.Request;

namespace Entity.DTOs.Specifics
{
    public class ParsedPersonRow
    {
        public int RowNumber { get; set; }
        public PersonDtoRequest Person { get; set; } = default!;
        public byte[]? PhotoBytes { get; set; }           // Foto embebida en el Excel
        public string? PhotoExtension { get; set; }       // ".jpg" / ".png" si se detecta
        public string? TempPassword { get; set; }         // Password temporal generado en parser
    }
}
