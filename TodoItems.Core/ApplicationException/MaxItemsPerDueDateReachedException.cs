public class MaxItemsPerDueDateReachedException : Exception
{
    public MaxItemsPerDueDateReachedException(DateOnly dueDate, int maxItems)
        : base($"Cannot create new Todo item completed on { dueDate }, already reach max limit({ maxItems})")
    {
    }
}
