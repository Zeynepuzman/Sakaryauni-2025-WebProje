using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models
{
    // Salon tarafından verilen hizmetler (fitness, yoga, pilates...)
    public class Hizmet
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Ad { get; set; }

        public int SureDakika { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        // Her hizmet bir salona bağlıdır
        //FK
        public int SalonId { get; set; }

        [ValidateNever]
        public Salon Salon { get; set; }

        // Many-to-many (Hizmet - Antrenör)
        [ValidateNever]
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }

        // Hizmete bağlı randevular
        [ValidateNever]
        public ICollection<Randevu> Randevular { get; set; }
    }
}
