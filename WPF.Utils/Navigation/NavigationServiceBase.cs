#region usings

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion usings

namespace WpfUtils.Navigation
{
    /// <summary>
    /// NavigationService
    /// </summary>
    public abstract class NavigationServiceBase<TPage> : INavigationService where TPage : class, new()
    {
        #region properties

        public PageInfo CurrentPage { get; private set; }
        public IList<PageInfo> Pages { get; private set; }

        #endregion properties

        #region fields

        private Dictionary<PageInfo, TPage> PageDictionary;
        private Stack<PageInfo> BackwardPageStack;
        private Stack<PageInfo> ForwardPageStack;

        #endregion fields

        #region services

        protected readonly ILogger Logger;

        #endregion services

        #region constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="uiService"></param>
        /// <param name="logger"></param>
        /// <param name="pageCreator"></param>
        public NavigationServiceBase(ILogger logger)
        {
            Logger = logger;
            Pages = new List<PageInfo>();
            Initlize();
        }

        #endregion constructors

        #region methods

        /// <summary>
        /// initlize service
        /// </summary>
        /// <returns>if service initlized correctly</returns>
        public bool Initlize()
        {
            Logger.LogInformation("Initilizing Navigation");
            PageDictionary = new Dictionary<PageInfo, TPage>();
            BackwardPageStack = new Stack<PageInfo>();
            ForwardPageStack = new Stack<PageInfo>();

            return true;
        }

        /// <summary>
        /// Go back to previous page
        /// </summary>
        /// <returns>current page</returns>
        public async Task<PageInfo> Back()
        {
            if (BackwardPageStack.Count > 0)
            {
                var page = BackwardPageStack.Pop();
                var isPageOpened = await SetCurrentPageAsync(page);
                //put page back
                if (!isPageOpened)
                {
                    BackwardPageStack.Push(page);
                }
                else //put page to forwadStack
                {
                    ForwardPageStack.Push(page);
                }
            }
            return CurrentPage;
        }

        /// <summary>
        /// Go to forward page if went back before
        /// </summary>
        /// <returns>current page</returns>
        public async Task<PageInfo> Forward()
        {
            if (ForwardPageStack.Count > 0)
            {
                var page = ForwardPageStack.Pop();
                var isPageOpened = await SetCurrentPageAsync(page);
                //put page back
                if (!isPageOpened)
                {
                    ForwardPageStack.Push(page);
                }
                else //put page to backward
                {
                    BackwardPageStack.Push(page);
                }
            }
            return CurrentPage;
        }

        /// <summary>
        /// Create page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private TPage CreatePageAsync(PageInfo page)
        {
            Logger.LogInformation($"Creating Page : {page.PageType.FullName}");

            if (!page.PageType.IsSubclassOf(typeof(TPage)))
            {
                Logger.LogError($"Page creation error. Incompatible Type {page.PageType.Name} should be derived from {typeof(TPage).Name}");
                return null;
            }
            var pageInstance = Activator.CreateInstance(page.PageType);

            if (pageInstance == null)
            {
                Logger.LogError($"Page creation error. Can't create page instance: {typeof(TPage).Name}");
                return null;
            }
            return pageInstance as TPage;
        }

        /// <summary>
        /// return if has page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool HasPage(PageInfo page)
        {
            return Pages.Contains(page);
        }

        /// <summary>
        /// return if has page async
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Task<bool> HasPageAsync(PageInfo page)
        {
            return Task.FromResult(HasPage(page));
        }

        /// <summary>
        /// Add page to page list
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="pageName"></param>
        /// <param name="isDefault"></param>
        /// <returns>If added</returns>
        public bool AddPage(Type pageType, string pageName = null, bool isDefault = false)
        {
            if (!pageType.IsSubclassOf(typeof(TPage)))
            {
                Logger.LogError($"Page creation error. Incompatible Type {pageType.FullName} should be derived from {typeof(TPage).FullName}");
                return false;
            }
            else if (Pages.Any(x => x.PageType == pageType))
            {
                Logger.LogError($"Page list contain item with type:{typeof(TPage).FullName}");
                return false;
            }
            else
            {
                Pages.Add(new PageInfo
                {
                    PageType = pageType,
                    PageName = pageName ?? pageType.FullName,
                    Index = Pages.Count() - 1,
                    IsDefaultPage = isDefault,
                });
                return true;
            }
        }

        /// <summary>
        /// Open page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<PageInfo> OpenPageAsync(PageInfo page)
        {
            var isPageOpened = await SetCurrentPageAsync(page);
            if (isPageOpened)
            {
                BackwardPageStack.Push(page);
                ForwardPageStack.Clear();
            }

            return CurrentPage;
        }

        public async Task<bool> SetCurrentPageAsync(Type typeOfPage)
        {
            var selectedPage = Pages.FirstOrDefault(x => x.PageType == typeOfPage);
            if (selectedPage == null)
            {
                Logger.LogError($"Attemp to open undefinded page: {typeOfPage.FullName}");
                throw new UndefinedPageExeption($"Attemp to open undefinded page: {typeOfPage.FullName}");
            }
            else
            {
                return await SetCurrentPageAsync(selectedPage);
            }
        }

        /// <summary>
        /// Set current page
        /// </summary>
        /// <param name="page"></param>
        /// <returns>is page set</returns>
        public async Task<bool> SetCurrentPageAsync(PageInfo page)
        {
            if (!PageDictionary.ContainsKey(page))
            {
                PageDictionary[page] = CreatePageAsync(page);
            }
            if (PageDictionary[page] == null)
            {
                Logger.LogError($"{page.PageName} can't created");
                return false;
            }
            else
            {
                await OpenPageAsync(PageDictionary[page]);
                CurrentPage = page;
            }
            return true;
        }

        public abstract Task<bool> OpenPageAsync(TPage page);

        #endregion methods
    }
}