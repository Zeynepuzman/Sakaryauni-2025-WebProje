using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models
{
    public class AiInputViewModel
    {
        [Required]
        [Range(10, 90, ErrorMessage = "Yaş 10 ile 90 arasında olmalıdır.")]
        public int Yas { get; set; }

        [Required]
        [Range(120, 230)]
        public int Boy { get; set; }

        [Required]
        [Range(30, 200)]
        public int Kilo { get; set; }

        [Required]
        public string Cinsiyet { get; set; }

        [Required]
        public string VucutTipi { get; set; }

        [Required]
        public string Hedef { get; set; }

        [Required]
        [Range(1, 7)]
        public int HaftadaKacGun { get; set; }
    }
}
