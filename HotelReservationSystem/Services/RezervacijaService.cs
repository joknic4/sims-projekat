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
            var sveRezervacije = rezervacijaRepository.GetAll().ToList();
            var postojece = new List<Rezervacija>();
            
            foreach (var r in sveRezervacije)
            {
                if (r.GetImeApartmana() == imeApartmana && 
                    r.GetSifraHotela() == sifraHotela)
                {
                    if (r.GetDatum().Date == datum.Date)
                    {
                        if (r.GetStatus() != StatusRezervacije.Odbijeno)
                        {
                            postojece.Add(r);
                        }
                    }
                }
            }
            
            if (postojece.Count > 0)
            {
                return false;
            }
            
            var rezervacija = new Rezervacija(jmbgGosta, imeApartmana, sifraHotela, datum);
            rezervacijaRepository.Add(rezervacija);
            rezervacijaRepository.Save();
            
            return true;
        }
        
        public List<Rezervacija> GetRezervacijeGosta(string jmbgGosta)
        {
            var rezervacije = rezervacijaRepository.GetByGost(jmbgGosta);
            
            rezervacije.Sort((a, b) => 
            {
                int statusCompare = a.GetStatus().CompareTo(b.GetStatus());
                if (statusCompare != 0) return statusCompare;
                
                int datumCompare = DateTime.Compare(a.GetDatum(), b.GetDatum());
                if (datumCompare != 0) return datumCompare;
                
                return string.Compare(a.GetId(), b.GetId(), StringComparison.Ordinal);
            });
            
            return rezervacije;
        }
        
        public void PotvrdiRezervaciju(string id)
        {
            var sve = rezervacijaRepository.GetAll();
            Rezervacija? rezervacija = null;
            
            foreach (var r in sve)
            {
                if (r.GetId() == id)
                {
                    rezervacija = r;
                    break;
                }
            }
            
            if (rezervacija != null)
            {
                rezervacija.Potvrdi();
                rezervacijaRepository.Save();
            }
        }
        
        public void OdbijiRezervaciju(string id, string razlog)
        {
            var sve = rezervacijaRepository.GetAll();
            Rezervacija? rezervacija = null;
            
            foreach (var r in sve)
            {
                if (r.GetId() == id)
                {
                    rezervacija = r;
                    break;
                }
            }
            
            if (rezervacija != null)
            {
                rezervacija.Odbij(razlog);
                rezervacijaRepository.Save();
            }
        }
        
        public List<Rezervacija> FilterByStatus(StatusRezervacije status)
        {
            var sve = rezervacijaRepository.GetAll();
            var rezultat = new List<Rezervacija>();
            
            for (int i = 0; i < sve.Count; i++)
            {
                if (sve[i].GetStatus() == status)
                {
                    rezultat.Add(sve[i]);
                }
            }
            
            return rezultat;
        }
        
        public void OtkaziRezervaciju(string id)
        {
            var sve = rezervacijaRepository.GetAll();
            
            foreach (var r in sve)
            {
                if (r.GetId() == id)
                {
                    r.SetStatus(StatusRezervacije.Otkazano);
                    rezervacijaRepository.Save();
                    break;
                }
            }
        }
    }
}
