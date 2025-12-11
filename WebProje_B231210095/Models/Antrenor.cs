using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models
{
    // Antrenör bilgileri
    public class Antrenor
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string AdSoyad { get; set; }

        [StringLength(150)]
        public string Uzmanlik { get; set; }

        [StringLength(500)]
        public string Biyografi { get; set; }

        [StringLength(150)]
        public string Telefon { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        // Antrenör bir salona bağlıdır
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        // Many-to-many ilişki (Antrenör - Hizmet)
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }

        // Antrenöre bağlı randevular
        public ICollection<Randevu> Randevular { get; set; }
    }
}
