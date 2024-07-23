namespace SellMilk.Api.Models.Common
{
    public class PagingResult<T>
    {
        //public PagingResult(List<T>? items, int pageIndex, int pageSize, int totalRecords,int totalPages) 
        //{ 
        //    Items = items;

        //    PageIndex = pageIndex;

        //    PageSize = pageSize;

        //    TotalRecords = totalRecords;

        //    TotalPages = totalPages;
        //}

        public List<T>? Items { set; get; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public long? TotalRecords { get; set; }

        public long? TotalPages { get; set; }
    }
}
