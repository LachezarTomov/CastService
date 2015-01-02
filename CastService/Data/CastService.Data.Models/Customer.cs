using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CastService.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(16)]
        public string Eik { get; set; }
        
        [Column(TypeName = "ntext")]
        [MaxLength]
        public string Address { get; set; }

        [MaxLength(30)]
        public string Place { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string Note { get; set; }
    }
}