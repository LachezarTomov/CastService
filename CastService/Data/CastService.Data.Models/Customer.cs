namespace CastService.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using CastService.Data.Common.Models;
    
    public class Customer : AuditInfo, IDeletableEntity
    {
        private ICollection<Instalation> instalations;
        public Customer()
        {
            this.instalations = new HashSet<Instalation>();
        }

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

        public virtual ICollection<Instalation> Instalations
        {
            get { return this.instalations; }
            set { this.instalations = value; }
        }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}