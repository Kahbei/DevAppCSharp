//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnglishBattle.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        public int id { get; set; }
        public int idPartie { get; set; }
        public int idVerbe { get; set; }
        public string reponseParticipePasse { get; set; }
        public string reponsePreterit { get; set; }
        public System.DateTime dateEnvoie { get; set; }
        public Nullable<System.DateTime> dateReponse { get; set; }
    
        public virtual Partie Partie { get; set; }
        public virtual Verbe Verbe { get; set; }
    }
}
