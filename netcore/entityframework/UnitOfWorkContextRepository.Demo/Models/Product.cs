using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class Product : IValidatableObject
    {
        public int Id { get; set; }
        
        [Display(Name = "组号")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string ProductCode { get; set; }

        [Display(Name = "产品名称")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string ProductName { get; set; }

        [Display(Name = "Vmax（V）")]
        [DisplayFormat(DataFormatString = "18,3")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        public decimal? Vmax { get; set; }

        [Display(Name = "Vmin（V）")]
        [DisplayFormat(DataFormatString = "18,3")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        public decimal? Vmin { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Vmax.HasValue && Vmin.HasValue && Vmax < Vmin)
            {
                yield return new ValidationResult("Vmax必须大于Vmin", new[] { nameof(Vmax), nameof(Vmin) });
            }
        }
    }
}