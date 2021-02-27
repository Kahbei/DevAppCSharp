using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishBattleApp.Models
{
    public class QuestionViewModel
    {
        [Required]
        [Display(Name = "Preterit")]
        public string Preterit { get; set; }

        [Required]
        [Display(Name = "Participe passé")]
        public string ParticipePasse { get; set; }
    }
}