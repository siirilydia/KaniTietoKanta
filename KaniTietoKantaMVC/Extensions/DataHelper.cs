using KaniTietoKantaMVC.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KaniTietoKantaMVC.Controllers;
using System.Security.Claims;

namespace KaniTietoKantaMVC.Extensions
{
    public class DataHelper
    {
        //private readonly IConfiguration _configuration;

        //public DataHelper(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    var conn = _configuration["ConnectionStrings:KaniDBContext"];
        //}
        public static string url = "https://siirinkaniapi.azurewebsites.net/api/kanidb/";
        public static string apiKEY;
        public static string useremail;

        //public string UserEmail()
        //{
        //    return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        //}

        #region perus tietohaku
        public static List<Kani> HaeKaikkiKanit(string email)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/Valikko/{apiKEY}/{email}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            List<Kani> kaikki = JsonConvert.DeserializeObject<List<Kani>>(json);

            return kaikki;
        }

        public static List<Poikueet> HaeKaikkiPoikueet(string email)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/Poikueet/{apiKEY}/{email}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            List<Poikueet> kaikki = JsonConvert.DeserializeObject<List<Poikueet>>(json);

            return kaikki;
        }

        public static List<Astutukset> HaeKaikkiAstutukset(string email)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/Astutukset/{apiKEY}/{email}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            List<Astutukset> kaikki = JsonConvert.DeserializeObject<List<Astutukset>>(json);

            return kaikki;
        }

        public static Poikueet HaePoikueIdllä(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/Poikueentiedot/{id}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            Poikueet idhaettu = JsonConvert.DeserializeObject<Poikueet>(json);

            return idhaettu;
        }

        public static Kani HaeKaniIdllä(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/Kanintiedot/{id}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            Kani muokattava = JsonConvert.DeserializeObject<Kani>(json);

            return muokattava;
        }

        public static Astutukset HaeAstutusIdllä(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/Astutuksentiedot/{id}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            Astutukset muokattava = JsonConvert.DeserializeObject<Astutukset>(json);

            return muokattava;
        }

        public static List<Poikastiedot> HaePoikastenTiedot(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/PoikasTiedot/{id}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            List<Poikastiedot> poikastiedot = JsonConvert.DeserializeObject<List<Poikastiedot>>(json);

            return poikastiedot;
        }

        public static Poikastiedot HaePoikasenTiedot(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/PoikasenTiedot/{id}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            Poikastiedot poikasentiedot = JsonConvert.DeserializeObject<Poikastiedot>(json);

            return poikasentiedot;
        }

        public static Ostaja HaeOstajanTiedot(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"{url}/OstajanTiedot/{id}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            Ostaja ostaja = JsonConvert.DeserializeObject<Ostaja>(json);

            return ostaja;
        }

        #endregion

        #region uuden lisääminen
        public static string LisääUusiKani(Kani uusikani)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(uusikani);

            //string urllisää = $"{url}/UusiKani";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync($"{url}/UusiKani/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string LisääUusiPoikue(Poikueet uusipoikue, bool astutusvalittu = false, int astutusid = 0)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(uusipoikue);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync($"{url}/UusiPoikue/{apiKEY}/{astutusvalittu}/{astutusid}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string LisääUusiPoikanen(Poikastiedot uusipoikanen)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(uusipoikanen);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync($"{url}/UusiPoikanen/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string LisääUusiOstaja(Ostaja uusiostaja)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(uusiostaja);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync($"{url}/UusiOstaja/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string LisääUusiAstutus(Astutukset uusiastutus)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(uusiastutus);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync($"{url}/UusiAstutus/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }
        #endregion

        #region muokkaaminen
        public static string MuokkaaKania(Kani muokattu)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(muokattu);

            //string url = "https://localhost:44323/api/KaniDB/MuokkaaKania/" + muokattu.KaniId.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync($"{url}/MuokkaaKania/{muokattu.KaniId.ToString()}/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }
            return json;
        }

        public static string MuokkaaPoikuetta(Poikueet muokattu)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(muokattu);

            //string url = "https://localhost:44323/api/KaniDB/MuokkaaPoikuetta/" + muokattu.PoikueId.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync($"{url}/MuokkaaPoikuetta/{muokattu.PoikueId.ToString()}/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string MuokkaaPoikasta(Poikastiedot muokattu)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(muokattu);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync($"{url}/MuokkaaPoikasta/{muokattu.PoikanenId.ToString()}/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string MuokkaaOstajaa(Ostaja muokattu)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(muokattu);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync($"{url}/MuokkaaOstajaa/{muokattu.OstajaId.ToString()}/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }
        public static string MuokkaaAstutusta(Astutukset muokattu)
        {
            string json = "";

            string body = JsonConvert.SerializeObject(muokattu);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync($"{url}/MuokkaaAstutusta/{muokattu.AstutusId.ToString()}/{apiKEY}", content).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }
        #endregion

        #region poistaminen
        public static string PoistaKani(int id)
        {
            string json = "";

            //string url = "https://localhost:44323/api/KaniDB/PoistaKani/" + id.ToString();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.DeleteAsync($"{url}/PoistaKani/{id.ToString()}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string PoistaPoikanen(int id)
        {
            string json = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.DeleteAsync($"{url}/PoistaPoikanen/{id.ToString()}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string PoistaPoikue(int id)
        {
            string json = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.DeleteAsync($"{url}/PoistaPoikue/{id.ToString()}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string PoistaOstaja(int id)
        {
            string json = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.DeleteAsync($"{url}/PoistaOstaja/{id.ToString()}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public static string PoistaAstutus(int id)
        {
            string json = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.DeleteAsync($"{url}/PoistaAstutus/{id.ToString()}/{apiKEY}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        #endregion
    }
}
