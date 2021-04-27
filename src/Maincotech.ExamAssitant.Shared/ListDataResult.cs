using System.Collections.Generic;

namespace Maincotech.ExamAssistant
{
    public class ListDataResult<TData>
    {
        public ListDataResult(string nextPageToken, List<TData> items)
        {
            HasMoreData = !string.IsNullOrEmpty(nextPageToken);
            NextPageToken = nextPageToken;
            Items = items;
        }

        public bool HasMoreData { get; }
        public string NextPageToken { get; }
        public List<TData> Items { get; set; }
    }
}