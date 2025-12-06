using System;

namespace HotelReservationSystem.Models
{
    public class Korisnik
    {
        private string jmbg;
        private string email;
        private string lozinka;
        private string ime;
        private string prezime;
        private string mobilniTelefon;
        private KorisnikTip tipKorisnika;

        public Korisnik()
        {
            jmbg = string.Empty;
            email = string.Empty;
            lozinka = string.Empty;
            ime = string.Empty;
            prezime = string.Empty;
            mobilniTelefon = string.Empty;
            tipKorisnika = KorisnikTip.Gost;
        }

        public Korisnik(string jmbg, string email, string lozinka, string ime, string prezime, 
                       string mobilniTelefon, KorisnikTip tipKorisnika)
        {
            this.jmbg = jmbg;
            this.email = email;
            this.lozinka = lozinka;
            this.ime = ime;
            this.prezime = prezime;
            this.mobilniTelefon = mobilniTelefon;
            this.tipKorisnika = tipKorisnika;
        }

        public string GetJmbg() => jmbg;
        public string GetEmail() => email;
        public string GetLozinka() => lozinka;
        public string GetIme() => ime;
        public string GetPrezime() => prezime;
        public string GetMobilniTelefon() => mobilniTelefon;
        public KorisnikTip GetTipKorisnika() => tipKorisnika;

        public void SetLozinka(string novaLozinka)
        {
            if (string.IsNullOrWhiteSpace(novaLozinka))
                throw new ArgumentException("Lozinka ne može biti prazna");
            
            lozinka = novaLozinka;
        }

        public void SetEmail(string noviEmail)
        {
            if (string.IsNullOrWhiteSpace(noviEmail))
                throw new ArgumentException("Email ne može biti prazan");
            
            email = noviEmail;
        }

        public void SetIme(string novoIme)
        {
            if (string.IsNullOrWhiteSpace(novoIme))
                throw new ArgumentException("Ime ne može biti prazno");
            
            ime = novoIme;
        }

        public void SetPrezime(string novoPrezime)
        {
            if (string.IsNullOrWhiteSpace(novoPrezime))
                throw new ArgumentException("Prezime ne može biti prazno");
            
            prezime = novoPrezime;
        }

        public void SetMobilniTelefon(string noviBroj)
        {
            if (string.IsNullOrWhiteSpace(noviBroj))
                throw new ArgumentException("Mobilni telefon ne može biti prazan");
            
            mobilniTelefon = noviBroj;
        }

        public override string ToString()
        {
            return $"{ime} {prezime} ({email}) - {tipKorisnika}";
        }
    }
}
