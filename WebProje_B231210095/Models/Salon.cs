using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models
{
    public class Salon
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Ad { get; set; }

        [StringLength(250)]
        public string Adres { get; set; }

        [StringLength(150)]
        public string CalismaSaatleri { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        // İlişkiler
        // Bir salonun birden çok hizmeti olabilir
        public ICollection<Hizmet> Hizmetler { get; set; }

        // Bir salonda birden çok antrenör çalışabilir
        public ICollection<Antrenor> Antrenorler { get; set; }
    }
}
