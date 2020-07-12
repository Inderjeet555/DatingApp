namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize= 50;
        public int PageNumber { get; set; } = 1;
        private int pazesize = 10;
        public int PazeSize
        {
            get { return pazesize; }
            set { pazesize = (value > MaxPageSize) ? MaxPageSize: value; }
        }
        
    }
}