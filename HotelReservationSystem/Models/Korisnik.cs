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
            jmbg = "";
            email = "";
            lozinka = "";
            ime = "";
            prezime = "";
            mobilniTelefon = "";
            tipKorisnika = KorisnikTip.Gost;
        }
        
        public Korisnik(string jmbg, string email, string lozinka, string ime, string prezime, string mobilniTelefon, KorisnikTip tip)
        {
            this.jmbg = jmbg;
            this.email = email;
            this.lozinka = lozinka;
            this.ime = ime;
            this.prezime = prezime;
            this.mobilniTelefon = mobilniTelefon;
            this.tipKorisnika = tip;
        }
        
        // Getteri
        public string GetJmbg() => jmbg;
        public string GetEmail() => email;
        public string GetLozinka() => lozinka;
        public string GetIme() => ime;
        public string GetPrezime() => prezime;
        public string GetMobilniTelefon() => mobilniTelefon;
        public KorisnikTip GetTipKorisnika() => tipKorisnika;
        
        // Setteri
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email ne moze biti prazan");
            this.email = email;
        }
        
        public void SetLozinka(string lozinka)
        {
            if (string.IsNullOrWhiteSpace(lozinka))
                throw new ArgumentException("Lozinka ne moze biti prazna");
            this.lozinka = lozinka;
        }
        
        public void SetIme(string ime)
        {
            if (string.IsNullOrWhiteSpace(ime))
                throw new ArgumentException("Ime ne moze biti prazno");
            this.ime = ime;
        }
        
        public void SetPrezime(string prezime)
        {
            if (string.IsNullOrWhiteSpace(prezime))
                throw new ArgumentException("Prezime ne moze biti prazno");
            this.prezime = prezime;
        }
    }
}
