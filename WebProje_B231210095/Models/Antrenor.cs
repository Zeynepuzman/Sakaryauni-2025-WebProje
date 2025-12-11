using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebProje_B231210095.Models
{
    public class Antrenor
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string? AdSoyad { get; set; }

        // Bunlar zorunlu değilse nullable yapalım:
        [StringLength(150)]
        public string? Uzmanlik { get; set; }

        [StringLength(150)]
        public string? Telefon { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        // Zorunlu salon id
        public int SalonId { get; set; }

        // Formdan değer gelmesini beklemediğimiz navigation property:
        [ValidateNever]
        public Salon? Salon { get; set; }

        // Navigation koleksiyonları da validation'a girmesin:
        [ValidateNever]
        public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }

        [ValidateNever]
        public ICollection<Randevu>? Randevular { get; set; }
    }
}
