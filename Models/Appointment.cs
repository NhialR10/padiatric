

namespace Padiatric.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }

        // Relationship with Assistant and Professor
        public string AssistantId { get; set; }
        public Assistant Assistant { get; set; }

        public string ProfessorId { get; set; }
        public Professor Professor { get; set; }
    }

}
