using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnglishBattleApp.Controllers;
using EnglishBattle.data;
using EnglishBattleApp.Models;
using EnglishBattle.data.Services;

namespace EnglishBattleApp.Controllers
{
    public class QuestionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Question()
        {
            var joueur = (Joueur)Session["joueur"];
            PartieService partieService = new PartieService(new EnglishBattle.data.EnglishBattleEntities());
            Partie partie = new Partie
            {
                idJoueur = joueur.id,
                score = 0,
            };
            partieService.InsertPartie(partie);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Question(QuestionViewModel model)
        {
            //validation côté serveur
            if (ModelState.IsValid)
            {
                //inscription en base de données
                QuestionService questionService = new QuestionService(new EnglishBattle.data.EnglishBattleEntities());

                Question question = new Question();

                question.reponseParticipePasse = model.ParticipePasse;
                question.reponsePreterit = model.Preterit;

                questionService.InsertQuestion(question);

                //pas obligatoire, c'est moi qui m'amuse !
                ViewBag.VerbInfinitif = "Bonne réponse !";
            }

            //ViewBag.VerbInfinitif = "Guru Project Infinity !";
            return View();
        }
    }
}