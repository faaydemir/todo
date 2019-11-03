using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfUtils.Navigation
{
    public interface INavigationService
    {
        PageInfo CurrentPage { get; }
        IList<PageInfo> Pages { get; }

        Task<PageInfo> Back();

        Task<PageInfo> Forward();

        bool HasPage(PageInfo page);

        Task<bool> HasPageAsync(PageInfo page);

        Task<PageInfo> OpenPageAsync(PageInfo page);
    }
}