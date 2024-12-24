using Padiatric.Models;

namespace Padiatric.ViewModels
{
    public class UserWithRolesViewModel
    {
        public AppUser User { get; set; }
        public List<string> Roles { get; set; }
    }

}
