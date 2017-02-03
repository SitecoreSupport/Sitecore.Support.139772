
namespace Sitecore.Support.Social.Search
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using ContentSearch.Security;
    using Sitecore.Data;
    using Sitecore.Social.Configuration.Model;
    using Sitecore.Social.Domain.Model;
    using Sitecore.Social.Search;
    using Sitecore.Social.Infrastructure.Extensions;
    using Sitecore.ContentSearch.Linq.Utilities;

    public class SearchProvider : Sitecore.Social.Search.SearchProvider, ISearchProvider
    {
        IEnumerable<Identifier> ISearchProvider.GetMessagesReadyToPostAutomatically(IEnumerable<Identifier> accountIds)
        {
            Expression<Func<ExSearchItem, bool>> messageFilterExpression = searchItem =>
                (searchItem.MessagePostedDate == DateTime.MinValue.Date) &&
                //(searchItem.FinalWorkflowState || (searchItem.WorkflowStateId == ID.Null));
                !searchItem.IsInWorkflow;

            var messagesByMessageFilter = this.SearchExItems(messageFilterExpression.And<ExSearchItem>(this.TemplateIsMessageExpression())).ToList();

            if (!messagesByMessageFilter.Any())
            {
                return Enumerable.Empty<Identifier>();
            }

            // search messages by Posting Configuration fields
            var contentPostingConfigurationTemplateId = this.ConfigurationFactory
              .Get<PostingConfigurationsConfiguration>()
              .PostingConfigurations
              .Where(postingConfiguration => string.Equals(postingConfiguration.Name, "ContentPosting", StringComparison.Ordinal))
              .Select(postingConfiguration => ID.Parse(postingConfiguration.TemplateId))
              .First();

            Expression<Func<SearchItem, bool>> postingConfigurationFilterExpression = searchItem =>
              (searchItem.TemplateId == contentPostingConfigurationTemplateId) &&
              searchItem.PostAutomatically && searchItem.ItemPublished;

            Expression<Func<SearchItem, bool>> accountFilterExpression = null;

            foreach (var accountId in accountIds)
            {
                var normalizedAccountId = accountId.GetID();

                if (accountFilterExpression == null)
                {
                    accountFilterExpression = searchItem => searchItem.AccountId == normalizedAccountId;
                }
                else
                {
                    accountFilterExpression = accountFilterExpression.Or(searchItem => searchItem.AccountId == normalizedAccountId);
                }
            }

            if (accountFilterExpression != null)
            {
                postingConfigurationFilterExpression = postingConfigurationFilterExpression.And(accountFilterExpression);
            }

            var messagesByPostingConfigurationFilter = this.SearchItems(postingConfigurationFilterExpression, searchItem => searchItem.Parent.GetIdentifier()).ToList();

            // return messages that exists in both search results
            return !messagesByPostingConfigurationFilter.Any()
              ? Enumerable.Empty<Identifier>() :
              messagesByMessageFilter.Intersect(messagesByPostingConfigurationFilter);
        }

        protected IEnumerable<Identifier> SearchExItems(Expression<Func<ExSearchItem, bool>> whereExpression)
        {
            if (this.SearchIndex == null)
            {
                return new List<Identifier>();
            }

            using (var searchContext = this.SearchIndex.CreateSearchContext(SearchSecurityOptions.DisableSecurityCheck))
            {
                return searchContext.GetQueryable<ExSearchItem>()
                  .Where(whereExpression)
                  .ToList()
                  .Select(searchItem => searchItem.ItemId.GetIdentifier());
            }
        }

        private Expression<Func<ExSearchItem, bool>> TemplateIsMessageExpression()
        {
            return this.OrTemplateExpression(
              this.ConfigurationFactory
                .Get<NetworksConfiguration>()
                .Networks
                .SelectMany(networkSettings => networkSettings.Items.Select(messageSettings => ID.Parse(messageSettings.MessageTemplateId))));
        }

        private Expression<Func<ExSearchItem, bool>> OrTemplateExpression(IEnumerable<ID> templateIds)
        {
            var templateIdList = templateIds as IList<ID> ?? templateIds.ToList();

            if (!templateIdList.Any())
            {
                return item => false;
            }

            Expression<Func<ExSearchItem, bool>> whereExpression = null;
            foreach (var templateId in templateIdList)
            {
                var filterTemplateId = templateId;

                if (whereExpression == null)
                {
                    whereExpression = searchItem => searchItem.TemplateId == filterTemplateId;
                }
                else
                {
                    whereExpression = whereExpression.Or(searchItem => searchItem.TemplateId == filterTemplateId);
                }
            }

            return whereExpression;
        }
    }
}