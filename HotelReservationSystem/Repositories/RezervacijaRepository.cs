using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public class RezervacijaRepository : IRezervacijaRepository
    {
        private readonly string filePath = "Data/rezervacije.json";
        private List<Rezervacija> rezervacije;
        
        public RezervacijaRepository()
        {
            rezervacije = new List<Rezervacija>();
            Load();
        }
        
        public List<Rezervacija> GetAll()
        {
            return rezervacije;
        }
        
        public List<Rezervacija> GetByGost(string jmbgGosta)
        {
            return rezervacije.Where(r => r.GetJmbgGosta() == jmbgGosta).ToList();
        }
        
        public List<Rezervacija> GetByStatus(StatusRezervacije status)
        {
            var rezultat = new List<Rezervacija>();
            
            foreach (var rez in rezervacije)
            {
                if (rez.GetStatus() == status)
                {
                    rezultat.Add(rez);
                }
            }
            
            return rezultat.ToList();
        }
        
        public Rezervacija? GetById(string id)
        {
            return rezervacije.FirstOrDefault(r => r.GetId() == id);
        }
        
        public void Add(Rezervacija rezervacija)
        {
            rezervacije.Add(rezervacija);
        }
        
        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "Data");
            string json = JsonConvert.SerializeObject(rezervacije, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        
        private void Load()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                rezervacije = JsonConvert.DeserializeObject<List<Rezervacija>>(json) ?? new List<Rezervacija>();
            }
        }
    }
}
