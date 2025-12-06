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
        private List<Korisnik> korisnici;
        private readonly string filePath;

        public KorisnikRepository(string filePath)
        {
            this.filePath = filePath;
            korisnici = new List<Korisnik>();
            LoadFromFile();
        }

        public List<Korisnik> GetAll()
        {
            return new List<Korisnik>(korisnici);
        }

        public Korisnik? GetByJmbg(string jmbg)
        {
            return korisnici.FirstOrDefault(k => k.GetJmbg() == jmbg);
        }

        public Korisnik? GetByEmail(string email)
        {
            return korisnici.FirstOrDefault(k => k.GetEmail().Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public bool Add(Korisnik korisnik)
        {
            if (korisnik == null)
                return false;

            if (GetByJmbg(korisnik.GetJmbg()) != null)
                return false;

            if (GetByEmail(korisnik.GetEmail()) != null)
                return false;

            korisnici.Add(korisnik);
            SaveToFile();
            return true;
        }

        public bool Update(Korisnik korisnik)
        {
            if (korisnik == null)
                return false;

            var existing = GetByJmbg(korisnik.GetJmbg());
            if (existing == null)
                return false;

            korisnici.Remove(existing);
            korisnici.Add(korisnik);
            SaveToFile();
            return true;
        }

        public bool Delete(string jmbg)
        {
            var korisnik = GetByJmbg(jmbg);
            if (korisnik == null)
                return false;

            korisnici.Remove(korisnik);
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

                var data = korisnici.Select(k => new
                {
                    Jmbg = k.GetJmbg(),
                    Email = k.GetEmail(),
                    Lozinka = k.GetLozinka(),
                    Ime = k.GetIme(),
                    Prezime = k.GetPrezime(),
                    MobilniTelefon = k.GetMobilniTelefon(),
                    TipKorisnika = k.GetTipKorisnika().ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri čuvanju korisnika: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    korisnici = new List<Korisnik>();
                    return;
                }

                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeAnonymousType(json, new[]
                {
                    new
                    {
                        Jmbg = "",
                        Email = "",
                        Lozinka = "",
                        Ime = "",
                        Prezime = "",
                        MobilniTelefon = "",
                        TipKorisnika = ""
                    }
                });

                if (data != null)
                {
                    korisnici = data.Select(d => new Korisnik(
                        d.Jmbg,
                        d.Email,
                        d.Lozinka,
                        d.Ime,
                        d.Prezime,
                        d.MobilniTelefon,
                        Enum.Parse<KorisnikTip>(d.TipKorisnika)
                    )).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri učitavanju korisnika: {ex.Message}");
                korisnici = new List<Korisnik>();
            }
        }
    }
}
