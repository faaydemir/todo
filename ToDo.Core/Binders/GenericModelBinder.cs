using System.Threading.Tasks;

namespace ToDo.Core.Binders
{
    public interface IModelInitilizer
    {
        object Model { get; }

        Task<bool> InitlizeAsync();
    }

    /// <summary>
    /// Create ViewModel and connect view model to services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericModelBinder<T> : IModelInitilizer
    {
        public T TypedModel { get; protected set; }
        public object Model => TypedModel;

        public abstract Task<bool> InitlizeAsync();
    }
}