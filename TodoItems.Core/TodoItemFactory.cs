
using static TodoItems.Core.ConstantsAndEnums;
using TodoItems.Core.ApplicationException;

namespace TodoItems.Core
{
    public static class TodoItemFactory
    {
        public static TodoItem CreateItem(ITodosRepository _todosRepository, string description, DateOnly? manualSetDueDate, ConstantsAndEnums.DueDateSetStrategy strategy)
        {
            var newItem = new TodoItem(description, manualSetDueDate);

            if (manualSetDueDate != null)
            {
                ValidateSpecifiedDueDate(newItem, (DateOnly)manualSetDueDate, _todosRepository);
            }
            else if (strategy == DueDateSetStrategy.FirstDateOfNextFiveDays)
            {
                CreateItemWithFirstDateOfNextFiveDays(newItem, _todosRepository);
            }
            else if (strategy == DueDateSetStrategy.MostFreeDateOfNextFiveDays)
            {
                CreateItemWithMostFreeDueDateOfNextFiveDays(_todosRepository, newItem);
            }

            return newItem;
        }

        private static void CreateItemWithMostFreeDueDateOfNextFiveDays(ITodosRepository _todosRepository, TodoItem newItem)
        {
            var createdDate = DateOnly.FromDateTime(newItem.CreatedTime.Date);
            DateOnly mostFreeDate = createdDate;
            int smallestCountPerDay = int.MaxValue;
            for (var day = 0; day < 5; day++)
            {
                var dueDate = createdDate.AddDays(day);
                var itemCount = _todosRepository.GetCountByDueDate(dueDate);
                if (itemCount < smallestCountPerDay)
                {
                    smallestCountPerDay = itemCount;
                    mostFreeDate = dueDate;
                }
            }

            if (smallestCountPerDay >= MAX_ITEMS_PER_DUE_DATE)
            {
                throw new NoAvailableDateInNextFiveDays();
            }
            newItem.DueDate = mostFreeDate;
        }

        private static void CreateItemWithFirstDateOfNextFiveDays(TodoItem newItem, ITodosRepository _todosRepository)
        {
            var createdDate = DateOnly.FromDateTime(newItem.CreatedTime.Date);
            for (var day = 0; day < 5; day++)
            {
                var dueDate = createdDate.AddDays(day);

                var itemCount = _todosRepository.GetCountByDueDate(dueDate);

                if (itemCount < MAX_ITEMS_PER_DUE_DATE)
                {
                    newItem.DueDate = dueDate;
                    return;
                }
            }
            throw new NoAvailableDateInNextFiveDays();
        }

        private static void ValidateSpecifiedDueDate(TodoItem newItem, DateOnly manualSetDueDate, ITodosRepository _todosRepository)
        {
            if (manualSetDueDate < DateOnly.FromDateTime(newItem.CreatedTime.Date))
            {
                throw new DueDateEarlierThanCreationDateException();
            }

            var itemCount = _todosRepository.GetCountByDueDate(manualSetDueDate);

            if (itemCount >= MAX_ITEMS_PER_DUE_DATE)
            {
                throw new MaxItemsPerDueDateReachedException(manualSetDueDate, MAX_ITEMS_PER_DUE_DATE);
            }
        }
    }
}