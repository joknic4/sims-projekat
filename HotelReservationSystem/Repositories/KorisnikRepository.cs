using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly string filePath = "Data/korisnici.json";
        private List<Korisnik> korisnici;
        
        public KorisnikRepository()
        {
            korisnici = new List<Korisnik>();
            Load();
        }
        
        public List<Korisnik> GetAll()
        {
            return korisnici;
        }
        
        public Korisnik? GetByJmbg(string jmbg)
        {
            return korisnici.FirstOrDefault(k => k.GetJmbg() == jmbg);
        }
        
        public Korisnik? GetByEmail(string email)
        {
            return korisnici.FirstOrDefault(k => k.GetEmail() == email);
        }
        
        public void Add(Korisnik korisnik)
        {
            korisnici.Add(korisnik);
        }
        
        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "Data");
            string json = JsonConvert.SerializeObject(korisnici, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        
        private void Load()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json) ?? new List<Korisnik>();
            }
        }
    }
}
