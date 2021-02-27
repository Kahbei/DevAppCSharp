using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishBattleApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nom")] // utiliser dans le label associé au champ
        public string Nom { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required]
        [Display(Name = "Courrier électronique")]
        [EmailAddress] // si email valide
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        [StringLength(14, ErrorMessage = "Le mot de passe doit être compris entre entre {2} et {1}", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirmer le mot de passe")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le mot de passe de correspond pas")]
        public string ConfirmPassword { get; set; }

        // niveau 
        [Required]
        [Display(Name = "Niveau")]
        public string Niveau { get; set; }

        // ville
        [Required]
        [Display(Name = "Ville")]
        public string Ville { get; set; }
    }
}