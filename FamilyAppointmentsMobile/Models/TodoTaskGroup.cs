
using FamilyAppointmentsMobile.Helpers;

namespace FamilyAppointmentsMobile.Models
{
    public class TodoTaskGroup : List<TodoTask>
    {
        public ECategorieType CategorieType { get; private set; }
        public List<TodoTask> Tasks { get; private set; }
        public string CategorieName => CategorieHelper.HandleEnumCategorieType(CategorieType);

        public TodoTaskGroup(ECategorieType connectionType, List<TodoTask> tasks) : base(tasks)
        {
            CategorieType = connectionType;
            Tasks = tasks;  
        }
    }
}
