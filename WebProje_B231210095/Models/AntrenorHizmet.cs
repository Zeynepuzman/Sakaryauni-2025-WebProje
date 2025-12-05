using WebProje_B231210095.Models;

namespace WebProje_B231210095.Models
{
    // Çoktan çoğa ilişki tablosu (Antrenör - Hizmet)
    public class AntrenorHizmet
    {
        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }
    }
}
