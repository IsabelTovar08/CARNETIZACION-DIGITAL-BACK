using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics.Cards
{
    public class IssuedCardBasicDto
    {
        public int Id { get; set; }

        /// <summary>Nombre del perfil al que pertenece la tarjeta</summary>
        public string ProfileName { get; set; }

        /// <summary>Nombre de la división interna</summary>
        public string InternalDivisionName { get; set; }

        /// <summary>Estado activo</summary>
        public bool IsActive { get; set; }
    }
}
