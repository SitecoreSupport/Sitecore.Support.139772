
namespace Sitecore.Support.Social.Client.MessagePosting.UI.CustomFields
{
    using ContentSearch;
    using ContentSearch.ComputedFields;
    using Data;
    using Data.Items;
    using Workflows;

    public class IsInWorkflow : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;

            Database database = item?.Database;

            if (database == null)
            {
                return null;
            }

            IWorkflow workflow = database.WorkflowProvider?.GetWorkflow(item);

            if (workflow == null)
            {
                return null;
            }

            return !workflow.IsApproved(item);
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}
