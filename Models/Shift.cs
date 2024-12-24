

namespace Padiatric.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public string AssistantId { get; set; }
        public Assistant Assistant { get; set; } // One Assistant can have many Shifts

        public int DepartmentId { get; set; }
        public Department Department { get; set; } // One Department can have many Shifts

        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
    }
}
