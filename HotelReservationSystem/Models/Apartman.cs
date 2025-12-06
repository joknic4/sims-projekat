using System;

namespace HotelReservationSystem.Models
{
    public class Apartman
    {
        private string id;
        private string ime;
        private string opis;
        private int brojSoba;
        private int maxBrojGostiju;
        private string sifraHotela;

        public Apartman()
        {
            id = Guid.NewGuid().ToString();
            ime = string.Empty;
            opis = string.Empty;
            brojSoba = 1;
            maxBrojGostiju = 1;
            sifraHotela = string.Empty;
        }

        public Apartman(string id, string ime, string opis, int brojSoba, int maxBrojGostiju, string sifraHotela)
        {
            this.id = id;
            this.ime = ime;
            this.opis = opis;
            this.brojSoba = brojSoba;
            this.maxBrojGostiju = maxBrojGostiju;
            this.sifraHotela = sifraHotela;
        }

        public string GetId() => id;
        public string GetIme() => ime;
        public string GetOpis() => opis;
        public int GetBrojSoba() => brojSoba;
        public int GetMaxBrojGostiju() => maxBrojGostiju;
        public string GetSifraHotela() => sifraHotela;

        public void SetIme(string novoIme)
        {
            if (string.IsNullOrWhiteSpace(novoIme))
                throw new ArgumentException("Ime apartmana ne može biti prazno");
            
            ime = novoIme;
        }

        public void SetOpis(string noviOpis)
        {
            opis = noviOpis ?? string.Empty;
        }

        public void SetBrojSoba(int broj)
        {
            if (broj < 1)
                throw new ArgumentException("Broj soba mora biti pozitivan broj");
            
            brojSoba = broj;
        }

        public void SetMaxBrojGostiju(int broj)
        {
            if (broj < 1)
                throw new ArgumentException("Maksimalan broj gostiju mora biti pozitivan broj");
            
            maxBrojGostiju = broj;
        }

        public override string ToString()
        {
            return $"{ime} - {brojSoba} sobe, do {maxBrojGostiju} gostiju";
        }
    }
}
