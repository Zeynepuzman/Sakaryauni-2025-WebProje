using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProje_B231210095.Models
{
    public class Randevu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime TarihSaat { get; set; }

        [Required]
        public int SureDakika { get; set; }

        [StringLength(30)]
        public string Durum { get; set; } // Bekliyor / Onaylandı / İptal


        // Üye
        public string UyeId { get; set; }
        public Uye Uye { get; set; }

        // Antrenör
        [Required]
        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }

        // Hizmet
        public int HizmetId { get; set; }

        [ValidateNever]
        public Hizmet Hizmet { get; set; }
        public decimal? Ucret { get; set; }

        public string? ReddetmeSebebi { get; set; }
    }
}
