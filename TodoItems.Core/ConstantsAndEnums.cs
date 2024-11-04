namespace TodoItems.Core
{
    public class ConstantsAndEnums
    {
        public const int MAX_ITEMS_PER_DUE_DATE = 8;

        public const int MAX_MODIFY_TIME_ONE_DAY = 3;

        public enum DueDateSetStrategy
        {
            FirstDateOfNextFiveDays, 
            MostFreeDateOfNextFiveDays,
            Manual
        }
    }
}
