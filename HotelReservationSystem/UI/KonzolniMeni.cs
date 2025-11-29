using System;
using HotelReservationSystem.Services;

namespace HotelReservationSystem.UI
{
    public class KonzolniMeni
    {
        private readonly AuthService authService;
        private readonly KorisnikService korisnikService;
        private readonly HotelService hotelService;
        
        public KonzolniMeni(AuthService auth, KorisnikService korisnik, HotelService hotel)
        {
            authService = auth;
            korisnikService = korisnik;
            hotelService = hotel;
        }
        
        public void Pokreni()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== HOTEL RESERVATION SYSTEM ===");
                Console.WriteLine();
                
                if (!authService.JePrijavljen())
                {
                    PrikaziGlavniMeni();
                }
                else
                {
                    var korisnik = authService.GetTrenutniKorisnik();
                    Console.WriteLine($"Prijavljen: {korisnik?.GetIme()} {korisnik?.GetPrezime()}");
                    PrikaziMeniZaPrijavljenog();
                }
            }
        }
        
        private void PrikaziGlavniMeni()
        {
            Console.WriteLine("1. Prijava");
            Console.WriteLine("2. Registracija");
            Console.WriteLine("3. Prikaz svih hotela");
            Console.WriteLine("0. Izlaz");
            Console.Write("\nIzaberite opciju: ");
            
            string? izbor = Console.ReadLine();
            
            switch (izbor)
            {
                case "1":
                    Prijava();
                    break;
                case "2":
                    Registracija();
                    break;
                case "3":
                    PrikaziSveHotele();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
            }
        }
        
        private void PrikaziMeniZaPrijavljenog()
        {
            Console.WriteLine("\n1. Prikaz svih hotela");
            Console.WriteLine("2. Pretraga hotela");
            Console.WriteLine("3. Odjava");
            Console.Write("\nIzaberite opciju: ");
            
            string? izbor = Console.ReadLine();
            
            switch (izbor)
            {
                case "1":
                    PrikaziSveHotele();
                    break;
                case "2":
                    PretragaHotela();
                    break;
                case "3":
                    authService.Logout();
                    break;
            }
        }
        
        private void Prijava()
        {
            Console.Clear();
            Console.WriteLine("=== PRIJAVA ===\n");
            
            Console.Write("Email: ");
            string? email = Console.ReadLine();
            
            Console.Write("Lozinka: ");
            string? lozinka = Console.ReadLine();
            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(lozinka))
            {
                Console.WriteLine("\nEmail i lozinka su obavezni!");
                Console.ReadKey();
                return;
            }
            
            if (authService.Login(email, lozinka))
            {
                Console.WriteLine("\nUspesna prijava!");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\nNeispravni podaci!");
                Console.ReadKey();
            }
        }
        
        private void Registracija()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRACIJA ===\n");
            
            Console.Write("JMBG: ");
            string? jmbg = Console.ReadLine() ?? "";
            
            Console.Write("Email: ");
            string? email = Console.ReadLine() ?? "";
            
            Console.Write("Lozinka: ");
            string? lozinka = Console.ReadLine() ?? "";
            
            Console.Write("Ime: ");
            string? ime = Console.ReadLine() ?? "";
            
            Console.Write("Prezime: ");
            string? prezime = Console.ReadLine() ?? "";
            
            Console.Write("Telefon: ");
            string? telefon = Console.ReadLine() ?? "";
            
            if (korisnikService.RegistrujGosta(jmbg, email, lozinka, ime, prezime, telefon))
            {
                Console.WriteLine("\nUspesna registracija!");
            }
            else
            {
                Console.WriteLine("\nGreska pri registraciji!");
            }
            
            Console.ReadKey();
        }
        
        private void PrikaziSveHotele()
        {
            Console.Clear();
            Console.WriteLine("=== SVI HOTELI ===\n");
            
            var hoteli = hotelService.GetSviHoteli();
            
            if (hoteli.Count == 0)
            {
                Console.WriteLine("Nema hotela u sistemu.");
            }
            else
            {
                foreach (var hotel in hoteli)
                {
                    Console.WriteLine($"[{hotel.GetSifra()}] {hotel.GetIme()} - Status: {hotel.GetStatus()}");
                    Console.WriteLine($"    Zvezdice: {hotel.GetBrojZvezdica()}, Godina: {hotel.GetGodinaIzgradnje()}");
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine("\nPritisnite bilo koji taster...");
            Console.ReadKey();
        }
        
        private void PretragaHotela()
        {
            Console.Clear();
            Console.WriteLine("=== PRETRAGA HOTELA ===\n");
            Console.WriteLine("1. Po imenu");
            Console.WriteLine("2. Po broju zvezdica");
            Console.Write("\nIzaberite: ");
            
            string? izbor = Console.ReadLine();
            
            if (izbor == "1")
            {
                Console.Write("Unesite ime: ");
                string? ime = Console.ReadLine() ?? "";
                var hoteli = hotelService.PretraziPoImenu(ime);
                
                Console.WriteLine($"\nPronadjeno {hoteli.Count} hotela:");
                foreach (var h in hoteli)
                {
                    Console.WriteLine($"- {h.GetIme()} ({h.GetBrojZvezdica()} zvezdica)");
                }
            }
            else if (izbor == "2")
            {
                Console.Write("Unesite broj zvezdica: ");
                if (int.TryParse(Console.ReadLine(), out int zvezdice))
                {
                    var hoteli = hotelService.PretraziPoZvezdicama(zvezdice);
                    Console.WriteLine($"\nPronadjeno {hoteli.Count} hotela:");
                    foreach (var h in hoteli)
                    {
                        Console.WriteLine($"- {h.GetIme()}");
                    }
                }
            }
            
            Console.WriteLine("\nPritisnite bilo koji taster...");
            Console.ReadKey();
        }
    }
}
