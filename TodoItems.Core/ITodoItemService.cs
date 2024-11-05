using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TodoItems.Core.ConstantsAndEnums;

namespace TodoItems.Core
{
    public interface ITodoItemService
    {
        public TodoItem Create(string description, DateOnly? manualSetDueDate, DueDateSetStrategy strategy = DueDateSetStrategy.Manual);

        public TodoItem ModifyDescription(string id, string description);
    }
}