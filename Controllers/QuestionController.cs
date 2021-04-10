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
        private List<int> listVerb = new List<int>();

        // Services
        private VerbeService verbeService = new VerbeService(new EnglishBattle.data.EnglishBattleEntities());
        private PartieService partieService = new PartieService(new EnglishBattle.data.EnglishBattleEntities());
        private QuestionService questionService = new QuestionService(new EnglishBattle.data.EnglishBattleEntities());

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
                    // Création d'une nouvelle partie
                    Partie partie = new Partie
                    {
                        idJoueur = joueur.id,
                        score = 0,
                    };
            
                    partieService.InsertPartie(partie);

                    for(int i = 1; i <= 160; i++)
                    {
                        listVerb.Add(i);
                    }

                    Session["listVerb"] = listVerb;
                    partie.score = listVerb.Count;

                    Session["partie"] = partie;


                    NewQuestion(partie.id);
                }
                else if(partieEnCours != null)
                {
                    if(TempData["message"] != null)
                    {
                        ViewBag.message = TempData["message"].ToString();
                    }

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
                VerbeService verbeService = new VerbeService(new EnglishBattle.data.EnglishBattleEntities());

                // Récupère de la valeur session contenant les informations à l'arrivée d'une question.
                Question fromSession = (Question)Session["questionInfo"];
                Partie partie = (Partie)Session["partie"];

                DateTime dateAnswer = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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

                questionService.InsertQuestion(question);

                DateTime a = question.dateEnvoie;
                DateTime b = (DateTime)question.dateReponse;

                if (b.Subtract(a).TotalSeconds >= 60)
                {
                    partieService.UpdatePartie(partie);

                    Session["partie"] = null;
                    Session["listVerb"] = null;
                    Session["questionInfo"] = null;

                    TempData["message"] = "You took too much time, more than 1mn !";

                    return RedirectToAction("Index", "Home");
                } 
                else if (isCorrectAnswers(question.idVerbe, question.reponsePreterit, question.reponseParticipePasse))
                {
                    TempData["message"] = "Good Answers";

                    partie.score++;
                    partieService.UpdatePartie(partie);

                    if (partie.score % 5 == 0)
                    {
                        TempData["message"] = "Good Answer ! It was the " + partie.score + " in a row !";

                        return RedirectToAction("Question", "Question");
                    }
                    else if (partie.score == verbeService.GetVerbList().Count)
                    {
                        TempData["message"] = "Félicitations ! Vous avez réussi à trouver le prétérit et le participe passé de tous les verbes irréguliers !";

                        return RedirectToAction("End", "Question");
                    }
                    else
                    {
                        return RedirectToAction("Question", "Question");
                    }
                }
                else
                {
                    partieService.UpdatePartie(partie);

                    Session["partie"] = null;
                    Session["listVerb"] = null;
                    Session["questionInfo"] = null;

                    TempData["message"] = "Mauvaise réponse ! Vous avez perdu !";

                    return RedirectToAction("End", "Question");
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
                dateEnvoie = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                idVerbe = GetRandomVerb(),
            };

            Session["questionInfo"] = question;

            List<int> verbList = (List<int>)Session["listVerb"];

            if(verbList != null)
            {
                while (verbList.Contains(question.idVerbe))
                {
                    question.idVerbe = GetRandomVerb();
                }
            }

            verbList.Add(question.idVerbe);

            Session["listVerb"] = verbList;
            
            ViewBag.verbe = verbeService.GetVerbItem(question.idVerbe).baseVerbale;
        }

        private bool isCorrectAnswers(int idVerb, string preterit, string partpast)
        {
            Verbe verbe = verbeService.GetVerbItem(idVerb);

            return (preterit == verbe.participePasse && partpast == verbe.preterit) ? true : false ;
        }

        public ActionResult End()
        {
            if (TempData["message"] != null)
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }
    }
}