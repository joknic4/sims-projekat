using System;
using HotelReservationSystem.Repositories;
using HotelReservationSystem.Services;

namespace HotelReservationSystem.Helpers
{
    public class ServiceLocator
    {
        private static ServiceLocator? instance;

        public AuthService AuthService { get; private set; }
        public KorisnikService KorisnikService { get; private set; }
        public HotelService HotelService { get; private set; }
        public ApartmanService ApartmanService { get; private set; }
        public RezervacijaService RezervacijaService { get; private set; }

        private ServiceLocator()
        {
            // Inicijalizacija repository-ja
            var korisnikRepo = new KorisnikRepository();
            var hotelRepo = new HotelRepository();
            var apartmanRepo = new ApartmanRepository();
            var rezervacijaRepo = new RezervacijaRepository();

            // Inicijalizacija servisa
            AuthService = new AuthService(korisnikRepo);
            KorisnikService = new KorisnikService(korisnikRepo);
            HotelService = new HotelService(hotelRepo);
            ApartmanService = new ApartmanService(apartmanRepo);
            RezervacijaService = new RezervacijaService(rezervacijaRepo);
        }

        public static ServiceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceLocator();
                }
                return instance;
            }
        }
    }
}
