using System.ComponentModel;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemCreateFields
    {
        public int TodoListId { get; set; }
        public string Title { get; set; }
        public string TodoListTitle { get; set; }
        [DisplayName("Responsible User")]
        public string ResponsiblePartyId { get; set; }
        public Importance Importance { get; set; }
        public int Rank { get; set; }

        public TodoItemCreateFields() { }

        public TodoItemCreateFields(int todoListId, string todoListTitle, string responsiblePartyId, int rank)
        {
            TodoListId = todoListId;
            TodoListTitle = todoListTitle;
            ResponsiblePartyId = responsiblePartyId;
            Rank = rank;
        }
    }
}