namespace eCommerceApp.Domain.Entities
{
    public abstract class BaseEntity 
    {
        public Guid Id { get; set; }
        //Denetim alanları(Audit fields)
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        //soft delete için.
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
