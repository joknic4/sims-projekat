using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly string filePath = "Data/hoteli.json";
        private List<Hotel> hoteli;
        
        public HotelRepository()
        {
            hoteli = new List<Hotel>();
            Load();
        }
        
        public List<Hotel> GetAll()
        {
            return hoteli;
        }
        
        public Hotel? GetBySifra(string sifra)
        {
            return hoteli.FirstOrDefault(h => h.GetSifra() == sifra);
        }
        
        public void Add(Hotel hotel)
        {
            hoteli.Add(hotel);
        }
        
        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "Data");
            string json = JsonConvert.SerializeObject(hoteli, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        
        private void Load()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                hoteli = JsonConvert.DeserializeObject<List<Hotel>>(json) ?? new List<Hotel>();
            }
        }
    }
}
