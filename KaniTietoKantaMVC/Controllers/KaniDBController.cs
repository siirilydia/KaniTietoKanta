using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KaniTietoKantaMVC.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using KaniTietoKantaMVC.Extensions;
using KaniTietoKantaMVC.ViewModels;
using System.Security.Claims;
//using System.Text.Json;

namespace KaniTietoKantaMVC.Controllers
{
    public class KaniDBController : Controller
    {
        public static string errormsg;
        public static bool astutusvalittu = false;
        public static int astutusid = 0;

        public string UserEmail()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
        //public List<Kani> HaeKanitEmaililla(List<Kani> kaikki)
        //{
        //    kaikki = (from k in kaikki
        //              where k.Email == UserEmail()
        //              select k).ToList();

        //    return kaikki;
        //}

        //public List<Poikueet> HaePoikueetEmaililla(List<Poikueet> kaikki)
        //{
        //    kaikki = (from k in kaikki
        //              where k.Email == UserEmail()
        //              select k).ToList();

        //    return kaikki;
        //}

        // GET: KaniDB/Error

        public ActionResult Error()
        {
            ViewBag.Message = errormsg;

            return View();
        }

        // GET: KaniDB
        public ActionResult Index()
        {
            return View();
        }


        #region perus tietohaku

        // GET: KaniDB/Sukupuu/5
        public ActionResult Sukupuu(int kaniid = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }


            List<Kani> kaikkikanit = DataHelper.HaeKaikkiKanit(UserEmail());
            List<Poikueet> kaikkipoikueet = DataHelper.HaeKaikkiPoikueet(UserEmail());

            ViewBag.Kaikkikanit = kaikkikanit;

            if (kaniid == 0)
            {
                ViewBag.Haku = false;
                return View();
            }
            ViewBag.Haku = true;


            Kani haettu = DataHelper.HaeKaniIdllä(kaniid);

            if (haettu == null || haettu.Email != UserEmail())
            {
                errormsg = "Tätä kania ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            //Kani haettu = (from k in kaikkikanit
            //               where k.KaniId == id
            //               select k).FirstOrDefault();



            ViewBag.Haettu = haettu;

            EmäIsä e = new EmäIsä();
            EmäIsä ee = new EmäIsä();
            EmäIsä ei = new EmäIsä();
            EmäIsä i = new EmäIsä();
            EmäIsä ie = new EmäIsä();
            EmäIsä ii = new EmäIsä();

            if (haettu.PoikueenId != null)
            {
                Poikueet eka = new Poikueet();

                eka = (from ekapoikue in kaikkipoikueet
                       where ekapoikue.PoikueId == haettu.PoikueenId
                       select ekapoikue).FirstOrDefault();

                //haetaan emä

                if (eka.EmäId != null)
                {
                    e.KaniId = eka.EmäId;
                }
                if (eka.EmäVnimi != null)
                {
                    e.Vnimi = eka.EmäVnimi;
                }
                e.Knimi = eka.EmäKnimi;

                //haetaan emän kanitiedot, jos löytyy
                if (e.KaniId != null)
                {
                    Kani eKani = (from k in kaikkikanit
                                   where k.KaniId == e.KaniId
                                   select k).FirstOrDefault();

                    //jos emällä on poikue liitettynä, haetaan sen tiedot
                    if (eKani != null && eKani.PoikueenId != null)
                    {
                        ViewBag.EmänPoikue = true;

                        Poikueet emänpoikue = (from p in kaikkipoikueet
                                               where p.PoikueId == eKani.PoikueenId
                                               select p).FirstOrDefault();

                        //haetaan poikueest emän emän tiedot

                        if (emänpoikue.EmäId != null)
                        {
                            ee.KaniId = emänpoikue.EmäId;
                        }
                        if (emänpoikue.EmäVnimi != null)
                        {
                            ee.Vnimi = emänpoikue.EmäVnimi;
                        }
                        ee.Knimi = emänpoikue.EmäKnimi;

                        //haetaan poikueesta emän isän tiedot

                        if (emänpoikue.IsäId != null)
                        {
                            ei.KaniId = emänpoikue.IsäId;
                        }
                        if (emänpoikue.IsäVnimi != null)
                        {
                            ei.Vnimi = emänpoikue.IsäVnimi;
                        }
                        ei.Knimi = emänpoikue.IsäKnimi;
                    }
                    else
                    {
                        ee.Knimi = eKani.EmänNimi;
                        ei.Knimi = eKani.IsänNimi;
                        ViewBag.EmänPoikue = false;
                    }
                }

                //haetaan isä
                if (eka.IsäId != null)
                {
                    i.KaniId = eka.IsäId;
                }
                if (eka.IsäVnimi != null)
                {
                    i.Vnimi = eka.IsäVnimi;
                }
                i.Knimi = eka.IsäKnimi;

                //haetaan isän kanitiedot, jos löytyy
                if (i.KaniId != null)
                {
                    Kani iKani = (from k in kaikkikanit
                                   where k.KaniId == i.KaniId
                                   select k).FirstOrDefault();

                    //jos isällä on poikue liitettynä, haetaan sen tiedot
                    if (iKani != null && iKani.PoikueenId != null)
                    {
                        Poikueet isänpoikue = (from p in kaikkipoikueet
                                               where p.PoikueId == iKani.PoikueenId
                                               select p).FirstOrDefault();

                        //haetaan poikueest isän emän tiedot

                        if (isänpoikue.EmäId != null)
                        {
                            ie.KaniId = isänpoikue.EmäId;
                        }
                        if (isänpoikue.EmäVnimi != null)
                        {
                            ie.Vnimi = isänpoikue.EmäVnimi;
                        }
                        ie.Knimi = isänpoikue.EmäKnimi;

                        //haetaan poikueesta isän isän tiedot

                        if (isänpoikue.IsäId != null)
                        {
                            ii.KaniId = isänpoikue.IsäId;
                        }
                        if (isänpoikue.IsäVnimi != null)
                        {
                            ii.Vnimi = isänpoikue.IsäVnimi;
                        }
                        ii.Knimi = isänpoikue.IsäKnimi;
                    }
                    else
                    {
                        ie.Knimi = iKani.EmänNimi;
                        ii.Knimi = iKani.IsänNimi;
                        ViewBag.IsänPoikue = false;
                    }

                }
            }
            else
            {
                e.Knimi = haettu.EmänNimi;
                i.Knimi = haettu.IsänNimi;
                ViewBag.EkanPoikue = false;
                ViewBag.EmänPoikue = false;
                ViewBag.IsänPoikue = false;
            }


            ViewBag.E = e;
            ViewBag.Ee = ee;
            ViewBag.Ei = ei;
            ViewBag.I = i;
            ViewBag.Ie = ie;
            ViewBag.Ii = ii;

            return View();
        }

