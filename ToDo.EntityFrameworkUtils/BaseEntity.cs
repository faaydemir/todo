using System;

namespace EntityFrameworkCoreHelper
{
    public abstract class BaseEntity : IEntity
    {
        public virtual int Id { get; set; }
        public virtual bool IsDeleted { get; set; } = false;
        public virtual Guid? UpdatedBy { get; set; }
        public virtual Guid? DeletedBy { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual DateTime DeletedAt { get; set; }
        public virtual DateTime CreatedAt { get; set; }
    }
}