using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Models
{
    public class Hizmet
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Ad { get; set; }

        public int SureDakika { get; set; } // Hizmet süresi

        public decimal Ucret { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        // Salon ile ilişki
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        // Çoktan çoğa → Antrenör ile ilişki
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }
    }
}
