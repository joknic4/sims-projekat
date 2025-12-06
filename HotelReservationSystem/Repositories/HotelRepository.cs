using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private List<Hotel> hoteli;
        private readonly string filePath;

        public HotelRepository(string filePath)
        {
            this.filePath = filePath;
            hoteli = new List<Hotel>();
            LoadFromFile();
        }

        public List<Hotel> GetAll()
        {
            return new List<Hotel>(hoteli);
        }

        public Hotel? GetBySifra(string sifra)
        {
            return hoteli.FirstOrDefault(h => h.GetSifra() == sifra);
        }

        public List<Hotel> GetByVlasnik(string jmbgVlasnika)
        {
            return hoteli.Where(h => h.GetJmbgVlasnika() == jmbgVlasnika).ToList();
        }

        public bool Add(Hotel hotel)
        {
            if (hotel == null)
                return false;

            if (GetBySifra(hotel.GetSifra()) != null)
                return false;

            hoteli.Add(hotel);
            SaveToFile();
            return true;
        }

        public bool Update(Hotel hotel)
        {
            if (hotel == null)
                return false;

            var existing = GetBySifra(hotel.GetSifra());
            if (existing == null)
                return false;

            hoteli.Remove(existing);
            hoteli.Add(hotel);
            SaveToFile();
            return true;
        }

        public bool Delete(string sifra)
        {
            var hotel = GetBySifra(sifra);
            if (hotel == null)
                return false;

            hoteli.Remove(hotel);
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

                var data = hoteli.Select(h => new
                {
                    Sifra = h.GetSifra(),
                    Ime = h.GetIme(),
                    GodinaIzgradnje = h.GetGodinaIzgradnje(),
                    BrojZvezdica = h.GetBrojZvezdica(),
                    JmbgVlasnika = h.GetJmbgVlasnika(),
                    Status = h.GetStatus().ToString()
                }).ToList();

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri čuvanju hotela: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    hoteli = new List<Hotel>();
                    return;
                }

                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeAnonymousType(json, new[]
                {
                    new
                    {
                        Sifra = "",
                        Ime = "",
                        GodinaIzgradnje = 0,
                        BrojZvezdica = 0,
                        JmbgVlasnika = "",
                        Status = ""
                    }
                });

                if (data != null)
                {
                    hoteli = data.Select(d => new Hotel(
                        d.Sifra,
                        d.Ime,
                        d.GodinaIzgradnje,
                        d.BrojZvezdica,
                        d.JmbgVlasnika,
                        Enum.Parse<StatusHotela>(d.Status)
                    )).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri učitavanju hotela: {ex.Message}");
                hoteli = new List<Hotel>();
            }
        }
    }
}
