# Sitecore.Support.139772
[Social][Azure Search] PostFlaggedMessages task can cause NotSupportedException if Azure Search provider is used

```
Exception: System.NotSupportedException
Message: Query is not supported as it crosses search/filter boundaries
Source: Sitecore.ContentSearch.Azure
   at Sitecore.ContentSearch.Azure.Query.CloudQueryBuilder.Merge(String query1, String query2, String operand, ShouldWrap wrap)
   at Sitecore.ContentSearch.Azure.Query.CloudQueryMapper.HandleOr(OrNode node, CloudQueryMapperState mappingState)
   at Sitecore.ContentSearch.Azure.Query.CloudQueryMapper.HandleCloudQuery(QueryNode node, CloudQueryMapperState mappingState)
   at Sitecore.ContentSearch.Azure.Query.CloudQueryMapper.HandleAnd(AndNode node, CloudQueryMapperState mappingState)
   at Sitecore.ContentSearch.Azure.Query.CloudQueryMapper.HandleCloudQuery(QueryNode node, CloudQueryMapperState mappingState)
   at Sitecore.ContentSearch.Azure.Query.CloudQueryMapper.HandleAnd(AndNode node, CloudQueryMapperState mappingState)
   at Sitecore.ContentSearch.Azure.Query.CloudQueryMapper.HandleCloudQuery(QueryNode node, CloudQueryMapperState mappingState)
   at Sitecore.ContentSearch.Azure.Query.CloudQueryMapper.MapQuery(IndexQuery query)
   at Sitecore.ContentSearch.Linq.Parsing.GenericQueryable`2.GetQuery(Expression expression)
   at Sitecore.ContentSearch.Linq.Parsing.GenericQueryable`2.GetEnumerator()
   at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
   at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
   at Sitecore.Social.Search.SearchProvider.SearchItems(Expression`1 whereExpression, Func`2 selector)
   at Sitecore.Social.Search.SearchProvider.GetMessagesReadyToPostAutomatically(IEnumerable`1 accountIds)
   at Sitecore.Social.MessageBusinessManager.GetMessagesReadyToPostAutomatically(IEnumerable`1 accountIds)
   at Sitecore.Social.Client.Tasks.PostFlaggedMessages.DoRun()
```

## License  
This patch is licensed under the [Sitecore Corporation A/S License for GitHub](https://github.com/sitecoresupport/Sitecore.Support.139772/blob/master/LICENSE).  

## Download  
Downloads are available via [GitHub Releases](https://github.com/sitecoresupport/Sitecore.Support.139772/releases).  
