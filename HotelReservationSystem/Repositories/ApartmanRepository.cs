using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public class ApartmanRepository : IApartmanRepository
    {
        private readonly string filePath = "Data/apartmani.json";
        private List<Apartman> apartmani;
        
        public ApartmanRepository()
        {
            apartmani = new List<Apartman>();
            Load();
        }
        
        public List<Apartman> GetAll()
        {
            return apartmani;
        }
        
        public List<Apartman> GetByHotel(string sifraHotela)
        {
            return apartmani.Where(a => a.GetSifraHotela() == sifraHotela).ToList();
        }
        
        public void Add(Apartman apartman)
        {
            apartmani.Add(apartman);
        }
        
        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "Data");
            string json = JsonConvert.SerializeObject(apartmani, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        
        private void Load()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                apartmani = JsonConvert.DeserializeObject<List<Apartman>>(json) ?? new List<Apartman>();
            }
        }
    }
}
