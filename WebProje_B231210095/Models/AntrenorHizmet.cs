using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Models
{
public class AntrenorHizmet
{
    [Required]
    public int AntrenorId { get; set; }

    [ValidateNever]
    public Antrenor Antrenor { get; set; }

    [Required]
    public int HizmetId { get; set; }

    [ValidateNever]
    public Hizmet Hizmet { get; set; }
}
}

