

namespace Padiatric.Models
{
    public class Professor : AppUser
    {
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        // Navigational properties for appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
