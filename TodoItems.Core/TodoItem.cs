namespace TodoItems.Core;
using static TodoItems.Core.ConstantsAndEnums;

public class TodoItem
{
    public string Id { get; init; }
    public DateOnly? DueDate { get; set; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; init; }
    public List<Modification> ModificationRecords { get; set; }

    public TodoItem()
    {
        this.ModificationRecords = new List<Modification>();
        this.CreatedTime = DateTime.Now;
    }

    public TodoItem(string description, DateOnly? dueDate)
    {
        this.Description = description;
        this.Id = Guid.NewGuid().ToString();
        this.DueDate = dueDate; // todo: change type to DateOnly
        this.CreatedTime = DateTime.Now;
        this.ModificationRecords = new List<Modification>();
    }

    public void ModifyItem(string description)
    {
        var today = DateTime.Today;
        var todayModicationsCount = ModificationRecords.Count(r => r.time.Date == today);
        if (todayModicationsCount < MAX_MODIFY_TIME_ONE_DAY)
        {
            this.Description = description;
            this.ModificationRecords.Add(new Modification());
        }
        else
        {
            throw new MaxModificationsReachedException();
        }
    }
}
