﻿namespace Maincotech.ExamAssitant
{
    public class LoadMoreQuery
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string SearchText { get; set; }
    }
}