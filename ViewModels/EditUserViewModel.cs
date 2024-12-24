using Padiatric.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Padiatric.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }

}
