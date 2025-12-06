using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models
{
    // Identity kullanıcı sınıfımız
    public class Uye : IdentityUser
    {
        [StringLength(150)]
        public string AdSoyad { get; set; }

        public int? Yas { get; set; }
        public float? Boy { get; set; }
        public float? Kilo { get; set; }

        // Kullanıcının randevuları
        public ICollection<Randevu> Randevular { get; set; }
    }
}
