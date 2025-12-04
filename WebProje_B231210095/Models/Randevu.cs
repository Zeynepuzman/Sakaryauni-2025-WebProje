using System;
using System.ComponentModel.DataAnnotations;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        [Required]
        public DateTime TarihSaat { get; set; }

        public int SureDakika { get; set; }

        public decimal Ucret { get; set; }

        [StringLength(50)]
        public string Durum { get; set; } // Onaylandı / Bekliyor / İptal

        // İlişkiler

        // Üye
        public string UyeId { get; set; }
        public Uye Uye { get; set; }

        // Antrenör
        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }

        // Hizmet
        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }
    }
}
