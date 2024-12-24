using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Padiatric.Data;
using Padiatric.Models;
using Padiatric.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Padiatric.Controllers
{
    public class AssistantController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AssistantController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the logged-in assistant
            var assistant = await _userManager.GetUserAsync(User) as Assistant;

            // Check if the assistant exists
            if (assistant == null)
            {
                return NotFound("Assistant not found.");
            }

            // Get shifts for the assistant and include the department
            var shifts = await _context.Shifts
                .Include(s => s.Department) // Include the Department related to the Shift
                .Where(s => s.AssistantId == assistant.Id) // Get shifts for the specific assistant
                .ToListAsync();

            // Get the department from the first shift (assuming all shifts belong to the same department)
            var department = shifts.Select(s => s.Department).FirstOrDefault();

            if (department == null)
            {
                return NotFound("Department not found for the assistant.");
            }

            // Pass the department (and any related shifts) to the view
            return View(department);
        }


        // Appointments
        public async Task<IActionResult> Appointments()
        {
            var assistant = await _userManager.GetUserAsync(User) as Assistant;

            if (assistant == null)
            {
                return NotFound("Assistant not found.");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Professor) // Eagerly load the Professor
                .Where(a => a.AssistantId == assistant.Id)
                .ToListAsync();

            return View(appointments);
        }


        // Duty Roster (Calendar View)
        public async Task<IActionResult> DutyRoster()
        {
            var assistant = await _userManager.GetUserAsync(User) as Assistant;

            if (assistant == null)
            {
                return NotFound("Assistant not found.");
            }

            var shifts = await _context.Shifts
                .Include(s => s.Department)
                .Where(s => s.AssistantId == assistant.Id)
                .ToListAsync();

            return View(shifts);
        }


        // Display appointment creation form
        public IActionResult CreateAppointment()
        {
            // Get the logged-in assistant's ID
         
            var assistantEmail = User?.Identity?.Name;
            var assistant = _context.Users
          .FirstOrDefault(u => u.Email == assistantEmail);
            // Get shifts for the assistant
            
                var shifts = _context.Shifts
                    .Include(s => s.Department)
                    .Where(s => s.AssistantId == assistant.Id)
                    .ToList();
            
            // Get unique departments the assistant has shifts in
            var departments = shifts.Select(s => s.Department).Distinct().ToList();

            // Get professors from these departments
            var professors = _context.Professors
                .Where(p => departments.Contains(p.Department))
                .ToList();

            // Hardcoded time slots for simplicity
            var hardcodedSlots = new List<SelectListItem>
        {
            new SelectListItem { Value = "2024-12-15T09:00", Text = "15 Dec 2024, 9:00 AM" },
            new SelectListItem { Value = "2024-12-15T10:00", Text = "15 Dec 2024, 10:00 AM" },
            new SelectListItem { Value = "2024-12-16T14:00", Text = "16 Dec 2024, 2:00 PM" }
        };

            var viewModel = new CreateAppointmentViewModel
            {
                Professors = professors,
                AvailableSlots = hardcodedSlots
            };

            return View(viewModel);
        }

        // Handle appointment creation
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentViewModel model)
        {
            var assistantEmail = User?.Identity?.Name;
            var assistant = _context.Users
          .FirstOrDefault(u => u.Email == assistantEmail);

            var appointment = new Appointment
            {
                
                AssistantId = assistant?.Id, // Logged-in assistant
                ProfessorId = model.SelectedProfessorId,
                AppointmentDate = DateTime.Parse(model.SelectedSlot),
                Reason = model.Reason 
        };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Appointment successfully created!";
            return RedirectToAction("Index", "Appointments");
        }

  
    }

}
