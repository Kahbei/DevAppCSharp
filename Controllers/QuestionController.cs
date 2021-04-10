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

            Joueur joueur = (Joueur)Session["joueur"];
            Partie partieEnCours = (Partie)Session["partie"];

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

                    // Création d'une Session pour stocker les informations de la partie
                    Session["partie"] = partie;

                    Session["listVerb"] = new List<int>();

                    // Création d'une question
                    NewQuestion(partie.id);
                }
                // Si une partie est en cours
                else if(partieEnCours != null)
                {
                    // Du fait de l'utilisation de RedirectToAction, récupération du TempData si il existe et assignation à un ViewBag
                    if(TempData["message"] != null)
                    {
                        ViewBag.message = TempData["message"].ToString();
                    }

                    // Création d'une question
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

                // Variable inutile servant pour caculer le delta entre 2 date 
                DateTime a = question.dateEnvoie;
                DateTime b = (DateTime)question.dateReponse;

                // Si le delta entre les dates d'envoie et de réponse est supérieur à 60 seconde, soit une minute
                if (b.Subtract(a).TotalSeconds >= 60)
                {
                    partieService.UpdatePartie(partie);

                    // Réinitialise les sessions en les passant à nulle
                    Session["partie"] = null;
                    Session["listVerb"] = new List<int>();
                    Session["questionInfo"] = null;

                    TempData["message"] = "You took too much time, more than 1mn !";

                    // Redirection sur la page final
                    return RedirectToAction("End", "Question");
                } 
                // Si la question a été correctement répondu
                else if (isCorrectAnswers(question.idVerbe, question.reponsePreterit, question.reponseParticipePasse))
                {
                    TempData["message"] = "Good Answers";

                    // Incrémentation du score et mis à jour du score dans la base
                    partie.score++;
                    partieService.UpdatePartie(partie);

                    // Si le score est un multiple de 5, message de Félicitation
                    if (partie.score % 5 == 0)
                    {
                        TempData["message"] = "Good Answer ! It was the " + partie.score + " in a row !";

                        return RedirectToAction("Question", "Question");
                    }
                    // Si le joueur a répondu à tout les verbes, redirection sur la page final
                    else if (partie.score == verbeService.GetVerbList().Count)
                    {
                        TempData["message"] = "Félicitations ! Vous avez réussi à trouver le prétérit et le participe passé de tous les verbes irréguliers !";

                        return RedirectToAction("End", "Question");
                    }
                    // Sinon, message de bonne réponse et passage à la question suivante
                    else
                    {
                        TempData["message"] = "Good Answer !";

                        return RedirectToAction("Question", "Question");
                    }
                }
                // Sinon, cela signifie que le joueur a mal répondu
                else
                {
                    partieService.UpdatePartie(partie);

                    // Réinitialise les sessions en les passant à nulle
                    Session["partie"] = null;
                    Session["listVerb"] = new List<int>();
                    Session["questionInfo"] = null;

                    TempData["message"] = "Mauvaise réponse ! Vous avez perdu !";

                    // Redirection sur la page final
                    return RedirectToAction("End", "Question");
                }

            }

            return View();
        }

        /**
         * Retourne un nombre aléatoire entre 1 et la taille de la liste des verbes
         **/
        private int GetRandomVerb()
        {
            VerbeService verbeService = new VerbeService(new EnglishBattle.data.EnglishBattleEntities());
            List<Verbe> listVerb = verbeService.GetVerbList();
            Random rndNum = new Random();

            return rndNum.Next(1, listVerb.Count + 1);
        }

        /**
         * Choisis le verbe auquel il faut répondre
         **/
        private void NewQuestion(int partieID)
        {
            // Initialise les données dès que la question s'affiche
            Question question = new Question
            {
                idPartie = partieID,
                dateEnvoie = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                idVerbe = GetRandomVerb(),
            };

            // Récupère la liste de verbe déjà répondu
            List<int> verbList = (List<int>)Session["listVerb"];

            // Si la liste des verbes n'est pas nulle, vérification si le verbe choisis n'a pas déjà été pris. Si oui on le modifie
            if(verbList != null)
            {
                while (verbList.Contains(question.idVerbe))
                {
                    question.idVerbe = GetRandomVerb();
                }
            }

            // Ajout du verbe dans la liste des verbes fait
            verbList.Add(question.idVerbe);

            // Stockage de la liste et de la question dans les Session
            Session["questionInfo"] = question;
            Session["listVerb"] = verbList;
            
            // On récupère le verbe via id qu'on avait définit
            ViewBag.verbe = verbeService.GetVerbItem(question.idVerbe).baseVerbale;
        }

        /**
         * Vérifie si les réponses sont bonnes. 
         * A noter que dans la base fourni, il y a eu inversement entre prétérit et participe passé
         **/
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