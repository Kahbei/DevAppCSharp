using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishBattle.data.Services
{
    public class JoueurService
    {
        private EnglishBattleEntities context;

        public JoueurService(EnglishBattleEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Recupère un joueur grâce à son id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>un objet joueur</returns>
        public Joueur GetJoueur(int id)
        {
            using (context)
            {
                return context.Joueur.Find(id);
            }
        }

        /// <summary>
        /// Insert un joueur dans la base
        /// </summary>
        /// <param name="joueur"> informations du joueur dans l'objet </param>
        public void Insert(Joueur joueur)
        {
            using (context)
            {
                context.Joueur.Add(joueur);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Connexion du joueur
        /// </summary>
        /// <param name="email"> email du joueur </param>
        /// <param name="motDePasse"> mdp du joueur </param>
        /// <returns> objet joueur </returns>
        public Joueur GetJoueur(string email, string motDePasse)
        {
            using (context)
            {
                IQueryable<Joueur> joueurs = from joueur in context.Joueur
                                                       where joueur.email == email
                                                       && joueur.motDePasse == motDePasse
                                                       select joueur;
                return joueurs.FirstOrDefault();
            }
        }

        /// <summary>
        /// Récupère l'id de la ville du joueur
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns> id ville</returns>
        public int GetIdVille(int id)
        {
            using (context)
            {
                IQueryable<int> idVille = from joueur in context.Joueur
                                             where joueur.id == id
                                             select joueur.idVille;

                return idVille.FirstOrDefault();
            }
        }
    }
}
