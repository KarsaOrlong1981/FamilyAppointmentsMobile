
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace FamilyAppointmentsMobile.Models
{
    public class TodoTask
    {
        public string Id { get; set; }
        public string Description { get; set; } 
        public bool IsDone { get; set; } 
        public ECategorieType CategorieType { get; set; }
        public string TodoListId { get; set; }

        public ICommand DeleteCommand { get; set; }

        public TodoTask()
        {

        }

        public TodoTask(Action<TodoTask> deleteAction)
        {
            DeleteCommand = new RelayCommand(() => deleteAction(this));
        }
    }
}
