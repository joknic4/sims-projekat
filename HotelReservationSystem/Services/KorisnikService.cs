using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class KorisnikService
    {
        private readonly IKorisnikRepository korisnikRepository;

        public KorisnikService(IKorisnikRepository korisnikRepository)
        {
            this.korisnikRepository = korisnikRepository;
        }

        public List<Korisnik> GetAllKorisnici()
        {
            return korisnikRepository.GetAll();
        }

        public Korisnik? GetKorisnikByJmbg(string jmbg)
        {
            return korisnikRepository.GetByJmbg(jmbg);
        }

        public Korisnik? GetKorisnikByEmail(string email)
        {
            return korisnikRepository.GetByEmail(email);
        }

        public bool AddKorisnik(Korisnik korisnik)
        {
            if (korisnik == null)
                return false;

            if (!ValidateJmbg(korisnik.GetJmbg()))
                throw new ArgumentException("JMBG mora imati tačno 13 cifara");

            if (!ValidateEmail(korisnik.GetEmail()))
                throw new ArgumentException("Email adresa nije validna");

            if (string.IsNullOrWhiteSpace(korisnik.GetLozinka()))
                throw new ArgumentException("Lozinka ne može biti prazna");

            if (string.IsNullOrWhiteSpace(korisnik.GetIme()))
                throw new ArgumentException("Ime ne može biti prazno");

            if (string.IsNullOrWhiteSpace(korisnik.GetPrezime()))
                throw new ArgumentException("Prezime ne može biti prazno");

            return korisnikRepository.Add(korisnik);
        }

        public bool UpdateKorisnik(Korisnik korisnik)
        {
            if (korisnik == null)
                return false;

            return korisnikRepository.Update(korisnik);
        }

        public bool DeleteKorisnik(string jmbg)
        {
            return korisnikRepository.Delete(jmbg);
        }

        public bool ValidateJmbg(string jmbg)
        {
            if (string.IsNullOrWhiteSpace(jmbg))
                return false;

            return jmbg.Length == 13 && jmbg.All(char.IsDigit);
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public List<Korisnik> GetVlasnike()
        {
            return korisnikRepository.GetAll()
                .Where(k => k.GetTipKorisnika() == KorisnikTip.Vlasnik)
                .ToList();
        }
    }
}
