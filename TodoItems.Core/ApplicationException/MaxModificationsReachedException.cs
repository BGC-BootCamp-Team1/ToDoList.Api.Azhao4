public class MaxModificationsReachedException : Exception
{
    public MaxModificationsReachedException()
        : base("You have reached the maximum number of modifications for today. Please try agian tomorrow.")
    {
    }
}
