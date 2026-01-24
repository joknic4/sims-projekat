using System;
using System.IO;
using HotelReservationSystem.Repositories;
using HotelReservationSystem.Services;

namespace HotelReservationSystem.Helpers
{
    public class ServiceLocator
    {
        private static ServiceLocator? instance;
        private static readonly object lockObject = new object();

        public AuthService AuthService { get; private set; }
        public KorisnikService KorisnikService { get; private set; }
        public HotelService HotelService { get; private set; }
        public ApartmanService ApartmanService { get; private set; }
        public RezervacijaService RezervacijaService { get; private set; }

        private ServiceLocator()
        {
            // Kreiranje Data direktorijuma ako ne postoji
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            // Inicijalizacija repository-ja
            var korisnikRepo = new KorisnikRepository(Path.Combine(dataPath, "korisnici.json"));
            var hotelRepo = new HotelRepository(Path.Combine(dataPath, "hoteli.json"));
            var apartmanRepo = new ApartmanRepository(Path.Combine(dataPath, "apartmani.json"));
            var rezervacijaRepo = new RezervacijaRepository(Path.Combine(dataPath, "rezervacije.json"));

            // Inicijalizacija servisa
            AuthService = new AuthService(korisnikRepo);
            KorisnikService = new KorisnikService(korisnikRepo);
            ApartmanService = new ApartmanService(apartmanRepo, rezervacijaRepo);
            HotelService = new HotelService(hotelRepo, apartmanRepo);
            RezervacijaService = new RezervacijaService(rezervacijaRepo, hotelRepo, apartmanRepo);
        }

        public static ServiceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceLocator();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
