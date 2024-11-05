using TodoItems.Core;

namespace TodoItems.Test;

public class TodoItemTest
{
    private readonly string _description = "this is description";
    private readonly DateOnly _dueDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(5));

    [Fact]
    public void Should_get_return_todo_item_description_and_due_date()
    {
        TodoItem todoItem = new TodoItem(_description, _dueDate);
        Assert.Equal(_description, todoItem.Description);
        Assert.Equal(_dueDate, todoItem.DueDate);
        Assert.Empty(todoItem.ModificationRecords);
    }

    [Fact]
    public void Should_update_content_when_edit()
    {
        TodoItem todoItem = new TodoItem(_description, _dueDate);
        string updateDespcription = "new content";
        todoItem.ModifyItem(updateDespcription);

        Assert.Equal(updateDespcription, todoItem.Description);
        Assert.Single(todoItem.ModificationRecords);
    }

    [Fact]
    public void Should_record_modification_frequency()
    {
        TodoItem todoItem = new TodoItem(_description, _dueDate);
        string updateDescription = "new content";

        todoItem.ModifyItem(updateDescription);
        todoItem.ModifyItem(updateDescription);
        todoItem.ModifyItem(updateDescription);

        Assert.Equal(3, todoItem.ModificationRecords.Count());
    }

    [Fact]
    public void Should_limit_daily_modification_frequency_up_to_3()
    {
        TodoItem todoItem = new TodoItem(_description, _dueDate);
        string updateDescription = "new content";
        string expectedErrMsg = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";

        todoItem.ModifyItem(updateDescription);
        todoItem.ModifyItem(updateDescription);
        todoItem.ModifyItem(updateDescription);

        var exception = Assert.Throws<MaxModificationsReachedException>(() => todoItem.ModifyItem(updateDescription));
        Assert.Equal(expectedErrMsg, exception.Message);
        Assert.Equal(3, todoItem.ModificationRecords.Count());
    }
}