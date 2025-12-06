using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public class RezervacijaRepository : IRezervacijaRepository
    {
        private List<Rezervacija> rezervacije;
        private readonly string filePath;

        public RezervacijaRepository(string filePath)
        {
            this.filePath = filePath;
            rezervacije = new List<Rezervacija>();
            LoadFromFile();
        }

        public List<Rezervacija> GetAll()
        {
            return new List<Rezervacija>(rezervacije);
        }

        public Rezervacija? GetById(string id)
        {
            return rezervacije.FirstOrDefault(r => r.GetId() == id);
        }

        public List<Rezervacija> GetByGost(string jmbgGosta)
        {
            return rezervacije.Where(r => r.GetJmbgGosta() == jmbgGosta).ToList();
        }

        public List<Rezervacija> GetByApartman(string idApartmana)
        {
            return rezervacije.Where(r => r.GetIdApartmana() == idApartmana).ToList();
        }

        public bool Add(Rezervacija rezervacija)
        {
            if (rezervacija == null)
                return false;

            if (GetById(rezervacija.GetId()) != null)
                return false;

            rezervacije.Add(rezervacija);
            SaveToFile();
            return true;
        }

        public bool Update(Rezervacija rezervacija)
        {
            if (rezervacija == null)
                return false;

            var existing = GetById(rezervacija.GetId());
            if (existing == null)
                return false;

            rezervacije.Remove(existing);
            rezervacije.Add(rezervacija);
            SaveToFile();
            return true;
        }

        public bool Delete(string id)
        {
            var rezervacija = GetById(id);
            if (rezervacija == null)
                return false;

            rezervacije.Remove(rezervacija);
            SaveToFile();
            return true;
        }

        public void SaveToFile()
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var data = rezervacije.Select(r => new
                {
                    Id = r.GetId(),
                    JmbgGosta = r.GetJmbgGosta(),
                    IdApartmana = r.GetIdApartmana(),
                    DatumOd = r.GetDatumOd().ToString("yyyy-MM-dd"),
                    DatumDo = r.GetDatumDo().ToString("yyyy-MM-dd"),
                    Status = r.GetStatus().ToString(),
                    RazlogOdbijanja = r.GetRazlogOdbijanja()
                }).ToList();

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri čuvanju rezervacija: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    rezervacije = new List<Rezervacija>();
                    return;
                }

                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeAnonymousType(json, new[]
                {
                    new
                    {
                        Id = "",
                        JmbgGosta = "",
                        IdApartmana = "",
                        DatumOd = "",
                        DatumDo = "",
                        Status = "",
                        RazlogOdbijanja = ""
                    }
                });

                if (data != null)
                {
                    rezervacije = data.Select(d => new Rezervacija(
                        d.Id,
                        d.JmbgGosta,
                        d.IdApartmana,
                        DateTime.Parse(d.DatumOd),
                        DateTime.Parse(d.DatumDo),
                        Enum.Parse<StatusRezervacije>(d.Status),
                        d.RazlogOdbijanja
                    )).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri učitavanju rezervacija: {ex.Message}");
                rezervacije = new List<Rezervacija>();
            }
        }
    }
}
