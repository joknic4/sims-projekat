using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Repositories;

namespace HotelReservationSystem.Services
{
    public class HotelService
    {
        private readonly IHotelRepository hotelRepository;
        private readonly IApartmanRepository apartmanRepository;

        public HotelService(IHotelRepository hotelRepository, IApartmanRepository apartmanRepository)
        {
            this.hotelRepository = hotelRepository;
            this.apartmanRepository = apartmanRepository;
        }

        public List<Hotel> GetAllHotels()
        {
            return hotelRepository.GetAll();
        }

        public Hotel? GetHotelBySifra(string sifra)
        {
            return hotelRepository.GetBySifra(sifra);
        }

        public List<Hotel> GetHotelsByVlasnik(string jmbgVlasnika)
        {
            return hotelRepository.GetByVlasnik(jmbgVlasnika);
        }

        public bool AddHotel(Hotel hotel)
        {
            if (hotel == null)
                return false;

            if (string.IsNullOrWhiteSpace(hotel.GetSifra()))
                throw new ArgumentException("Šifra hotela ne može biti prazna");

            if (string.IsNullOrWhiteSpace(hotel.GetIme()))
                throw new ArgumentException("Ime hotela ne može biti prazno");

            if (hotel.GetGodinaIzgradnje() < 1800 || hotel.GetGodinaIzgradnje() > DateTime.Now.Year)
                throw new ArgumentException("Godina izgradnje nije validna");

            if (hotel.GetBrojZvezdica() < 1 || hotel.GetBrojZvezdica() > 5)
                throw new ArgumentException("Broj zvezdica mora biti između 1 i 5");

            return hotelRepository.Add(hotel);
        }

        public bool UpdateHotel(Hotel hotel)
        {
            if (hotel == null)
                return false;

            return hotelRepository.Update(hotel);
        }

        public bool DeleteHotel(string sifra)
        {
            return hotelRepository.Delete(sifra);
        }

        public List<Hotel> GetApprovedHotels()
        {
            return hotelRepository.GetAll()
                .Where(h => h.GetStatus() == StatusHotela.Odobren)
                .ToList();
        }

        public List<Hotel> SearchHotelsByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetApprovedHotels();

            var approvedHotels = GetApprovedHotels();
            return approvedHotels
                .Where(h => h.GetIme().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Hotel> SearchHotelsByYear(int godina)
        {
            var approvedHotels = GetApprovedHotels();
            return approvedHotels
                .Where(h => h.GetGodinaIzgradnje() == godina)
                .ToList();
        }

        public List<Hotel> SearchHotelsByStars(int brojZvezdica)
        {
            var approvedHotels = GetApprovedHotels();
            return approvedHotels
                .Where(h => h.GetBrojZvezdica() == brojZvezdica)
                .ToList();
        }

        public List<Hotel> SearchHotelsByApartments(int? brojSoba, int? brojGostiju, string logickiOperator)
        {
            var approvedHotels = GetApprovedHotels();
            var result = new List<Hotel>();

            foreach (var hotel in approvedHotels)
            {
                var apartmani = apartmanRepository.GetByHotel(hotel.GetSifra());
                bool matches = false;

                if (logickiOperator == "&")
                {
                    // AND operator - apartman mora imati i broj soba i broj gostiju
                    matches = apartmani.Any(a =>
                        (!brojSoba.HasValue || a.GetBrojSoba() == brojSoba.Value) &&
                        (!brojGostiju.HasValue || a.GetMaxBrojGostiju() == brojGostiju.Value)
                    );
                }
                else if (logickiOperator == "|")
                {
                    // OR operator - apartman mora imati ili broj soba ili broj gostiju
                    matches = apartmani.Any(a =>
                        (brojSoba.HasValue && a.GetBrojSoba() == brojSoba.Value) ||
                        (brojGostiju.HasValue && a.GetMaxBrojGostiju() == brojGostiju.Value)
                    );
                }
                else
                {
                    // Bez operatora - samo jedan kriterijum
                    if (brojSoba.HasValue && !brojGostiju.HasValue)
                    {
                        matches = apartmani.Any(a => a.GetBrojSoba() == brojSoba.Value);
                    }
                    else if (!brojSoba.HasValue && brojGostiju.HasValue)
                    {
                        matches = apartmani.Any(a => a.GetMaxBrojGostiju() == brojGostiju.Value);
                    }
                }

                if (matches)
                {
                    result.Add(hotel);
                }
            }

            return result;
        }

        public List<Hotel> SortHotelsByStars(List<Hotel> hotels, bool ascending)
        {
            if (ascending)
            {
                return hotels.OrderBy(h => h.GetBrojZvezdica()).ToList();
            }
            else
            {
                return hotels.OrderByDescending(h => h.GetBrojZvezdica()).ToList();
            }
        }

        public bool ApproveHotel(string sifra)
        {
            var hotel = GetHotelBySifra(sifra);
            if (hotel == null)
                return false;

            hotel.SetStatus(StatusHotela.Odobren);
            return UpdateHotel(hotel);
        }

        public bool RejectHotel(string sifra)
        {
            var hotel = GetHotelBySifra(sifra);
            if (hotel == null)
                return false;

            hotel.SetStatus(StatusHotela.Odbijen);
            return UpdateHotel(hotel);
        }

        public string GenerateHotelCode()
        {
            var existingCodes = hotelRepository.GetAll().Select(h => h.GetSifra()).ToList();
            string newCode;
            int counter = 1;

            do
            {
                newCode = $"HOT{counter:D4}";
                counter++;
            } while (existingCodes.Contains(newCode));

            return newCode;
        }
    }
}
