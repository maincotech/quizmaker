using System.Collections.Generic;

namespace Maincotech.ExamAssistant
{
    public class LoadMoreResult<TData>
    {

        public LoadMoreResult(bool hasMoreData, int nextOffset, List<TData> items)
        {
            HasMoreData = hasMoreData;
            NextOffset = nextOffset;
            Items = items;        }

        public bool HasMoreData { get; set; }
        public int NextOffset { get; set; }
        public List<TData> Items { get; set; }
    }
}