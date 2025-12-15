using System;
using System.ComponentModel.DataAnnotations;

namespace WebProje_B231210095.Models.ViewModels
{
    public class RandevuCreateVM
    {
        [Required]
        public int AntrenorId { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public TimeSpan Saat { get; set; }
    }
}
