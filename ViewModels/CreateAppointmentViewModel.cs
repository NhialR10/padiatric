using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Padiatric.Models;

namespace Padiatric.ViewModels
{
    public class CreateAppointmentViewModel
    {
        // List of available professors for the assistant to choose from
        public IEnumerable<Professor> Professors { get; set; }

        // Hardcoded list of available appointment slots
        public IEnumerable<SelectListItem> AvailableSlots { get; set; }

        // The selected professor's ID (from the dropdown)
        public string SelectedProfessorId { get; set; }

        // The selected appointment slot (from the dropdown)
        public string SelectedSlot { get; set; }

        // The reason for the appointment
        public string Reason { get; set; }
    }
}
