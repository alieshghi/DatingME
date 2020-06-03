namespace TodoApi.helper
{
    public class PaginationHeader
    {
        public int TotalPage { get; set; }
        public int PageSize { get; set; }

        public int CurentPage { get; set; }
        public int TotalCountOfItems { get; set; }
        public PaginationHeader(int totalPage, int pageSize, int curentPage, int totalCountOfItems)
        {
           this.TotalCountOfItems =totalCountOfItems;
           this.PageSize= pageSize;
           this.CurentPage=curentPage;
           this.TotalPage=totalPage ;
        }
    }
}