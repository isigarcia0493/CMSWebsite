﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMSWebsite.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Please enter a role name")]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }
}
