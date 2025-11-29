using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class RezervacijaService
    {
        private readonly IRezervacijaRepository rezervacijaRepository;
        
        public RezervacijaService(IRezervacijaRepository repository)
        {
            rezervacijaRepository = repository;
        }
        
        public bool KreirajRezervaciju(string jmbgGosta, string imeApartmana, string sifraHotela, DateTime datum)
        {
            // Proveri da li je apartman zauzet
            var postojece = rezervacijaRepository.GetAll()
                .Where(r => r.GetImeApartmana() == imeApartmana && 
                           r.GetSifraHotela() == sifraHotela &&
                           r.GetDatum().Date == datum.Date &&
                           r.GetStatus() != StatusRezervacije.Odbijeno)
                .ToList();
            
            if (postojece.Count > 0)
            {
                return false; // Apartman zauzet
            }
            
            var rezervacija = new Rezervacija(jmbgGosta, imeApartmana, sifraHotela, datum);
            rezervacijaRepository.Add(rezervacija);
            rezervacijaRepository.Save();
            
            return true;
        }
        
        public List<Rezervacija> GetRezervacijeGosta(string jmbgGosta)
        {
            return rezervacijaRepository.GetByGost(jmbgGosta);
        }
        
        public void PotvrdiRezervaciju(string id)
        {
            var rezervacija = rezervacijaRepository.GetById(id);
            if (rezervacija != null)
            {
                rezervacija.Potvrdi();
                rezervacijaRepository.Save();
            }
        }
        
        public void OdbijiRezervaciju(string id, string razlog)
        {
            var rezervacija = rezervacijaRepository.GetById(id);
            if (rezervacija != null)
            {
                rezervacija.Odbij(razlog);
                rezervacijaRepository.Save();
            }
        }
        
        public void OtkaziRezervaciju(string id)
        {
            var rezervacija = rezervacijaRepository.GetById(id);
            if (rezervacija != null && 
                (rezervacija.GetStatus() == StatusRezervacije.NaCekanju || 
                 rezervacija.GetStatus() == StatusRezervacije.Potvrdjeno))
            {
                rezervacija.Otkazi();
                rezervacijaRepository.Save();
            }
        }
        
        public List<Rezervacija> FilterByStatus(List<Rezervacija> rezervacije, StatusRezervacije status)
        {
            return rezervacije.Where(r => r.GetStatus() == status).ToList();
        }
    }
}
