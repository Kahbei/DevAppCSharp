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
            if (ModelState.IsValid)
            {
                JoueurService joueurService = new JoueurService(new EnglishBattle.data.EnglishBattleEntities());
                Joueur joueur = joueurService.GetJoueur(model.Email, model.Password);

                if (joueur != null)
                {
                    Session["joueur"] = joueur;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Nettoie le model
                    ModelState.Clear();
                }

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
    }
}