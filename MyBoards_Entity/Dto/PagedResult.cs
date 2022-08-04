﻿using MyBoards_Entity.Entities;

namespace MyBoards_Entity.Dto
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalPages = totalCount;
            ItemsFrom = pageSize * (pageNumber-1)+1;
            ItemsTo = ItemsFrom +pageSize-1;
            TotalItemsCount = (int)Math.Ceiling(totalCount/(double)pageSize);  
        }
    }
}
