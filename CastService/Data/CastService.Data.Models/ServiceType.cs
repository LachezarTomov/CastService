namespace CastService.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ServiceType
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
