using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishBattle.data.Services
{
    public class VilleService
    {
        private EnglishBattleEntities context;

        public VilleService(EnglishBattleEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retourne une ville avec son id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>objet ville</returns>
        public Ville GetVilleItem(int id)
        {
            using (context)
            {
                return context.Ville.Find(id);
            }
        }

        /// <summary>
        /// Retourne la liste des villes
        /// </summary>
        /// <returns> liste des villes </returns>
        public List<Ville> GetVilleList()
        {
            using (context)
            {
                return context.Ville.ToList();
            }
        }
    }
}
