

namespace Padiatric.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HastaSayisi { get; set; }
        public int BosyatakSayisi { get; set; }
        // Relationship with Professors
        public ICollection<Professor> Professors { get; set; } = new List<Professor>();

        // Relationship with Assistants
        public ICollection<Assistant> Assistants { get; set; } = new List<Assistant>();

        // Relationship with Shifts
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();

        // Relationship with Emergencies
        public ICollection<Emergency> Emergencies { get; set; } = new List<Emergency>();
    }

}
