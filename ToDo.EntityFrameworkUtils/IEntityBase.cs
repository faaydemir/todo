namespace EntityFrameworkCoreHelper
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
    }
}