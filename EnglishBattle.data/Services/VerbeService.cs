using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishBattle.data.Services
{
    public class VerbeService
    {
        private EnglishBattleEntities context;

        public VerbeService(EnglishBattleEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get a verb following the id picked
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Verb by its id</returns>
        public Verbe GetVerbItem(int id)
        {
            using (context)
            {
                return context.Verbe.Find(id);
            }
        }

        /// <summary>
        /// Get verb list
        /// </summary>
        /// <returns>Verb list</returns>
        public List<Verbe> GetVerbList()
        {
            using (context)
            {
                return context.Verbe.ToList();
            }
        }
    }
}
