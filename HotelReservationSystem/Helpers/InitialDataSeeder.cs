using System;
using System.Linq;
using HotelReservationSystem.Models;
using HotelReservationSystem.Services;

namespace HotelReservationSystem.Helpers
{
    public class InitialDataSeeder
    {
        public static void SeedData()
        {
            try
            {
                var korisnikService = ServiceLocator.Instance.KorisnikService;
                
                var existingAdmins = korisnikService.GetAllKorisnici()
                    .Where(k => k.GetTipKorisnika() == KorisnikTip.Administrator)
                    .ToList();

                if (existingAdmins.Count == 0)
                {
                    var admin = new Korisnik(
                        "1234567890123",
                        "admin@hotel.rs",
                        "admin123",
                        "Admin",
                        "Adminović",
                        "0601234567",
                        KorisnikTip.Administrator
                    );
                    korisnikService.AddKorisnik(admin);
                }

                SeedTestData(korisnikService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri inicijalizaciji podataka: {ex.Message}");
            }
        }

        private static void SeedTestData(KorisnikService korisnikService)
        {
            try
            {
                var allKorisnici = korisnikService.GetAllKorisnici();

                // Vlasnici
                if (!allKorisnici.Any(k => k.GetEmail() == "vlasnik@hotel.rs"))
                {
                    korisnikService.AddKorisnik(new Korisnik("1111111111111", "vlasnik@hotel.rs", "vlasnik123", "Petar", "Petrović", "0611111111", KorisnikTip.Vlasnik));
                }
                if (!allKorisnici.Any(k => k.GetEmail() == "vlasnik2@hotel.rs"))
                {
                    korisnikService.AddKorisnik(new Korisnik("1111111111112", "vlasnik2@hotel.rs", "vlasnik123", "Jovan", "Jovanović", "0611111112", KorisnikTip.Vlasnik));
                }
                if (!allKorisnici.Any(k => k.GetEmail() == "vlasnik3@hotel.rs"))
                {
                    korisnikService.AddKorisnik(new Korisnik("1111111111113", "vlasnik3@hotel.rs", "vlasnik123", "Stefan", "Stefanović", "0611111113", KorisnikTip.Vlasnik));
                }

                // Gosti
                if (!allKorisnici.Any(k => k.GetEmail() == "gost@hotel.rs"))
                {
                    korisnikService.AddKorisnik(new Korisnik("2222222222222", "gost@hotel.rs", "gost123", "Marko", "Marković", "0622222222", KorisnikTip.Gost));
                }
                if (!allKorisnici.Any(k => k.GetEmail() == "gost2@hotel.rs"))
                {
                    korisnikService.AddKorisnik(new Korisnik("2222222222223", "gost2@hotel.rs", "gost123", "Ana", "Anić", "0622222223", KorisnikTip.Gost));
                }
                if (!allKorisnici.Any(k => k.GetEmail() == "gost3@hotel.rs"))
                {
                    korisnikService.AddKorisnik(new Korisnik("2222222222224", "gost3@hotel.rs", "gost123", "Nikola", "Nikolić", "0622222224", KorisnikTip.Gost));
                }

                // Hoteli i apartmani
                var hotelService = ServiceLocator.Instance.HotelService;
                var apartmanService = ServiceLocator.Instance.ApartmanService;
                var allHotels = hotelService.GetAllHotels();

                if (allHotels.Count == 0)
                {
                    // Hotel 1 - Grand Hotel (5 zvezdica)
                    var hotel1 = new Hotel("HOT0001", "Grand Hotel Belgrade", 2020, 5, "1111111111111", StatusHotela.Odobren);
                    hotelService.AddHotel(hotel1);
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Penthouse Suite", "Luksuzni penthouse sa terasom", 3, 6, "HOT0001"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Deluxe Room", "Prostrana soba sa king size krevetom", 2, 4, "HOT0001"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Standard Room", "Komforna standardna soba", 1, 2, "HOT0001"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Family Suite", "Apartman za porodicu", 2, 5, "HOT0001"));

                    // Hotel 2 - Four Seasons (5 zvezdica)
                    var hotel2 = new Hotel("HOT0002", "Four Seasons Novi Sad", 2018, 5, "1111111111111", StatusHotela.Odobren);
                    hotelService.AddHotel(hotel2);
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Presidential Suite", "Predsednički apartman", 4, 8, "HOT0002"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Executive Room", "Poslovna soba", 1, 3, "HOT0002"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Junior Suite", "Mali apartman", 2, 3, "HOT0002"));

                    // Hotel 3 - Hilton (4 zvezdice)
                    var hotel3 = new Hotel("HOT0003", "Hilton Garden Inn", 2019, 4, "1111111111112", StatusHotela.Odobren);
                    hotelService.AddHotel(hotel3);
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Garden View", "Soba sa pogledom na baštu", 1, 2, "HOT0003"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "City View", "Soba sa pogledom na grad", 1, 2, "HOT0003"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Family Room", "Porodična soba", 2, 4, "HOT0003"));

                    // Hotel 4 - Holiday Inn (3 zvezdice)
                    var hotel4 = new Hotel("HOT0004", "Holiday Inn Express", 2021, 3, "1111111111112", StatusHotela.Odobren);
                    hotelService.AddHotel(hotel4);
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Economy Room", "Ekonomična soba", 1, 2, "HOT0004"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Twin Room", "Soba sa dva kreveta", 1, 2, "HOT0004"));

                    // Hotel 5 - Marriott (4 zvezdice)
                    var hotel5 = new Hotel("HOT0005", "Marriott Hotel Kragujevac", 2017, 4, "1111111111113", StatusHotela.Odobren);
                    hotelService.AddHotel(hotel5);
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Classic Room", "Klasična soba", 1, 2, "HOT0005"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Superior Room", "Superiorna soba", 2, 3, "HOT0005"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Deluxe Suite", "Lux apartman", 3, 5, "HOT0005"));

                    // Hotel 6 - Budget Hotel (2 zvezdice)
                    var hotel6 = new Hotel("HOT0006", "Budget Inn Nis", 2022, 2, "1111111111113", StatusHotela.Odobren);
                    hotelService.AddHotel(hotel6);
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Basic Room", "Osnovna soba", 1, 2, "HOT0006"));
                    apartmanService.AddApartman(new Apartman(Guid.NewGuid().ToString(), "Shared Room", "Deljiva soba", 1, 4, "HOT0006"));
                }

                // Dodaj test rezervacije
                var rezervacijaService = ServiceLocator.Instance.RezervacijaService;
                var sveRezervacije = rezervacijaService.GetAllRezervacije();
                
                if (sveRezervacije.Count == 0)
                {
                    var apartmani = apartmanService.GetAllApartmani();
                    if (apartmani.Count > 0)
                    {
                        // Test rezervacija na čekanju
                        rezervacijaService.CreateRezervacija(new Rezervacija(
                            Guid.NewGuid().ToString(),
                            "2222222222222",
                            apartmani[0].GetId(),
                            DateTime.Today.AddDays(5),
                            DateTime.Today.AddDays(7),
                            StatusRezervacije.NaCekanju,
                            ""
                        ));

                        // Test potvrđena rezervacija
                        var rez2 = new Rezervacija(
                            Guid.NewGuid().ToString(),
                            "2222222222223",
                            apartmani[1].GetId(),
                            DateTime.Today.AddDays(10),
                            DateTime.Today.AddDays(12),
                            StatusRezervacije.Potvrdjeno,
                            ""
                        );
                        rezervacijaService.CreateRezervacija(rez2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri dodavanju test podataka: {ex.Message}");
            }
        }
    }
}
