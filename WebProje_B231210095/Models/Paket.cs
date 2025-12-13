using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models
{
    public class Paket
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Ad { get; set; }

        public int SureGun { get; set; }

        [Precision(18, 2)]
        public decimal Ucret { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        public int HizmetId { get; set; }

        [ValidateNever]
        public Hizmet Hizmet { get; set; }
        public int SalonId { get; set; }

    }
}
