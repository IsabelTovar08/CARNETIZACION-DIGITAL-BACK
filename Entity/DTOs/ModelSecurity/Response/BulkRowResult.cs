using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ModelSecurity.Response
{
    //Resultado por fila de carga masiva
    public class BulkRowResult
    {
        public int RowNumber { get; set; }           // Fila en el Excel (1‑based para el usuario)
        public bool Success { get; set; }
        public string? Message { get; set; }         // Error o “OK”
        public string? GeneratedPassword { get; set; }  // opcional: para auditoría (no recomendado devolverla)
        public bool Created { get; set; }      // true si se persistió persona/usuario
        public bool EmailSent { get; set; }    // true si el email salió ok
    
    }
}
