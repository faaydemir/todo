using System;

namespace WpfUtils.Navigation
{
    public class PageInfo
    {
        public string PageName { get; set; }
        public Type PageType { get; set; }
        public int Index { get; set; }
        public bool IsDefaultPage { get; internal set; }
    }
}