        // GET: KaniDB/Rokotukset
        public ActionResult Rokotukset()
        {
            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            List<Kani> voimassa = (from k in kaikki
                                   where k.Kuollut == false &&
                                   k.Myyty == false &&
                                   (DateTime.Now.Date - Convert.ToDateTime(k.Rokotuspv)).Days < 365
                                   orderby k.Rokotuspv ascending
                                   select k).ToList();

            List<Kani> vanhentuneet = (from k in kaikki
                                       where k.Rokotuspv != null &&
                                       k.Kuollut == false &&
                                       k.Myyty == false &&
                                       (DateTime.Now.Date - Convert.ToDateTime(k.Rokotuspv)).Days > 365
                                       orderby k.Rokotuspv ascending
                                       select k).ToList();

            List<Kani> eirokotettu = (from k in kaikki
                                      where k.Rokotettu == false
                                      && k.Kuollut == false &&
                                      k.Myyty == false
                                      orderby k.Syntymäpäivä descending
                                      select k).ToList();

            if (voimassa.Count != 0)
            {
                ViewBag.Voimassa = voimassa;
            }
            if (vanhentuneet.Count != 0)
            {
                ViewBag.Vanhentuneet = vanhentuneet;
            }
            if (eirokotettu.Count != 0)
            {
                ViewBag.EiRokotettu = eirokotettu;
            }

            return View();
        }

