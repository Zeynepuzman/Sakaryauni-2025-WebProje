using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Models
{
    public class Antrenor
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string AdSoyad { get; set; }

        [StringLength(150)]
        public string Uzmanlik { get; set; }

        [StringLength(500)]
        public string Biyografi { get; set; }

        // Salon ile ilişki
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        // Çoktan çoğa ilişki
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }

        // Bir antrenörün randevuları olabilir
        public ICollection<Randevu> Randevular { get; set; }
    }
}
