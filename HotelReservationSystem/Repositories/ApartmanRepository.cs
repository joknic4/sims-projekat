using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public class ApartmanRepository : IApartmanRepository
    {
        private List<Apartman> apartmani;
        private readonly string filePath;

        public ApartmanRepository(string filePath)
        {
            this.filePath = filePath;
            apartmani = new List<Apartman>();
            LoadFromFile();
        }

        public List<Apartman> GetAll()
        {
            return new List<Apartman>(apartmani);
        }

        public Apartman? GetById(string id)
        {
            return apartmani.FirstOrDefault(a => a.GetId() == id);
        }

        public List<Apartman> GetByHotel(string sifraHotela)
        {
            return apartmani.Where(a => a.GetSifraHotela() == sifraHotela).ToList();
        }

        public bool Add(Apartman apartman)
        {
            if (apartman == null)
                return false;

            if (GetById(apartman.GetId()) != null)
                return false;

            apartmani.Add(apartman);
            SaveToFile();
            return true;
        }

        public bool Update(Apartman apartman)
        {
            if (apartman == null)
                return false;

            var existing = GetById(apartman.GetId());
            if (existing == null)
                return false;

            apartmani.Remove(existing);
            apartmani.Add(apartman);
            SaveToFile();
            return true;
        }

        public bool Delete(string id)
        {
            var apartman = GetById(id);
            if (apartman == null)
                return false;

            apartmani.Remove(apartman);
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

                var data = apartmani.Select(a => new
                {
                    Id = a.GetId(),
                    Ime = a.GetIme(),
                    Opis = a.GetOpis(),
                    BrojSoba = a.GetBrojSoba(),
                    MaxBrojGostiju = a.GetMaxBrojGostiju(),
                    SifraHotela = a.GetSifraHotela()
                }).ToList();

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri čuvanju apartmana: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    apartmani = new List<Apartman>();
                    return;
                }

                var json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeAnonymousType(json, new[]
                {
                    new
                    {
                        Id = "",
                        Ime = "",
                        Opis = "",
                        BrojSoba = 0,
                        MaxBrojGostiju = 0,
                        SifraHotela = ""
                    }
                });

                if (data != null)
                {
                    apartmani = data.Select(d => new Apartman(
                        d.Id,
                        d.Ime,
                        d.Opis,
                        d.BrojSoba,
                        d.MaxBrojGostiju,
                        d.SifraHotela
                    )).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri učitavanju apartmana: {ex.Message}");
                apartmani = new List<Apartman>();
            }
        }
    }
}
