using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Product
    {

        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name ="Price 1-50")]
        [Range(1,1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name ="Price 50-100")]
        [Range(1,1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name ="Price 100+")]
        [Range(1,1000)]
        public double Price100 { get; set; }

        [Required] 
        public int CategoryId {  get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
