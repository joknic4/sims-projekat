using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class AuthService
    {
        private readonly IKorisnikRepository korisnikRepository;
        private Korisnik? trenutniKorisnik;
        
        public AuthService(IKorisnikRepository repository)
        {
            korisnikRepository = repository;
            trenutniKorisnik = null;
        }
        
        public bool Login(string email, string lozinka)
        {
            var korisnik = korisnikRepository.GetByEmail(email);
            
            if (korisnik != null && korisnik.GetLozinka() == lozinka)
            {
                trenutniKorisnik = korisnik;
                return true;
            }
            
            return false;
        }
        
        public void Logout()
        {
            trenutniKorisnik = null;
        }
        
        public Korisnik? GetTrenutniKorisnik()
        {
            return trenutniKorisnik;
        }
        
        public bool JePrijavljen()
        {
            return trenutniKorisnik != null;
        }
    }
}
