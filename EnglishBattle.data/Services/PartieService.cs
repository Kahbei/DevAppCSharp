using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishBattle.data.Services
{
    public class PartieService
    {
        private EnglishBattleEntities context;

        public PartieService(EnglishBattleEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get a partie following the id selected
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Partie by id seleted</returns>
        public Partie GetPartieItem(int id)
        {
            using (context)
            {
                return context.Partie.Find(id);
            }
        }

        /// <summary>
        /// Get a partie following the idJoueur
        /// </summary>
        /// <param name="idJoueur"></param>
        /// <returns></returns>
        public Partie GetPartieByJoueur(int idJoueur)
        {
            using (context)
            {
                IQueryable<Partie> parties = from partie in context.Partie where partie.idJoueur == idJoueur orderby partie.score ascending select partie ;

                return parties.FirstOrDefault();
            }
        }

        /// <summary>
        /// Get the list of partie
        /// </summary>
        /// <returns>Partie list</returns>
        public List<Partie> GetPartieList()
        {
            using (context)
            {
                return context.Partie.ToList();
            }
        }

        /// <summary>
        /// Insert a new Partie
        /// </summary>
        /// <param name="partie"></param>
        public void InsertPartie(Partie partie)
        {
            using (context)
            {
                context.Partie.Add(partie);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update a partie
        /// </summary>
        /// <param name="partie"></param>
        public void UpdatePartie(Partie partie)
        {
            using (context)
            {
                context.Entry(partie).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