        // GET: KaniDB/Details/5
        public ActionResult Details(int id)
        {
            Kani idhaettu = DataHelper.HaeKaniIdllä(id);
            if (idhaettu == null || idhaettu.Email != UserEmail())
            {
                errormsg = "Tätä kania ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }
            return View(idhaettu);
        }

        // GET: KaniDB/Poikuetiedot/5
        public ActionResult Poikuetiedot(int id)
        {
            Poikueet idhaettu = DataHelper.HaePoikueIdllä(id);
            if (idhaettu == null || idhaettu.Email != UserEmail())
            {
                errormsg = "Tätä poikuetta ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            List<Poikastiedot> poikaset = DataHelper.HaePoikastenTiedot(id);
            //ViewBag.PoikasTiedot = poikaset;

            List<Ostaja> ostajat = new List<Ostaja>();

            foreach (var item in poikaset)
            {
                if (item.OstajaId != null)
                {
                    ostajat.Add(DataHelper.HaeOstajanTiedot(item.OstajaId.GetValueOrDefault()));
                }
            }

            PoikueTietoNäkymä ptn = new PoikueTietoNäkymä();
            ptn.poikue = idhaettu;
            ptn.pt = poikaset;
            ptn.ostajat = ostajat;

            return View(ptn);
        }

        public ActionResult Astutuksentiedot(int id)
        {
            Astutukset idhaettu = DataHelper.HaeAstutusIdllä(id);


            if (idhaettu == null || idhaettu.Email != UserEmail())
            {
                errormsg = "Tämän astutuksen tietoja ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            return View(idhaettu);
        }

        // GET: KaniDB/Kaikki
        public ActionResult Kaikki(string järjestys = null, string nykhaku = null, string hakusana = null, bool kuolleet = false, bool myydyt = false, int sivunro = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            ViewBag.NykJärjestys = järjestys;

            ViewBag.VNimiJärjestys = järjestys == "vnimi_asc" ? "vnimi_desc" : "vnimi_asc";
            ViewBag.KNimiJärjestys = järjestys == "knimi_asc" ? "knimi_desc" : "knimi_asc";
            ViewBag.RotuJärjestys = järjestys == "rotu_asc" ? "rotu_desc" : "rotu_asc";
            ViewBag.VäriJärjestys = järjestys == "väri_asc" ? "väri_desc" : "väri_asc";
            ViewBag.SkpJärjestys = järjestys == "skp_asc" ? "skp_desc" : "skp_asc";
            ViewBag.SpvJärjestys = järjestys == "spv_asc" ? "spv_desc" : "spv_asc";
            ViewBag.KpvJärjestys = järjestys == "kpv_asc" ? "kpv_desc" : "kpv_asc";
            //ViewBag.Kuolleet = 

            if (hakusana != null)
            {
                sivunro = 0;
            }
            else
            {
                hakusana = nykhaku;
            }
            ViewBag.HakuSana = hakusana;
            ViewBag.Kuolleet = kuolleet;
            ViewBag.Myydyt = myydyt;


            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            if (!String.IsNullOrEmpty(hakusana))
            {
                kaikki = kaikki.Where(s => s.Nimi != null && s.Nimi.ToLower().Contains(hakusana.ToLower())
                                       || s.Kutsumanimi != null && s.Kutsumanimi.ToLower().Contains(hakusana.ToLower())
                                       || s.Rotu != null && s.Rotu.ToLower().Contains(hakusana.ToLower())
                                       || s.Sukupuoli != null && s.Sukupuoli.ToLower().Contains(hakusana.ToLower())
                                       || s.Tietoja != null && s.Tietoja.ToLower().Contains(hakusana.ToLower())
                                       || s.Väri != null && s.Väri.ToLower().Contains(hakusana.ToLower())).ToList();

            }

            if (kuolleet == false && myydyt == true)
            {
                kaikki = kaikki.Where(s => s.Kuollut == false || s.Myyty == true).ToList();
            }
            else if (kuolleet == false)
            {
                kaikki = kaikki.Where(s => s.Kuollut == false).ToList();
            }
            if (myydyt == false && kuolleet == true)
            {
                kaikki = kaikki.Where(s => s.Myyty == false || s.Kuollut == true).ToList();
            }
            else if (myydyt == false)
            {
                kaikki = kaikki.Where(s => s.Myyty == false).ToList();
            }

            switch (järjestys)
            {
                case "vnimi_asc":
                    kaikki = kaikki.OrderBy(s => s.Nimi).ToList();
                    break;
                case "vnimi_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Nimi).ToList();
                    break;
                case "knimi_asc":
                    kaikki = kaikki.OrderBy(s => s.Kutsumanimi).ToList();
                    break;
                case "knimi_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Kutsumanimi).ToList();
                    break;
                case "spv_asc":
                    kaikki = kaikki.OrderBy(s => s.Syntymäpäivä).ToList();
                    break;
                case "spv_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Syntymäpäivä).ToList();
                    break;
                case "kpv_asc":
                    kaikki = kaikki.OrderBy(s => s.Kuolinpäivä).ToList();
                    break;
                case "kpv_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Kuolinpäivä).ToList();
                    break;
                case "rotu_asc":
                    kaikki = kaikki.OrderBy(s => s.Rotu).ToList();
                    break;
                case "rotu_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Rotu).ToList();
                    break;
                case "väri_asc":
                    kaikki = kaikki.OrderBy(s => s.Väri).ToList();
                    break;
                case "väri_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Väri).ToList();
                    break;
                case "skp_asc":
                    kaikki = kaikki.OrderBy(s => s.Sukupuoli).ToList();
                    break;
                case "skp_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Sukupuoli).ToList();
                    break;
                default:
                    kaikki = kaikki.OrderBy(s => s.Syntymäpäivä).ToList();
                    break;
            }

            int tuloksiasivulla = 10;
            int sivumäärä = kaikki.Count() / tuloksiasivulla;

            Math.DivRem(kaikki.Count, tuloksiasivulla, out int jakojäännös);

            if (jakojäännös > 0) sivumäärä++;

            int skippaa = sivunro * tuloksiasivulla;

            if (skippaa >= kaikki.Count - tuloksiasivulla)
            {
                skippaa = kaikki.Count - tuloksiasivulla;
            }

            if (sivunro < 0) sivunro = 1;

            ViewBag.Sivunro = sivunro;

            ViewBag.Sivumäärä = sivumäärä;

            if (sivunro == sivumäärä - 1 && jakojäännös != 0)
            {
                skippaa += tuloksiasivulla - jakojäännös;
                tuloksiasivulla = jakojäännös;
            }

            kaikki = kaikki.Skip(skippaa).Take<Kani>(tuloksiasivulla).ToList();

            ViewBag.Kaikkikanit = kaikki;

            return View(kaikki);
        }

        // GET: KaniDB/Poikueet
        public ActionResult Poikueet(string järjestys = null, string nykhaku = null, string hakusana = null, int sivunro = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            ViewBag.NykJärjestys = järjestys;

            ViewBag.SpvJärjestys = järjestys == "spv_asc" ? "spv_desc" : "spv_asc";
            ViewBag.RotuJärjestys = järjestys == "rotu_asc" ? "rotu_desc" : "rotu_asc";
            ViewBag.LkmJärjestys = järjestys == "lkm_asc" ? "lkm_desc" : "lkm_asc";
            ViewBag.ENimiJärjestys = järjestys == "enimi_asc" ? "enimi_desc" : "enimi_asc";
            ViewBag.INimiJärjestys = järjestys == "inimi_asc" ? "inimi_desc" : "inimi_asc";

            if (hakusana != null)
            {
                sivunro = 0;
            }
            else
            {
                hakusana = nykhaku;
            }
            ViewBag.HakuSana = hakusana;

            List<Poikueet> kaikki = DataHelper.HaeKaikkiPoikueet(UserEmail());


            if (!String.IsNullOrEmpty(hakusana))
            {
                kaikki = kaikki.Where(s => s.EmäVnimi != null && s.EmäVnimi.ToLower().Contains(hakusana.ToLower())
                                       || s.IsäVnimi != null && s.IsäVnimi.ToLower().Contains(hakusana.ToLower())
                                       || s.Rotu != null && s.Rotu.ToLower().Contains(hakusana.ToLower())).ToList();
            }

            switch (järjestys)
            {
                case "spv_asc":
                    kaikki = kaikki.OrderBy(s => s.Syntymäpv).ToList();
                    break;
                case "spv_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Syntymäpv).ToList();
                    break;
                case "rotu_asc":
                    kaikki = kaikki.OrderBy(s => s.Rotu).ToList();
                    break;
                case "rotu_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Rotu).ToList();
                    break;
                case "lkm_asc":
                    kaikki = kaikki.OrderBy(s => s.Poikaslkm).ToList();
                    break;
                case "lkm_desc":
                    kaikki = kaikki.OrderByDescending(s => s.Poikaslkm).ToList();
                    break;
                case "enimi_asc":
                    kaikki = kaikki.OrderBy(s => s.EmäVnimi).ToList();
                    break;
                case "enimi_desc":
                    kaikki = kaikki.OrderByDescending(s => s.EmäVnimi).ToList();
                    break;
                case "inimi_asc":
                    kaikki = kaikki.OrderBy(s => s.IsäVnimi).ToList();
                    break;
                case "inimi_desc":
                    kaikki = kaikki.OrderByDescending(s => s.IsäVnimi).ToList();
                    break;
                default:
                    kaikki = kaikki.OrderBy(s => s.Syntymäpv).ToList();
                    break;
            }

            int tuloksiasivulla = 5;
            int sivumäärä = kaikki.Count() / tuloksiasivulla;

            Math.DivRem(kaikki.Count, tuloksiasivulla, out int jakojäännös);

            if (jakojäännös > 0) sivumäärä++;

            int skippaa = sivunro * tuloksiasivulla;

            if (skippaa >= kaikki.Count - tuloksiasivulla)
            {
                skippaa = kaikki.Count - tuloksiasivulla;
            }

            if (sivunro < 0) sivunro = 1;

            ViewBag.Sivunro = sivunro;

            ViewBag.Sivumäärä = sivumäärä;

            if (sivunro == sivumäärä - 1 && jakojäännös != 0)
            {
                skippaa += tuloksiasivulla - jakojäännös;
                tuloksiasivulla = jakojäännös;
            }

            kaikki = kaikki.Skip(skippaa).Take<Poikueet>(tuloksiasivulla).ToList();

            ViewBag.Kaikkikanit = kaikki;

            return View(kaikki);
        }

        // GET: KaniDB/Astutukset
        public ActionResult Astutukset()
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            List<Astutukset> kaikki = DataHelper.HaeKaikkiAstutukset(UserEmail());


            //if (idhaettu == null || idhaettu.Email != UserEmail())
            //{
            //    errormsg = "Tätä kania ei löydy tietokannastasi.";
            //    return RedirectToAction("Error");
            //}
            return View(kaikki);
        }

        #endregion

        #region uuden lisääminen

        // GET: KaniDB/UusiKani
        public ActionResult UusiKani(int id = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            List<Poikueet> kaikki = DataHelper.HaeKaikkiPoikueet(UserEmail());
            ViewBag.Poikueet = kaikki;
            ViewBag.Email = UserEmail();

            bool poikanenvalittu = false;

            if (id != 0)
            {
                poikanenvalittu = true;
                Poikastiedot pt = DataHelper.HaePoikasenTiedot(id);
                ViewBag.PoikasTiedot = pt;
                ViewBag.ValittuPoikue = (from k in kaikki
                                         where k.PoikueId == pt.PoikueId
                                         select k).FirstOrDefault();
            }
            ViewBag.PoikanenValittu = poikanenvalittu;

            return View();
        }

        [HttpPost]
        public ActionResult UusiKani(Kani uusikani)
        {
            if (ModelState.IsValid || uusikani.PoikueenId != 0)
            {
                string json = DataHelper.LisääUusiKani(uusikani);

                if (json == "400")
                {
                    errormsg = "Kanin lisääminen tietokantaan epäonnistui.";
                    return RedirectToAction("Error");
                }
                return RedirectToAction("Kaikki", "KaniDB");
            }

            List<Poikueet> kaikki = DataHelper.HaeKaikkiPoikueet(UserEmail());
            ViewBag.Poikueet = kaikki;
            ViewBag.Email = UserEmail();
            //ViewBag.PoikanenValittu = poikanenvalittu;

            return View(uusikani);
        }

        // GET: KaniDB/UusiPoikue
        public ActionResult UusiPoikue(int id = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            List<Kani> naaraat = (from n in kaikki
                                  where n.Sukupuoli == "naaras"
                                  select n).ToList();

            List<Kani> urokset = (from u in kaikki
                                  where u.Sukupuoli == "uros"
                                  select u).ToList();

            ViewBag.Naaraat = naaraat;
            ViewBag.Urokset = urokset;
            ViewBag.Email = UserEmail();

            ViewBag.Njson = JsonConvert.SerializeObject(naaraat);
            ViewBag.Ujson = JsonConvert.SerializeObject(naaraat);

            //astutusvalittu = false;

            if (id != 0)
            {
                astutusvalittu = true;
                astutusid = id;
                Astutukset astutus = DataHelper.HaeAstutusIdllä(id);
                ViewBag.Astutus = astutus;
            }

            ViewBag.AstutusValittu = astutusvalittu;

            return View();
        }

        [HttpPost]
        public ActionResult UusiPoikue(Poikueet uusipoikue)
        {
            if (ModelState.IsValid)
            {
                string json = null;

                if (astutusvalittu)
                {
                    json = DataHelper.LisääUusiPoikue(uusipoikue, true, astutusid);
                    astutusvalittu = false;
                    astutusid = 0;
                }
                else
                {
                    json = DataHelper.LisääUusiPoikue(uusipoikue, false, 0);
                }

                if (json == "400")
                {
                    errormsg = "Poikueen lisääminen tietokantaan epäonnistui.";
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Poikueet", "KaniDB");
            }

            return View(uusipoikue);
        }

        // GET: KaniDB/UusiPoikanen/5
        public ActionResult UusiPoikanen(int poikueid)
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            Poikueet poikue = DataHelper.HaePoikueIdllä(poikueid);

            ViewBag.Poikue = poikue;
            ViewBag.Email = UserEmail();

            return View();
        }

        [HttpPost]
        public ActionResult UusiPoikanen(Poikastiedot uusipoikanen)
        {
            if (ModelState.IsValid)
            {
                string json = DataHelper.LisääUusiPoikanen(uusipoikanen);

                if (json == "400")
                {
                    errormsg = "Poikasen lisääminen tietokantaan epäonnistui.";
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Poikuetiedot", "KaniDB", new { id = uusipoikanen.PoikueId });
            }

            Poikueet poikue = DataHelper.HaePoikueIdllä(uusipoikanen.PoikueId);

            ViewBag.Poikue = poikue;
            ViewBag.Email = UserEmail();

            return View(uusipoikanen);

        }

        // GET: KaniDB/UusiOstaja/5
        public ActionResult UusiOstaja(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            //Poikueet poikue = DataHelper.HaePoikueIdllä(poikueid);

            ViewBag.PoikasenId = id;
            ViewBag.Email = UserEmail();
            ViewBag.PoikueId = DataHelper.HaePoikasenTiedot(id).PoikueId;

            return View();
        }

        [HttpPost]
        public ActionResult UusiOstaja(Ostaja uusiostaja)
        {
            if (ModelState.IsValid)
            {
                string json = DataHelper.LisääUusiOstaja(uusiostaja);

                if (json == "400")
                {
                    errormsg = "Ostajan tietojen lisääminen tietokantaan epäonnistui.";
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Poikuetiedot", "KaniDB", new { id = DataHelper.HaePoikasenTiedot(uusiostaja.PoikanenId).PoikueId });
            }


            return View(uusiostaja);
        }

        // GET: KaniDB/UusiAstutus
        public ActionResult UusiAstutus()
        {
            if (!User.Identity.IsAuthenticated)
            {
                errormsg = "Kirjaudu ensin sisään käyttääksesi tätä ominaisuutta.";
                return RedirectToAction("Error");
            }

            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            List<Kani> naaraat = (from n in kaikki
                                  where n.Sukupuoli == "naaras" && n.Kuollut == false
                                  select n).ToList();

            List<Kani> urokset = (from u in kaikki
                                  where u.Sukupuoli == "uros" && u.Kuollut == false
                                  select u).ToList();

            ViewBag.Naaraat = naaraat;
            ViewBag.Urokset = urokset;
            ViewBag.Email = UserEmail();

            return View();
        }

        [HttpPost]
        public ActionResult UusiAstutus(Astutukset uusiastutus)
        {
            if (ModelState.IsValid)
            {
                string json = DataHelper.LisääUusiAstutus(uusiastutus);

                if (json == "400")
                {
                    errormsg = "Astutustietojen lisääminen tietokantaan epäonnistui.";
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Astutukset", "KaniDB");
            }
            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            List<Kani> naaraat = (from n in kaikki
                                  where n.Sukupuoli == "naaras" && n.Kuolinpäivä == null
                                  select n).ToList();

            List<Kani> urokset = (from u in kaikki
                                  where u.Sukupuoli == "uros" && u.Kuolinpäivä == null
                                  select u).ToList();

            ViewBag.Naaraat = naaraat;
            ViewBag.Urokset = urokset;
            ViewBag.Email = UserEmail();

            return View(uusiastutus);
        }

        #endregion

        #region muokkaaminen

        // GET: KaniDB/MuokkaaKania/5
        public IActionResult MuokkaaKania(int id)
        {
            Kani muokattava = DataHelper.HaeKaniIdllä(id);

            if (muokattava == null || muokattava.Email != UserEmail())
            {
                errormsg = "Tätä kania ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            List<Poikueet> kaikki = DataHelper.HaeKaikkiPoikueet(UserEmail());
            ViewBag.Poikueet = kaikki;


            return View(muokattava);
        }

        // GET: KaniDB/MuokkaaKania/5
        [HttpPost]
        public ActionResult MuokkaaKania(Kani muokattu)
        {
            if (ModelState.IsValid || muokattu.PoikueenId != 0)
            {
                string json = DataHelper.MuokkaaKania(muokattu);

                if (json == "400")
                {
                    errormsg = "Kanin tietojen muokkaaminen epäonnistui.";
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Details", "KaniDB", new { id = muokattu.KaniId });
            }

            return View(muokattu);
        }

        // GET: KaniDB/MuokkaaPoikuetta/5
        public IActionResult MuokkaaPoikuetta(int id)
        {
            Poikueet muokattava = DataHelper.HaePoikueIdllä(id);

            if (muokattava == null || muokattava.Email != UserEmail())
            {
                errormsg = "Tätä poikuetta ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            List<Kani> naaraat = (from n in kaikki
                                  where n.Sukupuoli == "naaras"
                                  select n).ToList();
            List<Kani> urokset = (from u in kaikki
                                  where u.Sukupuoli == "uros"
                                  select u).ToList();

            ViewBag.Naaraat = naaraat;
            ViewBag.Urokset = urokset;


            return View(muokattava);
        }

        // GET: KaniDB/MuokkaaPoikuetta/5
        [HttpPost]
        public ActionResult MuokkaaPoikuetta(Poikueet muokattu)
        {
            if (ModelState.IsValid)
            {
                string json = DataHelper.MuokkaaPoikuetta(muokattu);

                if (json == "400")
                {
                    errormsg = "Poikueen tietojen muokkaaminen epäonnistui.";
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Poikuetiedot", "KaniDB", new { id = muokattu.PoikueId });
            }

            return View(muokattu);
        }

        // GET: KaniDB/MuokkaaPoikasta/5
        public IActionResult MuokkaaPoikasta(int id)
        {
            Poikastiedot muokattava = DataHelper.HaePoikasenTiedot(id);

            if (muokattava == null || muokattava.Email != UserEmail())
            {
                errormsg = "Tämän poikasen tietoja ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            return View(muokattava);
        }

        // GET: KaniDB/MuokkaaPoikasta/5
        [HttpPost]
        public ActionResult MuokkaaPoikasta(Poikastiedot muokattu)
        {
            string json = DataHelper.MuokkaaPoikasta(muokattu);

            if (json == "400")
            {
                errormsg = "Poikasen tietojen muokkaaminen epäonnistui.";
                return RedirectToAction("Error");
            }

            return RedirectToAction("Poikuetiedot", "KaniDB", new { id = muokattu.PoikueId });
        }


        // GET: KaniDB/MuokkaaOstajaa/5
        public IActionResult MuokkaaOstajaa(int id)
        {
            Ostaja muokattava = DataHelper.HaeOstajanTiedot(id);

            if (muokattava == null || muokattava.Email != UserEmail())
            {
                errormsg = "Tätä ostajaa ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            ViewBag.PoikueId = DataHelper.HaePoikasenTiedot(muokattava.PoikanenId).PoikueId;

            return View(muokattava);
        }

        // GET: KaniDB/MuokkaaOstajaa/5
        [HttpPost]
        public ActionResult MuokkaaOstajaa(Ostaja muokattu)
        {
            if (ModelState.IsValid)
            {
                string json = DataHelper.MuokkaaOstajaa(muokattu);

                if (json == "400")
                {
                    errormsg = "Ostajan tietojen muokkaaminen epäonnistui.";
                    return RedirectToAction("Error");
                }
                return RedirectToAction("Poikuetiedot", "KaniDB", new { id = DataHelper.HaePoikasenTiedot(muokattu.PoikanenId).PoikueId });
            }

            ViewBag.PoikueId = DataHelper.HaePoikasenTiedot(muokattu.PoikanenId).PoikueId;

            return View(muokattu);
        }

        // GET: KaniDB/MuokkaaAstutusta/5
        public IActionResult MuokkaaAstutusta(int id)
        {
            Astutukset muokattava = DataHelper.HaeAstutusIdllä(id);

            if (muokattava == null || muokattava.Email != UserEmail())
            {
                errormsg = "Tätä astutustietoa ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            List<Kani> naaraat = (from n in kaikki
                                  where n.Sukupuoli == "naaras" && n.Kuollut == false
                                  select n).ToList();
            List<Kani> urokset = (from u in kaikki
                                  where u.Sukupuoli == "uros" && u.Kuollut == false
                                  select u).ToList();

            ViewBag.Naaraat = naaraat;
            ViewBag.Urokset = urokset;


            return View(muokattava);
        }

        // GET: KaniDB/MuokkaaKania/5
        [HttpPost]
        public ActionResult MuokkaaAstutusta(Astutukset muokattu)
        {
            if (ModelState.IsValid)
            {
                string json = DataHelper.MuokkaaAstutusta(muokattu);

                if (json == "400")
                {
                    errormsg = "Astutustietojen muokkaaminen epäonnistui.";
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Astutuksentiedot", "KaniDB", new { id = muokattu.AstutusId });
            }

            List<Kani> kaikki = DataHelper.HaeKaikkiKanit(UserEmail());

            List<Kani> naaraat = (from n in kaikki
                                  where n.Sukupuoli == "naaras"
                                  select n).ToList();
            List<Kani> urokset = (from u in kaikki
                                  where u.Sukupuoli == "uros"
                                  select u).ToList();

            ViewBag.Naaraat = naaraat;
            ViewBag.Urokset = urokset;

            return View(muokattu);
        }

        #endregion

        #region poistaminen

        // GET: KaniDB/PoistaKani/5
        public ActionResult PoistaKani(int id)
        {
            if (DataHelper.HaeKaniIdllä(id).Email != UserEmail() || DataHelper.HaeKaniIdllä(id) == null)
            {
                errormsg = "Tätä kania ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            string json = DataHelper.PoistaKani(id);

            if (json == "400")
            {
                errormsg = "Kanin poistaminen tietokannasta epäonnistui.";
                return RedirectToAction("Error");
            }

            return RedirectToAction("Kaikki", "KaniDB");

        }

        // GET: KaniDB/PoistaPoikue/5
        public ActionResult PoistaPoikue(int id)
        {
            if (DataHelper.HaePoikueIdllä(id).Email != UserEmail() || DataHelper.HaePoikueIdllä(id) == null)
            {
                errormsg = "Tätä poikuetta ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            string json = DataHelper.PoistaPoikue(id);

            if (json == "400")
            {
                errormsg = "Poikueen poistaminen tietokannasta epäonnistui.";
                return RedirectToAction("Error");
            }

            return RedirectToAction("Poikueet", "KaniDB");

        }

        // GET: KaniDB/PoistaPoikanen/5
        public ActionResult PoistaPoikanen(int id)
        {
            Poikastiedot poistettava = DataHelper.HaePoikasenTiedot(id);
            if (poistettava.Email != UserEmail() || poistettava == null)
            {
                errormsg = "Tätä poikasta ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            string json = DataHelper.PoistaPoikanen(id);

            if (json == "400")
            {
                errormsg = "Poikasen poistaminen tietokannasta epäonnistui.";
                return RedirectToAction("Error");
            }

            return RedirectToAction("Poikuetiedot", "KaniDB", new { id = poistettava.PoikueId });

        }

        // GET: KaniDB/PoistaOstaja/5
        public ActionResult PoistaOstaja(int id)
        {
            Ostaja poistettava = DataHelper.HaeOstajanTiedot(id);

            if (poistettava.Email != UserEmail() || poistettava == null)
            {
                errormsg = "Tätä ostajaa ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            string json = DataHelper.PoistaOstaja(id);

            if (json == "400")
            {
                errormsg = "Ostajan poistaminen tietokannasta epäonnistui.";
                return RedirectToAction("Error");
            }

            return RedirectToAction("Poikuetiedot", "KaniDB", new { id = DataHelper.HaePoikasenTiedot(poistettava.PoikanenId).PoikueId });
        }

        // GET: KaniDB/PoistaAstutus/5
        public ActionResult PoistaAstutus(int id)
        {
            Astutukset poistettava = DataHelper.HaeAstutusIdllä(id);

            if (poistettava.Email != UserEmail() || poistettava == null)
            {
                errormsg = "Tämän astutuksen tietoja ei löydy tietokannastasi.";
                return RedirectToAction("Error");
            }

            string json = DataHelper.PoistaAstutus(id);

            if (json == "400")
            {
                errormsg = "Astutustietojen poistaminen tietokannasta epäonnistui.";
                return RedirectToAction("Error");
            }

            return RedirectToAction("Astutukset", "KaniDB");
        }

        #endregion

    }
}