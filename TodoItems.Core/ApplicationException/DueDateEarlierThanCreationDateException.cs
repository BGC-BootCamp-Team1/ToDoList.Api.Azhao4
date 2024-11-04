namespace TodoItems.Core.ApplicationException
{
    public class DueDateEarlierThanCreationDateException : Exception
    {
        public DueDateEarlierThanCreationDateException()
            : base("Cannot create todo item that due date earlier than creation date")
        {
        }
    }
}