using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Specifics.Cards
{
    /// <summary>
    /// Contiene los datos visibles en el carnet (nombre, cargo, QR, etc.)
    /// </summary>
    public class CardUserData
    {
        public string Name { get; set; }
        public string Profile { get; set; }
        public string CategoryArea { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CardId { get; set; }
        public string BloodTypeValue { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public Guid UniqueId { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }

        public string UserPhotoUrl { get; set; }
        public string LogoUrl { get; set; }
        public string QrUrl { get; set; }
    }
}
