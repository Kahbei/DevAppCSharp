using EnglishBattle.data;
using EnglishBattle.data.Services;
using EnglishBattleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishBattleApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            //si les champs sont remplis
            if (ModelState.IsValid)
            {
                JoueurService joueurService = new JoueurService(new EnglishBattle.data.EnglishBattleEntities());
                Joueur joueur = joueurService.GetJoueur(model.Email, model.Password);

                //si l'email et le mdp correspondent
                if (joueur != null)
                {
                    Session["joueur"] = joueur;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Nettoie le model
                    ModelState.Clear();
                    ViewBag.erreurConnexion = "Erreur de connexion. Mail ou mot de passe erroné.";
                }

            }
            //si l'email et le mdp ne sont pas bons
            else
            {
                ViewBag.erreurConnexion = "Erreur de connexion. Vous devez remplir les champs.";
            }

            return View();
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            //si les champs sont remplis
            if (ModelState.IsValid)
            {
                JoueurService joueurService = new JoueurService(new EnglishBattle.data.EnglishBattleEntities());
                //VilleService villeService = new VilleService(new EnglishBattle.data.EnglishBattleEntities());
                //Ville ville = villeService.GetVilleItem(int.Parse(model.Ville));

                Joueur joueur = new Joueur
                {
                    nom = model.Nom,
                    prenom = model.Prenom,
                    email = model.Email,
                    motDePasse = model.Password,
                    niveau = int.Parse(model.Niveau),
                    idVille = int.Parse(model.Ville)
                };

                joueurService.Insert(joueur);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.erreurInscription = "Erreur d'inscription. Vous devez remplir tous les champs.";
            }

            // Si on arrive ici, les données du formulaire ne sont pas valides.
            return View();
        }
        //recup des villes
        [HttpPost]
        public ActionResult GVille(RegisterViewModel model)
        {
            VilleService villeService = new VilleService(new EnglishBattle.data.EnglishBattleEntities());
            List<Ville> ville = villeService.GetVilleList();

            return View();

        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}