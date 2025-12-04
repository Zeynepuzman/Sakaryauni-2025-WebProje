using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebProje_B231210095.Models;

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
        public ICollection<Hizmet> Hizmetler { get; set; }
        public ICollection<Antrenor> Antrenorler { get; set; }
    }
}
