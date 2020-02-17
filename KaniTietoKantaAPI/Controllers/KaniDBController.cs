using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KaniTietoKantaAPI.Models;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

//using Microsoft.AspNetCore.Cors;

namespace KaniTietoKantaAPI.Controllers
{
    //[EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class KaniDBController : ControllerBase
    {
        public static string APIKey;

        //private readonly IConfiguration _configuration;

        //public KaniDBController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    var conn = _configuration["ConnectionStrings:KaniDBContext"];
        //}

        private readonly KanitContext _context;

        public KaniDBController(KanitContext context)
        {
            _context = context;
            //This is my solution in getting the APIKey, since I did not know other ways to do it. It works, tho.
            APIKey = context.GetAPIKey();
        }

        #region perus tietohaku

        // GET: api/KaniDB/Valikko
        [HttpGet("Valikko/{key}/{email}", Name = "Valikko")]
        public List<Kani> Valikko(string key, string email)
        {
            if (key != APIKey)
            {
                return null;
            }

            List<Kani> kaikki = (from k in _context.Kani
                                 where k.Email == email
                                 select k).ToList();

            return kaikki;
        }

        [HttpGet("{key}")]
        public List<Kani> Get(string key)
        {
            if (key != APIKey)
            {
                return null;
            }

            List<Kani> kaikkiKanit = (from k in _context.Kani
                                      select k).ToList();
            return kaikkiKanit;
        }

        // GET: api/KaniDB/Poikueet
        [HttpGet("Poikueet/{key}/{email}", Name = "Poikueet")]
        public List<Poikueet> Poikueet(string key, string email)
        {
            if (key != APIKey)
            {
                return null;
            }

            List<Poikueet> kaikkiPoikueet = (from p in _context.Poikueet
                                             where p.Email == email
                                             select p).ToList();
            return kaikkiPoikueet;
        }

        // GET: api/KaniDB/Astutukset
        [HttpGet("Astutukset/{key}/{email}", Name = "Astutukset")]
        public List<Astutukset> Astutukset(string key, string email)
        {
            if (key != APIKey)
            {
                return null;
            }

            List<Astutukset> kaikki = (from a in _context.Astutukset
                                       where a.Email == email
                                       select a).ToList();
            return kaikki;
        }

        // GET: api/KaniDB/Kanintiedot/5
        [HttpGet("Kanintiedot/{id}/{key}", Name = "Kanintiedot")]
        public Kani Kanintiedot(int id, string key)
        {
            if (key != APIKey)
            {
                return null;
            }

            Kani idhaettu = (from k in _context.Kani
                             where k.KaniId == id
                             select k).FirstOrDefault();
            return idhaettu;
        }

        // GET: api/KaniDB/Poikueentiedot/5
        [HttpGet("Poikueentiedot/{id}/{key}", Name = "Poikueentiedot")]
        public Poikueet Poikueentiedot(int id, string key)
        {
            if (key != APIKey)
            {
                return null;
            }

            Poikueet idhaettu = (from p in _context.Poikueet
                                 where p.PoikueId == id
                                 select p).FirstOrDefault();
            return idhaettu;
        }

        // GET: api/KaniDB/PoikasTiedot/5
        [HttpGet("PoikasTiedot/{id}/{key}", Name = "Poikastiedot")]
        public List<Poikastiedot> PoikasTiedot(int id, string key)
        {
            if (key != APIKey)
            {
                return null;
            }

            List<Poikastiedot> idhaettu = (from p in _context.Poikastiedot
                                           where p.PoikueId == id
                                           select p).ToList();
            return idhaettu;
        }

        // GET: api/KaniDB/PoikasTiedot/5
        [HttpGet("PoikasenTiedot/{id}/{key}", Name = "PoikasenTiedot")]
        public Poikastiedot PoikasenTiedot(int id, string key)
        {
            if (key != APIKey)
            {
                return null;
            }

            Poikastiedot idhaettu = (from p in _context.Poikastiedot
                                     where p.PoikanenId == id
                                     select p).FirstOrDefault();
            return idhaettu;
        }

        // GET: api/KaniDB/OstajanTiedot/5
        [HttpGet("OstajanTiedot/{id}/{key}", Name = "OstajanTiedot")]
        public Ostaja OstajanTiedot(int id, string key)
        {
            if (key != APIKey)
            {
                return null;
            }

            Ostaja idhaettu = (from p in _context.Ostaja
                               where p.OstajaId == id
                               select p).FirstOrDefault();
            return idhaettu;
        }

        // GET: api/KaniDB/AstutuksenTiedot/5
        [HttpGet("AstutuksenTiedot/{id}/{key}", Name = "AstutuksenTiedot")]
        public Astutukset AstutuksenTiedot(int id, string key)
        {
            if (key != APIKey)
            {
                return null;
            }

            Astutukset idhaettu = (from a in _context.Astutukset
                                   where a.AstutusId == id
                                   select a).FirstOrDefault();
            return idhaettu;
        }
        #endregion

        #region uuden lisääminen

        // POST: api/KaniDB/UusiKani
        [HttpPost("UusiKani/{key}", Name = "UusiKani")]
        public HttpStatusCode UusiKani([FromBody] Kani uusikani, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            if (uusikani.Kuollut == false)
            {
                uusikani.Kuolinpäivä = null;
            }

            Poikastiedot pt = new Poikastiedot();
            bool poikanenLöytyi = false;

            //uusituote.TuoteId = db.Tuote.Max(x => x.TuoteId) + 1;
            if (uusikani.PoikueenId != 0)
            {
                Poikueet valittu = (from p in _context.Poikueet
                                    where p.PoikueId == uusikani.PoikueenId
                                    select p).FirstOrDefault();

                uusikani.Syntymäpäivä = valittu.Syntymäpv;
                uusikani.Rotu = valittu.Rotu;
                //uusikani.Poikueen = valittu;

                _context.Kani.Add(uusikani);

                List<Poikastiedot> ptlist = (from pkst in _context.Poikastiedot
                                             where pkst.PoikueId == uusikani.PoikueenId
                                             select pkst).ToList();

                pt = (from poikanen in ptlist
                      where poikanen.Nimi == uusikani.Nimi
                      select poikanen).FirstOrDefault();

                if (pt != null)
                {
                    poikanenLöytyi = true;
                    //pt.KaniId = uusikani.KaniId;
                    pt.Kani = uusikani;
                    pt.Väri = uusikani.Väri;
                    pt.Sukupuoli = uusikani.Sukupuoli;
                }

                else
                {
                    pt = new Poikastiedot();

                    pt.PoikueId = valittu.PoikueId;
                    pt.Syntymäpv = uusikani.Syntymäpäivä;
                    pt.Nimi = uusikani.Nimi;
                    pt.Väri = uusikani.Väri;
                    pt.Sukupuoli = uusikani.Sukupuoli;
                    pt.Tietoja = uusikani.Tietoja;
                    pt.Email = uusikani.Email;
                    pt.Kani = uusikani;
                    pt.Poikue = valittu;
                }
            }
            else
            {
                uusikani.PoikueenId = null;
                _context.Kani.Add(uusikani);
            }

            //_context.Kani.Add(uusikani);

            if (uusikani.PoikueenId != null)
            {
                pt.KaniId = (from p in _context.Kani
                             where p == uusikani
                             select p.KaniId).FirstOrDefault();

                if (!poikanenLöytyi)
                {
                    _context.Poikastiedot.Add(pt);
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (HttpStatusCode.BadRequest);
            }

            if (uusikani.Sukupuoli == "naaras")
            {
                List<Poikueet> poikueet = (from p in _context.Poikueet
                                           where p.EmäVnimi == uusikani.Nimi && p.Email == uusikani.Email
                                           select p).ToList();

                if (poikueet != null)
                {
                    foreach (var item in poikueet)
                    {
                        item.EmäVnimi = uusikani.Nimi;
                        item.EmäKnimi = uusikani.Kutsumanimi;
                        Kani emä = (from k in _context.Kani
                                    where k == uusikani
                                    select k).FirstOrDefault();

                        if (emä != null)
                        {
                            item.EmäId = emä.KaniId;
                        }
                    }
                }
            }
            else
            {
                List<Poikueet> poikueet = (from p in _context.Poikueet
                                           where p.IsäVnimi == uusikani.Nimi && p.Email == uusikani.Email
                                           select p).ToList();

                if (poikueet != null)
                {
                    foreach (var item in poikueet)
                    {
                        item.IsäVnimi = uusikani.Nimi;
                        item.IsäKnimi = uusikani.Kutsumanimi;
                        Kani isä = (from k in _context.Kani
                                    where k == uusikani
                                    select k).FirstOrDefault();

                        if (isä != null)
                        {
                            item.IsäId = isä.KaniId;
                        }
                    }
                }
            }



            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (HttpStatusCode.BadRequest);
            }

            return (HttpStatusCode.OK);
        }

        // POST: api/KaniDB/UusiPoikue
        [HttpPost("UusiPoikue/{key}/{astutusvalittu}/{astutusid}", Name = "UusiPoikue")]
        public HttpStatusCode UusiPoikue([FromBody] Poikueet uusipoikue, string key, bool astutusvalittu, int astutusid)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            if (uusipoikue.EmäId != 0)
            {
                Kani emä = (from e in _context.Kani
                            where e.KaniId == uusipoikue.EmäId
                            select e).FirstOrDefault();

                //uusipoikue.EmäI

                uusipoikue.EmäVnimi = emä.Nimi;
                uusipoikue.EmäKnimi = emä.Kutsumanimi;
                uusipoikue.EmäVk = emä.Vk;
            }
            else
            {
                uusipoikue.EmäId = null;
            }
            if (uusipoikue.IsäId != 0)
            {
                Kani isä = (from i in _context.Kani
                            where i.KaniId == uusipoikue.IsäId
                            select i).FirstOrDefault();

                //uusipoikue.Isä = isä;

                uusipoikue.IsäVnimi = isä.Nimi;
                uusipoikue.IsäKnimi = isä.Kutsumanimi;
                uusipoikue.IsäVk = isä.Vk;
            }
            else
            {
                uusipoikue.IsäId = null;
            }
            if (astutusvalittu)
            {
                Astutukset poistettava = (from a in _context.Astutukset
                                          where a.AstutusId == astutusid
                                          select a).FirstOrDefault();

                _context.Astutukset.Remove(poistettava);
            }
            try
            {
                _context.Poikueet.Add(uusipoikue);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (HttpStatusCode.BadRequest);
            }

            return (HttpStatusCode.OK);
        }

