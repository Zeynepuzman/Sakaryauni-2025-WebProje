using System;
using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models
{
    public class UyePaket
    {
        public int Id { get; set; }

        [Required]
        public string UyeId { get; set; }
        public Uye Uye { get; set; }

        [Required]
        public int PaketId { get; set; }
        public Paket Paket { get; set; }

        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }

        public bool AktifMi { get; set; }
    }
}
