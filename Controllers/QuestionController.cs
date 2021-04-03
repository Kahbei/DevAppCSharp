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
            var partieEnCours = (Partie)Session["partie"];

            if (joueur != null)
            {
                // Si il n'y a pas eu de partie créer.
                if( partieEnCours == null)
                {
                    // Appelle le service pour créer une nouvelle partie.
                    PartieService partieService = new PartieService(new EnglishBattle.data.EnglishBattleEntities());
                    Partie partie = new Partie
                    {
                        idJoueur = joueur.id,
                        score = 0,
                    };
            
                    partieService.InsertPartie(partie);

                    Session["partie"] = partie;

                    NewQuestion(partie.id);
                }
                else if(partieEnCours != null)
                {
                    NewQuestion(partieEnCours.id);
                }
            }

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
                // Récupère les services
                QuestionService questionService = new QuestionService(new EnglishBattle.data.EnglishBattleEntities());
                PartieService partieService = new PartieService(new EnglishBattle.data.EnglishBattleEntities());

                // Récupère de la valeur session contenant les informations à l'arrivée d'une question.
                Question fromSession = (Question)Session["questionInfo"];
                DateTime dateAnswer = DateTime.Now;

                // Objet Question qui va être insérer dans la base.
                Question question = new Question
                {
                    idPartie = fromSession.idPartie,
                    idVerbe = fromSession.idVerbe,
                    dateEnvoie = fromSession.dateEnvoie,
                    reponseParticipePasse = model.ParticipePasse,
                    reponsePreterit = model.Preterit,
                    dateReponse = dateAnswer,
                };

                Partie partie = (Partie)Session["partie"];

                questionService.InsertQuestion(question);

                // Check if the answers are good or not
                if (isCorrectAnswers(question.idVerbe, question.reponsePreterit, question.reponseParticipePasse))
                {
                    ViewBag.answer = "Good Answers";
                    partie.score++;
                    ViewBag.score = partie.score;
                    ViewBag.part = partie.Joueur;

                    return RedirectToAction("Question", "Question");                    
                }
                else
                {
                    partieService.UpdatePartie(partie);

                    Session["partie"] = null;

                    ViewBag.answer = "Bad Answers";

                    return RedirectToAction("Index", "Home");
                }

            }

            return View();
        }

        private int GetRandomVerb()
        {
            VerbeService verbeService = new VerbeService(new EnglishBattle.data.EnglishBattleEntities());
            List<Verbe> listVerb = verbeService.GetVerbList();
            Random rndNum = new Random();

            return rndNum.Next(1, listVerb.Count + 1);
        }

        private void NewQuestion(int partieID)
        {
            // Initialise les données dès que la question s'affiche.
            Question question = new Question
            {
                idPartie = partieID,
                dateEnvoie = DateTime.Now,
                idVerbe = GetRandomVerb(),
            };

            Session["questionInfo"] = question;

            VerbeService verbeService = new VerbeService(new EnglishBattle.data.EnglishBattleEntities());
            ViewBag.verbe = verbeService.GetVerbItem(question.idVerbe).baseVerbale;
        }

        private bool isCorrectAnswers(int idVerb, string preterit, string partpast)
        {
            VerbeService verbeService = new VerbeService(new EnglishBattle.data.EnglishBattleEntities());
            Verbe verbe = verbeService.GetVerbItem(idVerb);

            return (preterit == verbe.participePasse && partpast == verbe.preterit) ? true : false ;
        }
    }
}