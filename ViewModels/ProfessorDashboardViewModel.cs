using Padiatric.Models;

namespace Padiatric.ViewModels
{
    public class ProfessorDashboardViewModel
    {
        public Department Department { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<Assistant> Assistants { get; set; }
        public List<Emergency> Emergencies { get; set; }
    }
}
