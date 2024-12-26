
namespace FamilyAppointmentsMobile.Models
{
    public class TodoList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsShoppingList { get; set; }
        public List<TodoTask> Todos { get; set; } = new List<TodoTask>();
    }
}
