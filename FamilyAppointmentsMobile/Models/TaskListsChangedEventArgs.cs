using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAppointmentsMobile.Models
{
    public class TaskListsChangedEventArgs
    {
        public TodoList TodoList { get; set; }
        public ETodoOperationType TodoOperationType { get; set; }
    }
}
