using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CastService.Data.Common.Models;
using System.ComponentModel;

namespace CastService.Data.Models
{
    public class Customer : AuditInfo, IDeletableEntity
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

        public int? OldNameId { get; set; }
        
        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}