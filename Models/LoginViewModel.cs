using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishBattleApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Courrier électronique")]
        [EmailAddress] // si email valide
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}