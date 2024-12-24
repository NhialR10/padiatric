namespace Padiatric.Models
{
    public class Emergency
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateReported { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }

}
