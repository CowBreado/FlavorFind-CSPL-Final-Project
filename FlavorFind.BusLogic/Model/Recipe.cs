using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlavorFind.BusLogic.Model
{
    [Table("Recipes")]
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public required string Description { get; set; }

        public required string Ingredients { get; set; }

        public required string Instructions { get; set; }

        public required string Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}