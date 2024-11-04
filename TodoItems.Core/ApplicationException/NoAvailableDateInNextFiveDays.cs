namespace TodoItems.Core.ApplicationException;
public class NoAvailableDateInNextFiveDays : Exception
{
    public NoAvailableDateInNextFiveDays()
        : base("Cannot create new Todo item with due date on next five days")
    {
    }
}