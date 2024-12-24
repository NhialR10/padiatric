

namespace Padiatric.Models
{
    public class Assistant:AppUser
    {
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
    }
}
