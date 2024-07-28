using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        // auditing properties
        public string? CreateDate { get; set; }
        public string? UpdateDate { get; set; }

        public string? CreatedById { get; set; }

        [NotMapped]
        public AppUser? CreatedBy { get; set; }

        public string? UpdatedById { get; set; }
        
        [NotMapped]
        public AppUser? UpdatedBy { get; set; }
    }
}
