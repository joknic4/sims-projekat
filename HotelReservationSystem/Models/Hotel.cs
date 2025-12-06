using System;

namespace HotelReservationSystem.Models
{
    public class Hotel
    {
        private string sifra;
        private string ime;
        private int godinaIzgradnje;
        private int brojZvezdica;
        private string jmbgVlasnika;
        private StatusHotela status;

        public Hotel()
        {
            sifra = string.Empty;
            ime = string.Empty;
            godinaIzgradnje = DateTime.Now.Year;
            brojZvezdica = 1;
            jmbgVlasnika = string.Empty;
            status = StatusHotela.NaCekanju;
        }

        public Hotel(string sifra, string ime, int godinaIzgradnje, int brojZvezdica, 
                    string jmbgVlasnika, StatusHotela status)
        {
            this.sifra = sifra;
            this.ime = ime;
            this.godinaIzgradnje = godinaIzgradnje;
            this.brojZvezdica = brojZvezdica;
            this.jmbgVlasnika = jmbgVlasnika;
            this.status = status;
        }

        public string GetSifra() => sifra;
        public string GetIme() => ime;
        public int GetGodinaIzgradnje() => godinaIzgradnje;
        public int GetBrojZvezdica() => brojZvezdica;
        public string GetJmbgVlasnika() => jmbgVlasnika;
        public StatusHotela GetStatus() => status;

        public void SetStatus(StatusHotela noviStatus)
        {
            status = noviStatus;
        }

        public void SetIme(string novoIme)
        {
            if (string.IsNullOrWhiteSpace(novoIme))
                throw new ArgumentException("Ime hotela ne može biti prazno");
            
            ime = novoIme;
        }

        public void SetGodinaIzgradnje(int godina)
        {
            if (godina < 1800 || godina > DateTime.Now.Year)
                throw new ArgumentException("Nevalidna godina izgradnje");
            
            godinaIzgradnje = godina;
        }

        public void SetBrojZvezdica(int broj)
        {
            if (broj < 1 || broj > 5)
                throw new ArgumentException("Broj zvezdica mora biti između 1 i 5");
            
            brojZvezdica = broj;
        }

        public override string ToString()
        {
            return $"{ime} ({brojZvezdica}*) - {godinaIzgradnje} - Status: {status}";
        }
    }
}
