using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class ApartmanService
    {
        private readonly IApartmanRepository apartmanRepository;
        private readonly IRezervacijaRepository rezervacijaRepository;

        public ApartmanService(IApartmanRepository apartmanRepository, IRezervacijaRepository rezervacijaRepository)
        {
            this.apartmanRepository = apartmanRepository;
            this.rezervacijaRepository = rezervacijaRepository;
        }

        public List<Apartman> GetAllApartmani()
        {
            return apartmanRepository.GetAll();
        }

        public Apartman? GetApartmanById(string id)
        {
            return apartmanRepository.GetById(id);
        }

        public List<Apartman> GetApartmaniByHotel(string sifraHotela)
        {
            return apartmanRepository.GetByHotel(sifraHotela);
        }

        public bool AddApartman(Apartman apartman)
        {
            if (apartman == null)
                return false;

            if (string.IsNullOrWhiteSpace(apartman.GetIme()))
                throw new ArgumentException("Ime apartmana ne može biti prazno");

            if (apartman.GetBrojSoba() < 1)
                throw new ArgumentException("Broj soba mora biti najmanje 1");

            if (apartman.GetMaxBrojGostiju() < 1)
                throw new ArgumentException("Maksimalan broj gostiju mora biti najmanje 1");

            // Provera jedinstvenog imena u okviru hotela
            var existingApartmani = GetApartmaniByHotel(apartman.GetSifraHotela());
            if (existingApartmani.Any(a => a.GetIme().Equals(apartman.GetIme(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Apartman sa ovim imenom već postoji u hotelu");
            }

            return apartmanRepository.Add(apartman);
        }

        public bool UpdateApartman(Apartman apartman)
        {
            if (apartman == null)
                return false;

            return apartmanRepository.Update(apartman);
        }

        public bool DeleteApartman(string id)
        {
            return apartmanRepository.Delete(id);
        }

        public bool CheckAvailability(string idApartmana, DateTime datumOd, DateTime datumDo)
        {
            var rezervacije = rezervacijaRepository.GetByApartman(idApartmana);

            // Filtriranje samo aktivnih rezervacija (potvrdjene i na čekanju)
            var activeRezervacije = rezervacije.Where(r =>
                r.GetStatus() == StatusRezervacije.Potvrdjeno ||
                r.GetStatus() == StatusRezervacije.NaCekanju
            ).ToList();

            // Provera preklapanja datuma
            foreach (var rezervacija in activeRezervacije)
            {
                bool overlaps = !(datumDo < rezervacija.GetDatumOd() || datumOd > rezervacija.GetDatumDo());
                if (overlaps)
                {
                    return false; // Apartman nije dostupan
                }
            }

            return true; // Apartman je dostupan
        }

        public string GenerateApartmanId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
