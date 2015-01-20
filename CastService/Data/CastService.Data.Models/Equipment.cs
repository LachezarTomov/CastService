namespace CastService.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Equipment
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string BatchNumber { get; set; }

        [MaxLength(16)]
        public string Model { get; set; }
    }
}
