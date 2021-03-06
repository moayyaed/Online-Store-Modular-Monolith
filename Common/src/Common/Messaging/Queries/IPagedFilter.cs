using System.Collections.Generic;

namespace Common.Messaging.Queries
{
    public interface IPagedFilter<TResult, in TQuery> where TQuery : IQuery
    {
        Paged<TResult> Filter(IEnumerable<TResult> values, TQuery query);
    }
}