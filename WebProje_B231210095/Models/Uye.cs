using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Models
{
    public class Uye : IdentityUser
    {
        [StringLength(150)]
        public string AdSoyad { get; set; }

        public int? Yas { get; set; }

        public float? Boy { get; set; }
        public float? Kilo { get; set; }

        public string ProfilResmi { get; set; }  // isteğe bağlı

        // Randevu ilişkisi
        public ICollection<Randevu> Randevular { get; set; }
    }
}
