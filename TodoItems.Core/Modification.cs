namespace TodoItems.Core
{
    public class Modification
    {
        public DateTime time {  get; init; }
        public Modification() 
        { 
            time = DateTime.Now;
        }
    }
}