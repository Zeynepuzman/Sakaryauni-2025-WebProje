using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebProje_B231210095.Models
{
    // Salon tarafından verilen hizmetler (fitness, yoga, pilates...)
    public class Hizmet
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Ad { get; set; }

        public int SureDakika { get; set; }

        // Ücret decimal olduğuna precision ekledik
        [Precision(18, 2)]
        public decimal Ucret { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        // Her hizmet bir salona bağlıdır
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        // Many-to-many (Hizmet - Antrenör)
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }

        // Hizmete bağlı randevular
        public ICollection<Randevu> Randevular { get; set; }
    }
}
