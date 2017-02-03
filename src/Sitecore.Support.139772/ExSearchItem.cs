
namespace Sitecore.Support.Social.Search
{
    using System;
    using ContentSearch;
    using Sitecore.Social.Search;

    [Serializable]
    public class ExSearchItem : SearchItem
    {
        [IndexField("is_in_workflow")]
        public bool IsInWorkflow { get; set; }
    }
}