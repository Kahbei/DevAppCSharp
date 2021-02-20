using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishBattle.data.Services
{
    class QuestionService
    {
        private EnglishBattleEntities context;

        /// <summary>
        /// Get a Question following the id picked
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Question by the id</returns>
        public Question GetQuestion(int id)
        {
            using (context)
            {
                return context.Question.Find(id);
            }
        }

        /// <summary>
        /// Get a Question list
        /// </summary>
        /// <returns>Question list</returns>
        public List<Question> GetQuestionList()
        {
            using (context)
            {
                return context.Question.ToList();
            }
        }

        /// <summary>
        /// Add a Question
        /// </summary>
        /// <param name="question"></param>
        public void InsertQuestion(Question question)
        {
            using (context)
            {
                context.Question.Add(question);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update a question
        /// </summary>
        /// <param name="question"></param>
        public void UpdateQuestion(Question question)
        {
            using (context)
            {
                context.Entry(question).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete a question
        /// </summary>
        /// <param name="question"></param>
        public void DeleteQuestion(Question question)
        {
            using (context)
            {
                context.Entry(question).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
