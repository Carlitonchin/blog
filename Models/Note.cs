using System.ComponentModel.DataAnnotations;
using blog.Areas.Identity.Data;

namespace blog.Models;

public class Note{
    [Key]
    public int Id {get; set;}

    [Required(ErrorMessage ="Inserte un título")]
     [RegularExpression(@"\s*[^[\s]]*\s*", ErrorMessage ="Inserte un título")] //ony blank spaces not allowed
     [StringLength(100, ErrorMessage = "El título debe tener máximo 100 caracteres")]
     [Display(Name = "Título")]
    public string? Title {get; set;}

    [Required(ErrorMessage ="Escriba algo en el cuerpo")]
     [RegularExpression(@"\s*[^[\s]]*\s*", ErrorMessage ="Escriba algo en el cuerpo")] //ony blank spaces not allowed
     [StringLength(250000, ErrorMessage = "El cuerpo debe tener máximo 250 mil caracteres")]
     [Display(Name = "Título")]
    public string? Body {get; set;}

    [Required]
    public string? UserId {get; set;}
    public virtual User? User {get; set;}
}