        // POST: api/KaniDB/UusiPoikanen
        [HttpPost("UusiPoikanen/{key}", Name = "UusiPoikanen")]
        public HttpStatusCode UusiPoikanen([FromBody] Poikastiedot uusipoikanen, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            uusipoikanen.Syntymäpv = (from e in _context.Poikueet
                                      where e.PoikueId == uusipoikanen.PoikueId
                                      select e.Syntymäpv).FirstOrDefault();

            _context.Poikastiedot.Add(uusipoikanen);

            Kani linkitetty = (from k in _context.Kani
                               where k.Nimi == uusipoikanen.Nimi && k.Email == uusipoikanen.Email
                               select k).FirstOrDefault();

            if (linkitetty != null)
            {
                linkitetty.PoikueenId = uusipoikanen.PoikueId;
                linkitetty.Syntymäpäivä = uusipoikanen.Syntymäpv;
                linkitetty.Väri = uusipoikanen.Väri;
                linkitetty.Rotu = (from p in _context.Poikueet
                                   where p.PoikueId == uusipoikanen.PoikueId
                                   select p.Rotu).FirstOrDefault();
                linkitetty.Sukupuoli = uusipoikanen.Sukupuoli;
                linkitetty.EmänNimi = null;
                linkitetty.IsänNimi = null;
                uusipoikanen.KaniId = linkitetty.KaniId;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (HttpStatusCode.BadRequest);
            }

            return (HttpStatusCode.OK);
        }

