namespace TodoApi.helper
{
    public class UserParams
    {
        private int Maxsize=50;
        public int currentUserId { get; set; }
        public int CurentPage { get; set; } =1;
        private int pageSize=10;
        public int MinAge { get; set; }=18;
        public int MaxAge { get; set; }=99;
        public string Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize= value< Maxsize ? value: Maxsize; }
        }
        public string SortOrder { get; set; }
        public byte SortType { get; set; }=0;
        public bool Likers { get; set; }=false;
        public bool Likeds { get; set; }=false;
        
    }
}