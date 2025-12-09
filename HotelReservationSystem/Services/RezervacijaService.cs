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
        private readonly ApartmanService apartmanService;
        private readonly IHotelRepository hotelRepository;
        private readonly IApartmanRepository apartmanRepository;

        public RezervacijaService(IRezervacijaRepository rezervacijaRepository, 
                                 ApartmanService apartmanService,
                                 IHotelRepository hotelRepository,
                                 IApartmanRepository apartmanRepository)
        {
            this.rezervacijaRepository = rezervacijaRepository;
            this.apartmanService = apartmanService;
            this.hotelRepository = hotelRepository;
            this.apartmanRepository = apartmanRepository;
        }

        public List<Rezervacija> GetAllRezervacije()
        {
            return rezervacijaRepository.GetAll();
        }

        public Rezervacija? GetRezervacijaById(string id)
        {
            return rezervacijaRepository.GetById(id);
        }

        public List<Rezervacija> GetRezervacijeByGost(string jmbgGosta)
        {
            return rezervacijaRepository.GetByGost(jmbgGosta);
        }

        public List<Rezervacija> GetRezervacijeByApartman(string idApartmana)
        {
            return rezervacijaRepository.GetByApartman(idApartmana);
        }

        public bool CreateRezervacija(Rezervacija rezervacija)
        {
            if (rezervacija == null)
                return false;

            if (rezervacija.GetDatumOd() < DateTime.Today)
                throw new ArgumentException("Datum početka ne može biti u prošlosti");

            if (rezervacija.GetDatumDo() < rezervacija.GetDatumOd())
                throw new ArgumentException("Datum kraja ne može biti pre datuma početka");

            // Provera dostupnosti apartmana (bez ove rezervacije)
            if (!CheckAvailabilityExcluding(rezervacija.GetIdApartmana(), 
                                           rezervacija.GetDatumOd(), 
                                           rezervacija.GetDatumDo(),
                                           null))
            {
                throw new InvalidOperationException("Apartman nije dostupan za izabrane datume");
            }

            return rezervacijaRepository.Add(rezervacija);
        }

        public bool UpdateRezervacija(Rezervacija rezervacija)
        {
            if (rezervacija == null)
                return false;

            return rezervacijaRepository.Update(rezervacija);
        }

        public bool DeleteRezervacija(string id)
        {
            return rezervacijaRepository.Delete(id);
        }

        public bool ApproveRezervacija(string id)
        {
            var rezervacija = GetRezervacijaById(id);
            if (rezervacija == null)
                return false;

            if (rezervacija.GetStatus() != StatusRezervacije.NaCekanju)
                return false;

            // Ponovo proveriti dostupnost pre potvrde, ALI ignoriši ovu rezervaciju
            if (!CheckAvailabilityExcluding(rezervacija.GetIdApartmana(),
                                           rezervacija.GetDatumOd(),
                                           rezervacija.GetDatumDo(),
                                           id))
            {
                return false;
            }

            rezervacija.SetStatus(StatusRezervacije.Potvrdjeno);
            return UpdateRezervacija(rezervacija);
        }

        public bool RejectRezervacija(string id, string razlog)
        {
            var rezervacija = GetRezervacijaById(id);
            if (rezervacija == null)
                return false;

            if (rezervacija.GetStatus() != StatusRezervacije.NaCekanju)
                return false;

            rezervacija.SetStatus(StatusRezervacije.Odbijeno);
            rezervacija.SetRazlogOdbijanja(razlog);
            return UpdateRezervacija(rezervacija);
        }

        public bool CancelRezervacija(string id)
        {
            var rezervacija = GetRezervacijaById(id);
            if (rezervacija == null)
                return false;

            if (rezervacija.GetStatus() != StatusRezervacije.NaCekanju && 
                rezervacija.GetStatus() != StatusRezervacije.Potvrdjeno)
                return false;

            rezervacija.SetStatus(StatusRezervacije.Otkazano);
            return UpdateRezervacija(rezervacija);
        }

        public List<Rezervacija> GetRezervacijeByStatus(StatusRezervacije status, string? jmbgGosta = null)
        {
            var rezervacije = jmbgGosta != null 
                ? GetRezervacijeByGost(jmbgGosta) 
                : GetAllRezervacije();

            return rezervacije.Where(r => r.GetStatus() == status).ToList();
        }

        public List<Rezervacija> GetRezervacijeForVlasnik(string jmbgVlasnika)
        {
            var hoteli = hotelRepository.GetByVlasnik(jmbgVlasnika)
                .Where(h => h.GetStatus() == StatusHotela.Odobren)
                .ToList();

            var apartmaniIds = new List<string>();
            foreach (var hotel in hoteli)
            {
                var apartmani = apartmanRepository.GetByHotel(hotel.GetSifra());
                apartmaniIds.AddRange(apartmani.Select(a => a.GetId()));
            }

            var sveRezervacije = GetAllRezervacije();
            return sveRezervacije.Where(r => apartmaniIds.Contains(r.GetIdApartmana())).ToList();
        }

        public string GenerateRezervacijaId()
        {
            return Guid.NewGuid().ToString();
        }

        // NOVA metoda - proverava dostupnost ali ignoriše određenu rezervaciju
        private bool CheckAvailabilityExcluding(string idApartmana, DateTime datumOd, DateTime datumDo, string? excludeRezervacijaId)
        {
            var rezervacije = GetRezervacijeByApartman(idApartmana);

            // Filtriranje samo aktivnih rezervacija (potvrdjene i na čekanju)
            // ALI ignoriši rezervaciju koja se odobrava
            var activeRezervacije = rezervacije.Where(r =>
                (r.GetStatus() == StatusRezervacije.Potvrdjeno || r.GetStatus() == StatusRezervacije.NaCekanju) &&
                (excludeRezervacijaId == null || r.GetId() != excludeRezervacijaId)
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
    }
}
