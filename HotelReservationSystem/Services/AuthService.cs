using System;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class AuthService
    {
        private readonly IKorisnikRepository korisnikRepository;
        private Korisnik? trenutniKorisnik;

        public AuthService(IKorisnikRepository korisnikRepository)
        {
            this.korisnikRepository = korisnikRepository;
            trenutniKorisnik = null;
        }

        public Korisnik? Login(string email, string lozinka)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(lozinka))
                return null;

            var korisnik = korisnikRepository.GetByEmail(email);
            if (korisnik == null)
                return null;

            if (korisnik.GetLozinka() != lozinka)
                return null;

            trenutniKorisnik = korisnik;
            return korisnik;
        }

        public void Logout()
        {
            trenutniKorisnik = null;
        }

        public Korisnik? GetCurrentUser()
        {
            return trenutniKorisnik;
        }

        public bool IsLoggedIn()
        {
            return trenutniKorisnik != null;
        }

        public bool RegisterGost(Korisnik korisnik)
        {
            if (korisnik == null)
                return false;

            if (korisnik.GetTipKorisnika() != KorisnikTip.Gost)
                return false;

            return korisnikRepository.Add(korisnik);
        }

        public bool IsAdministrator()
        {
            return trenutniKorisnik?.GetTipKorisnika() == KorisnikTip.Administrator;
        }

        public bool IsVlasnik()
        {
            return trenutniKorisnik?.GetTipKorisnika() == KorisnikTip.Vlasnik;
        }

        public bool IsGost()
        {
            return trenutniKorisnik?.GetTipKorisnika() == KorisnikTip.Gost;
        }
    }
}