        // POST: api/KaniDB/UusiOstaja
        [HttpPost("UusiOstaja/{key}", Name = "UusiOstaja")]
        public HttpStatusCode UusiOstaja([FromBody] Ostaja uusiostaja, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            _context.Ostaja.Add(uusiostaja);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (HttpStatusCode.BadRequest);
            }

            Poikastiedot p = (from pt in _context.Poikastiedot
                              where pt.PoikanenId == uusiostaja.PoikanenId
                              select pt).FirstOrDefault();

            p.OstajaId = (from o in _context.Ostaja
                          where o == uusiostaja
                          select o.OstajaId).FirstOrDefault();

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (HttpStatusCode.BadRequest);
            }

            return (HttpStatusCode.OK);
        }

        // POST: api/KaniDB/UusiAstutus
        [HttpPost("UusiAstutus/{key}", Name = "UusiAstutus")]
        public HttpStatusCode UusiAstutus([FromBody] Astutukset uusiastutus, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            if (uusiastutus.EmäId != 0)
            {
                Kani emä = (from k in _context.Kani
                            where k.KaniId == uusiastutus.EmäId
                            select k).FirstOrDefault();

                uusiastutus.EmäVnimi = emä.Nimi;
                uusiastutus.EmäKnimi = emä.Kutsumanimi;
                uusiastutus.EmäRotu = emä.Rotu;
                uusiastutus.EmäSyntymäpv = emä.Syntymäpäivä;
                uusiastutus.EmäVäri = emä.Väri;
            }
            else
            {
                uusiastutus.EmäId = null;
            }
            if (uusiastutus.IsäId != 0)
            {
                Kani isä = (from k in _context.Kani
                            where k.KaniId == uusiastutus.IsäId
                            select k).FirstOrDefault();

                uusiastutus.IsäVnimi = isä.Nimi;
                uusiastutus.IsäKnimi = isä.Kutsumanimi;
                uusiastutus.IsäRotu = isä.Rotu;
                uusiastutus.IsäSyntymäpv = isä.Syntymäpäivä;
                uusiastutus.IsäVäri = isä.Väri;
            }
            else
            {
                uusiastutus.IsäId = null;
            }

            _context.Astutukset.Add(uusiastutus);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (HttpStatusCode.BadRequest);
            }

            //Poikastiedot p = (from pt in _context.Poikastiedot
            //                  where pt.PoikanenId == uusiostaja.PoikanenId
            //                  select pt).FirstOrDefault();

            //p.OstajaId = (from o in _context.Ostaja
            //              where o == uusiostaja
            //              select o.OstajaId).FirstOrDefault();

            //try
            //{
            //    _context.SaveChanges();
            //}
            //catch (Exception)
            //{
            //    return (HttpStatusCode.BadRequest);
            //}

            return (HttpStatusCode.OK);
        }

        #endregion

        #region muokkaaminen

        // PUT: api/KaniDB/MuokkaaKania/5
        [HttpPut("MuokkaaKania/{id}/{key}")]
        public HttpStatusCode MuokkaaKania(int id, [FromBody] Kani muokattu, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            if (muokattu.PoikueenId != 0)
            {
                Poikueet valittu = (from p in _context.Poikueet
                                    where p.PoikueId == muokattu.PoikueenId
                                    select p).FirstOrDefault();

                muokattu.Syntymäpäivä = valittu.Syntymäpv;
                muokattu.Rotu = valittu.Rotu;
                muokattu.EmänNimi = null;
                muokattu.IsänNimi = null;
            }
            else
            {
                muokattu.PoikueenId = null;
            }
            try
            {
                Kani muokattava = (from k in _context.Kani
                                   where k.KaniId == muokattu.KaniId
                                   select k).FirstOrDefault();

                muokattava.KaniId = muokattu.KaniId;
                muokattava.Nimi = muokattu.Nimi;
                muokattava.Kutsumanimi = muokattu.Kutsumanimi;
                muokattava.Syntymäpäivä = muokattu.Syntymäpäivä;
                muokattava.Kuollut = muokattu.Kuollut;
                if (muokattu.Kuollut == true)
                {
                    muokattava.Kuolinpäivä = muokattu.Kuolinpäivä;
                }
                else
                {
                    muokattava.Kuolinpäivä = null;
                }
                muokattava.Väri = muokattu.Väri;
                muokattava.Ok = muokattu.Ok;
                muokattava.Vk = muokattu.Vk;
                muokattava.Sukupuoli = muokattu.Sukupuoli;
                muokattava.Rokotettu = muokattu.Rokotettu;
                muokattava.Rokotuspv = muokattu.Rokotuspv;
                muokattava.PoikueenId = muokattu.PoikueenId;
                muokattava.Tietoja = muokattu.Tietoja;
                muokattava.Rotu = muokattu.Rotu;
                muokattava.Myyty = muokattu.Myyty;
                muokattava.OstajanTiedot = muokattu.OstajanTiedot;
                muokattava.KasvattajanTiedot = muokattu.KasvattajanTiedot;
                muokattava.IsänNimi = muokattu.IsänNimi;
                muokattava.EmänNimi = muokattu.EmänNimi;

                Poikastiedot pt = new Poikastiedot();

                if (muokattu.PoikueenId != 0 && muokattu.PoikueenId != null)
                {
                    List<Poikastiedot> ptlist = (from pkst in _context.Poikastiedot
                                                 where pkst.PoikueId == muokattava.PoikueenId
                                                 select pkst).ToList();

                    pt = (from poikanen in ptlist
                          where poikanen.Nimi == muokattava.Nimi
                          select poikanen).FirstOrDefault();

                    if (pt != null)
                    {
                        pt.KaniId = muokattava.KaniId;
                        pt.Väri = muokattava.Väri;
                        pt.Sukupuoli = muokattava.Sukupuoli;
                        pt.Nimi = muokattava.Nimi;
                    }

                    else
                    {
                        Poikastiedot vanhat = (from tiedot in _context.Poikastiedot
                                               where tiedot.KaniId == muokattava.KaniId
                                               select tiedot).FirstOrDefault();

                        if (vanhat != null)
                        {
                            vanhat.KaniId = muokattava.KaniId;
                            vanhat.Väri = muokattava.Väri;
                            vanhat.Sukupuoli = muokattava.Sukupuoli;
                            vanhat.Nimi = muokattava.Nimi;
                        }

                        else
                        {
                            pt = new Poikastiedot();

                            pt.PoikueId = (from p in _context.Poikueet
                                           where p.PoikueId == muokattava.PoikueenId
                                           select p.PoikueId).FirstOrDefault();
                            pt.KaniId = muokattava.KaniId;
                            pt.Syntymäpv = muokattava.Syntymäpäivä;
                            pt.Nimi = muokattava.Nimi;
                            pt.Väri = muokattava.Väri;
                            pt.Sukupuoli = muokattava.Sukupuoli;
                            pt.Email = muokattava.Email;

                            _context.Poikastiedot.Add(pt);
                        }
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            if (muokattu.Sukupuoli == "naaras")
            {
                List<Poikueet> poikueet = (from p in _context.Poikueet
                                           where p.EmäId == muokattu.KaniId || p.IsäId == muokattu.KaniId
                                           select p).ToList();

                foreach (var item in poikueet)
                {
                    if (item.IsäId == muokattu.KaniId)
                    {
                        item.IsäId = null;
                        item.IsäVk = null;
                    }
                    else
                    {
                        item.EmäVnimi = muokattu.Nimi;
                        item.EmäKnimi = muokattu.Kutsumanimi;
                        item.EmäVk = muokattu.Vk;
                    }
                }

                List<Astutukset> astutukset = (from a in _context.Astutukset
                                               where a.EmäId == muokattu.KaniId || a.IsäId == muokattu.KaniId
                                               select a).ToList();

                foreach (var item in astutukset)
                {
                    if (item.IsäId == muokattu.KaniId)
                    {
                        item.IsäId = null;
                    }
                    else
                    {
                        item.EmäVnimi = muokattu.Nimi;
                        item.EmäKnimi = muokattu.Kutsumanimi;
                        item.EmäRotu = muokattu.Rotu;
                        item.EmäSyntymäpv = muokattu.Syntymäpäivä;
                        item.EmäVäri = muokattu.Väri;
                    }
                }
            }
            else if (muokattu.Sukupuoli == "uros")
            {
                List<Poikueet> poikueet = (from p in _context.Poikueet
                                           where p.IsäId == muokattu.KaniId || p.EmäId == muokattu.KaniId
                                           select p).ToList();

                foreach (var item in poikueet)
                {
                    if (item.EmäId == muokattu.KaniId)
                    {
                        item.EmäId = null;
                        item.EmäVk = null;
                    }
                    else
                    {
                        item.IsäVnimi = muokattu.Nimi;
                        item.IsäKnimi = muokattu.Kutsumanimi;
                        item.IsäVk = muokattu.Vk;
                    }
                }

                List<Astutukset> astutukset = (from a in _context.Astutukset
                                               where a.IsäId == muokattu.KaniId || a.EmäId == muokattu.KaniId
                                               select a).ToList();

                foreach (var item in astutukset)
                {
                    if (item.EmäId == muokattu.KaniId)
                    {
                        item.EmäId = null;
                    }
                    else
                    {
                        item.IsäVnimi = muokattu.Nimi;
                        item.IsäKnimi = muokattu.Kutsumanimi;
                        item.IsäRotu = muokattu.Rotu;
                        item.IsäSyntymäpv = muokattu.Syntymäpäivä;
                        item.IsäVäri = muokattu.Väri;
                    }
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        // PUT: api/KaniDB/MuokkaaKania/5
        [HttpPut("MuokkaaPoikuetta/{id}/{key}")]
        public HttpStatusCode MuokkaaPoikuetta(int id, [FromBody] Poikueet muokattu, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            if (muokattu.EmäId != 0)
            {
                Kani emä = (from e in _context.Kani
                            where e.KaniId == muokattu.EmäId
                            select e).FirstOrDefault();

                //muokattu.Emä = emä;
                if (emä.Nimi != null)
                {
                    muokattu.EmäVnimi = emä.Nimi;
                }
                else
                {
                    muokattu.EmäVnimi = null;
                }

                muokattu.EmäKnimi = emä.Kutsumanimi;
                muokattu.EmäVk = emä.Vk;
            }
            else
            {
                muokattu.EmäId = null;
            }

            if (muokattu.IsäId != 0)
            {
                Kani isä = (from i in _context.Kani
                            where i.KaniId == muokattu.IsäId
                            select i).FirstOrDefault();

                //muokattu.Isä = isä;
                if (isä.Nimi != null)
                {
                    muokattu.IsäVnimi = isä.Nimi;
                }
                else
                {
                    muokattu.IsäVnimi = null;
                }
                muokattu.IsäKnimi = isä.Kutsumanimi;
                muokattu.IsäVk = isä.Vk;
            }
            else
            {
                muokattu.IsäId = null;
            }

            Poikueet muokattava = (from p in _context.Poikueet
                                   where p.PoikueId == id
                                   select p).FirstOrDefault();

            muokattava.PoikueId = muokattu.PoikueId;
            muokattava.EmäId = muokattu.EmäId;
            muokattava.EmäVnimi = muokattu.EmäVnimi;
            muokattava.EmäKnimi = muokattu.EmäKnimi;
            muokattava.EmäVk = muokattu.EmäVk;
            muokattava.IsäId = muokattu.IsäId;
            muokattava.IsäVnimi = muokattu.IsäVnimi;
            muokattava.IsäKnimi = muokattu.IsäKnimi;
            muokattava.IsäVk = muokattu.IsäVk;
            muokattava.Poikaslkm = muokattu.Poikaslkm;
            muokattava.Syntymäpv = muokattu.Syntymäpv;
            muokattava.Rotu = muokattu.Rotu;
            muokattava.Tietoja = muokattu.Tietoja;

            List<Poikastiedot> linkitetyt = (from pt in _context.Poikastiedot
                                             where pt.PoikueId == id
                                             select pt).ToList();

            foreach (var item in linkitetyt)
            {
                item.Syntymäpv = muokattava.Syntymäpv;

                Kani linkitetty = (from k in _context.Kani
                                   where k.KaniId == item.KaniId
                                   select k).FirstOrDefault();

                if (linkitetty != null)
                {
                    linkitetty.Syntymäpäivä = item.Syntymäpv;
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        // PUT: api/KaniDB/MuokkaaKania/5
        [HttpPut("MuokkaaPoikasta/{id}/{key}")]
        public HttpStatusCode MuokkaaPoikasta(int id, [FromBody] Poikastiedot muokattu, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            Poikastiedot muokattava = (from p in _context.Poikastiedot
                                       where p.PoikanenId == id
                                       select p).FirstOrDefault();

            //muokattava.PoikanenId = muokattu.PoikanenId;
            //muokattava.PoikueId = muokattu.PoikueId;
            //muokattava.KaniId = muokattu.KaniId;
            muokattava.Nimi = muokattu.Nimi;
            //muokattava.Syntymäpv = muokattu.Syntymäpv;
            muokattava.Väri = muokattu.Väri;
            muokattava.Sukupuoli = muokattu.Sukupuoli;
            muokattava.Tietoja = muokattu.Tietoja;
            //muokattava.OstajaId = muokattu.OstajaId;
            //muokattava.Email = muokattu.Email;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            Kani linkitetty = (from k in _context.Kani
                               where k.KaniId == muokattava.KaniId
                               select k).FirstOrDefault();

            if (linkitetty != null)
            {
                linkitetty.Nimi = muokattava.Nimi;
                linkitetty.Väri = muokattava.Väri;
                linkitetty.Sukupuoli = muokattava.Sukupuoli;

                List<Astutukset> linkitetyt = (from a in _context.Astutukset
                                               where a.EmäId == linkitetty.KaniId || a.IsäId == linkitetty.KaniId
                                               select a).ToList();

                foreach (var item in linkitetyt)
                {
                    if (muokattava.Sukupuoli == "naaras")
                    {
                        if (item.IsäId == linkitetty.KaniId)
                        {
                            item.IsäId = null;
                        }
                        else
                        {
                            item.EmäVnimi = muokattava.Nimi;
                            item.EmäVäri = muokattava.Väri;
                        }
                    }
                    else if (muokattava.Sukupuoli == "uros")
                    {
                        if (item.EmäId == linkitetty.KaniId)
                        {
                            item.EmäId = null;
                        }
                        else
                        {
                            item.IsäVnimi = muokattava.Nimi;
                            item.IsäVäri = muokattava.Väri;
                        }
                    }
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        // PUT: api/KaniDB/MuokkaaOstajaa/5
        [HttpPut("MuokkaaOstajaa/{id}/{key}", Name = "MuokkaaOstajaa")]
        public HttpStatusCode MuokkaaOstajaa(int id, [FromBody] Ostaja muokattu, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            Ostaja muokattava = (from o in _context.Ostaja
                                 where o.OstajaId == id
                                 select o).FirstOrDefault();

            muokattava.Nimi = muokattu.Nimi;
            muokattava.Puhnro = muokattu.Puhnro;
            muokattava.OEmail = muokattu.OEmail;
            muokattava.Ostopäivä = muokattu.Ostopäivä;
            muokattava.Hinta = muokattu.Hinta;
            muokattava.Maksettu = muokattu.Maksettu;
            muokattava.Tietoja = muokattu.Tietoja;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }


        // PUT: api/KaniDB/MuokkaaAstutusta/5
        [HttpPut("MuokkaaAstutusta/{id}/{key}", Name = "MuokkaaAstutusta")]
        public HttpStatusCode MuokkaaAstutusta(int id, [FromBody] Astutukset muokattu, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            if (muokattu.EmäId != 0)
            {
                Kani emä = (from e in _context.Kani
                            where e.KaniId == muokattu.EmäId
                            select e).FirstOrDefault();

                if (emä.Nimi != null)
                {
                    muokattu.EmäVnimi = emä.Nimi;
                }
                else
                {
                    muokattu.EmäVnimi = null;
                }

                muokattu.EmäKnimi = emä.Kutsumanimi;
                muokattu.EmäSyntymäpv = emä.Syntymäpäivä;
                muokattu.EmäVäri = emä.Väri;
                muokattu.EmäRotu = emä.Rotu;
            }
            else
            {
                muokattu.EmäId = null;
            }

            if (muokattu.IsäId != 0)
            {
                Kani isä = (from i in _context.Kani
                            where i.KaniId == muokattu.IsäId
                            select i).FirstOrDefault();

                if (isä.Nimi != null)
                {
                    muokattu.IsäVnimi = isä.Nimi;
                }
                else
                {
                    muokattu.IsäVnimi = null;
                }

                muokattu.IsäKnimi = isä.Kutsumanimi;
                muokattu.IsäSyntymäpv = isä.Syntymäpäivä;
                muokattu.IsäVäri = isä.Väri;
                muokattu.IsäRotu = isä.Rotu;
            }
            else
            {
                muokattu.IsäId = null;
            }


            Astutukset muokattava = (from a in _context.Astutukset
                                     where a.AstutusId == id
                                     select a).FirstOrDefault();

            muokattava.Astutuspäivä = muokattu.Astutuspäivä;

            muokattava.EmäId = muokattu.EmäId;
            muokattava.EmäVnimi = muokattu.EmäVnimi;
            muokattava.EmäKnimi = muokattu.EmäKnimi;
            muokattava.EmäVäri = muokattu.EmäVäri;
            muokattava.EmäRotu = muokattu.EmäRotu;
            muokattava.EmäSyntymäpv = muokattu.EmäSyntymäpv;

            muokattava.IsäId = muokattu.IsäId;
            muokattava.IsäVnimi = muokattu.IsäVnimi;
            muokattava.IsäKnimi = muokattu.IsäKnimi;
            muokattava.IsäVäri = muokattu.IsäVäri;
            muokattava.IsäRotu = muokattu.IsäRotu;
            muokattava.IsäSyntymäpv = muokattu.IsäSyntymäpv;

            muokattava.Lisätietoja = muokattu.Lisätietoja;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        #endregion

        #region poistaminen

        // DELETE: api/KaniDB/PoistaKani/5
        [HttpDelete("PoistaKani/{id}/{key}", Name = "PoistaKani")]
        public HttpStatusCode PoistaKani(int id, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            Kani poistettava = (from k in _context.Kani
                                where k.KaniId == id
                                select k).FirstOrDefault();

            Poikastiedot pt = (from p in _context.Poikastiedot
                               where p.KaniId == id
                               select p).FirstOrDefault();

            if (pt != null)
            {
                pt.KaniId = null;
            }


            if (poistettava.Sukupuoli == "naaras")
            {
                List<Poikueet> poikueet = (from p in _context.Poikueet
                                           where p.EmäId == id
                                           select p).ToList();

                foreach (var item in poikueet)
                {
                    item.EmäId = null;
                }
            }
            else
            {
                List<Poikueet> poikueet = (from p in _context.Poikueet
                                           where p.IsäId == id
                                           select p).ToList();

                foreach (var item in poikueet)
                {
                    item.IsäId = null;
                }
            }

            _context.Kani.Remove(poistettava);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        // DELETE: api/KaniDB/PoistaPoikanen/5
        [HttpDelete("PoistaPoikanen/{id}/{key}", Name = "PoistaPoikanen")]
        public HttpStatusCode PoistaPoikanen(int id, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }


            Poikastiedot poistettava = (from pt in _context.Poikastiedot
                                        where pt.PoikanenId == id
                                        select pt).FirstOrDefault();

            Kani linkitetty = (from k in _context.Kani
                               where k.KaniId == poistettava.KaniId
                               select k).FirstOrDefault();

            if (linkitetty != null)
            {
                linkitetty.PoikueenId = null;
            }

            if (poistettava.OstajaId != null)
            {
                Ostaja ostajalinkitetty = (from o in _context.Ostaja
                                           where o.OstajaId == poistettava.OstajaId
                                           select o).FirstOrDefault();

                _context.Ostaja.Remove(ostajalinkitetty);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            _context.Poikastiedot.Remove(poistettava);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        // DELETE: api/KaniDB/PoistaPoikue/5
        [HttpDelete("PoistaPoikue/{id}/{key}", Name = "PoistaPoikue")]
        public HttpStatusCode PoistaPoikue(int id, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            List<Poikastiedot> poistettavat = (from pt in _context.Poikastiedot
                                               where pt.PoikueId == id
                                               select pt).ToList();

            foreach (var item in poistettavat)
            {

                //PoistaPoikanen(item.PoikanenId, key);

                Ostaja ostaja = (from o in _context.Ostaja
                                 where o.OstajaId == item.OstajaId
                                 select o).FirstOrDefault();

                if (ostaja != null)
                {
                    _context.Ostaja.Remove(ostaja);

                    _context.SaveChanges();
                }

                item.KaniId = null;

                _context.SaveChanges();

                _context.Poikastiedot.Remove(item);
            }

            _context.SaveChanges();

            List<Kani> kanit = (from k in _context.Kani
                                where k.PoikueenId == id
                                select k).ToList();

            foreach (var item in kanit)
            {
                item.PoikueenId = null;
            }

            _context.SaveChanges();

            Poikueet poistettava = (from p in _context.Poikueet
                                    where p.PoikueId == id
                                    select p).FirstOrDefault();

            _context.Poikueet.Remove(poistettava);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        // DELETE: api/KaniDB/PoistaOstaja/5
        [HttpDelete("PoistaOstaja/{id}/{key}", Name = "PoistaOstaja")]
        public HttpStatusCode PoistaOstaja(int id, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }


            Ostaja poistettava = (from o in _context.Ostaja
                                  where o.OstajaId == id
                                  select o).FirstOrDefault();


            Poikastiedot linkitetty = (from pt in _context.Poikastiedot
                                       where pt.OstajaId == id
                                       select pt).FirstOrDefault();

            linkitetty.OstajaId = null;

            _context.Ostaja.Remove(poistettava);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        // DELETE: api/KaniDB/PoistaAstutus/5
        [HttpDelete("PoistaAstutus/{id}/{key}", Name = "PoistaAstutus")]
        public HttpStatusCode PoistaAstutus(int id, string key)
        {
            if (key != APIKey)
            {
                return (HttpStatusCode.BadRequest);
            }

            Astutukset poistettava = (from a in _context.Astutukset
                                      where a.AstutusId == id
                                      select a).FirstOrDefault();

            _context.Astutukset.Remove(poistettava);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.OK;
        }

        #endregion
    }
}
