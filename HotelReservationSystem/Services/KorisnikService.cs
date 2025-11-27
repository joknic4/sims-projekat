using System;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class KorisnikService
    {
        private readonly IKorisnikRepository korisnikRepository;
        
        public KorisnikService(IKorisnikRepository repository)
        {
            korisnikRepository = repository;
        }
        
        public bool RegistrujGosta(string jmbg, string email, string lozinka, string ime, 
                                   string prezime, string telefon)
        {
            // Provera jedinstvnosti
            if (korisnikRepository.GetByJmbg(jmbg) != null)
            {
                Console.WriteLine("Korisnik sa ovim JMBG vec postoji");
                return false;
            }
            
            if (korisnikRepository.GetByEmail(email) != null)
            {
                Console.WriteLine("Korisnik sa ovim emailom vec postoji");
                return false;
            }
            
            var gost = new Korisnik(jmbg, email, lozinka, ime, prezime, telefon, KorisnikTip.Gost);
            korisnikRepository.Add(gost);
            korisnikRepository.Save();
            
            return true;
        }
        
        public bool RegistrujVlasnika(string jmbg, string email, string lozinka, string ime, 
                                      string prezime, string telefon)
        {
            // Provera jedinstvnosti
            if (korisnikRepository.GetByJmbg(jmbg) != null)
            {
                return false;
            }
            
            if (korisnikRepository.GetByEmail(email) != null)
            {
                return false;
            }
            
            var vlasnik = new Korisnik(jmbg, email, lozinka, ime, prezime, telefon, KorisnikTip.Vlasnik);
            korisnikRepository.Add(vlasnik);
            korisnikRepository.Save();
            
            return true;
        }
    }
}
