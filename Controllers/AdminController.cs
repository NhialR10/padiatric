 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Padiatric.Data;
using Padiatric.Models;
using Padiatric.ViewModels;

using System.Linq;
using System.Threading.Tasks;

namespace Padiatric.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context; // Add DbContext for accessing departments

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        // Display Users List
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                
                .ToListAsync();

            var usersWithRoles = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = (await _userManager.GetRolesAsync(user)).ToList(); // Convert to List<string>
                usersWithRoles.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roles
                });
            }

            return View(usersWithRoles);
        }


        // Edit User (GET)
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = roles.ToList(), // Convert roles to List<string>
                AllRoles = _roleManager.Roles.Select(r => r.Name).ToList()
            };

            return View(model);
        }


        // Edit User (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Users));
            }

            ModelState.AddModelError("", "Error updating user.");
            return View(model);
        }

        // Delete User
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Users));
        }
        // GET: Admin/Register
        public IActionResult Register()
        {
            var model = new RegisterUserViewModel
            {
                Departments = _context.Departments.ToList() // Fetch departments for dropdown
            };
            return View(model);
        }


        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> Assistants()
        {
            var professor = await _userManager.GetUserAsync(User) as Professor;

            if (professor?.DepartmentId == null)
            {
                return NotFound("Department not found for the professor.");
            }

            var assistants = await _context.Shifts
                .Where(s => s.DepartmentId == professor.DepartmentId)
                .Select(s => s.Assistant)
                .Distinct()
                .ToListAsync();

            return View(assistants);
        }

        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> Appointments()
        {
            var professor = await _userManager.GetUserAsync(User) as Professor;

            if (professor?.DepartmentId == null)
            {
                return NotFound("Department not found for the professor.");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Assistant)
                .Where(a => a.ProfessorId == professor.Id)
                .ToListAsync();

            return View(appointments);
        }

        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> Shifts()
        {
            var professor = await _userManager.GetUserAsync(User) as Professor;

            if (professor?.DepartmentId == null)
            {
                return NotFound("Department not found for the professor.");
            }

            var shifts = await _context.Shifts
                .Include(a => a.Assistant)
                .Where(s => s.DepartmentId == professor.DepartmentId)
                .ToListAsync();

            return View(shifts);
        }

        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> Emergencies()
        {
            var professor = await _userManager.GetUserAsync(User) as Professor;

            if (professor?.DepartmentId == null)
            {
                return NotFound("Department not found for the professor.");
            }

            var emergencies = await _context.Emergencies
                .Where(e => e.DepartmentId == professor.DepartmentId)
                .ToListAsync();

            return View(emergencies);
        }
        // POST: Admin/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = null;

                // Create user object based on role
                if (model.Role == "Professor")
                {
                    user = new Professor
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        DateOfBirth = model.DateOfBirth,
                        DepartmentId = model.DepartmentId // Assign selected department
                    };
                }
                else if (model.Role == "Assistant")
                {
                    user = new Assistant
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        DateOfBirth = model.DateOfBirth
                    };
                }
                else if (model.Role == "Admin")
                {
                    user = new Admin
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        DateOfBirth = model.DateOfBirth
                    };
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid role selected.");
                    return View(model);
                }

                // Create the user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign the selected role
                    var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

                    if (roleResult.Succeeded)
                    {
                        // Redirect after successful registration
                        return RedirectToAction("Users", "Admin");
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If validation fails, return the registration view with errors
            model.Departments = _context.Departments.ToList(); // Re-fetch departments on error
            return View(model);
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                // Admin-specific dashboard logic
                return View("AdminDashboard");
            }
            else if (await _userManager.IsInRoleAsync(user, "Professor"))
            {
                // Redirect to Professor Dashboard
                return RedirectToAction(nameof(ProfessorDashboard));
            }
            else if (await _userManager.IsInRoleAsync(user, "Assistant"))
            {
                // Assistant-specific dashboard logic
                return View("AssistantDashboard");
            }

            return RedirectToAction("Login", "Account"); // Default redirect if role is unknown
        }

        // Professor dashboard logic
        public async Task<IActionResult> ProfessorDashboard()
        {
            var professor = await _userManager.GetUserAsync(User) as Professor;

            if (professor == null || professor.DepartmentId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Fetch data specific to the professor's department
            var department = await _context.Departments
                .Include(d => d.Shifts)
                .Include(d => d.Emergencies)
                .FirstOrDefaultAsync(d => d.Id == professor.DepartmentId);

            if (department == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Get assistants with shifts in the department
            var assistantIds = _context.Shifts
                .Where(s => s.DepartmentId == department.Id)
                .Select(s => s.AssistantId)
                .Distinct();

            var assistants = await _context.Assistants
                .Where(a => assistantIds.Contains(a.Id))
                .ToListAsync();

            // Prepare data for the view
            var viewModel = new ProfessorDashboardViewModel
            {
                Department = department,
                Shifts = department.Shifts.ToList(),
                Assistants = assistants,
                Emergencies = department.Emergencies.ToList()
            };

            return View("ProfessorDashboard", viewModel);
        }
       
    }
}
