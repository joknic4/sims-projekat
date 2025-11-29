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
            sifra = "";
            ime = "";
            godinaIzgradnje = 0;
            brojZvezdica = 0;
            jmbgVlasnika = "";
            status = StatusHotela.NaCekanju;
        }
        
        public Hotel(string sifra, string ime, int godinaIzgradnje, int brojZvezdica, string jmbgVlasnika)
        {
            this.sifra = sifra;
            this.ime = ime;
            this.godinaIzgradnje = godinaIzgradnje;
            this.brojZvezdica = brojZvezdica;
            this.jmbgVlasnika = jmbgVlasnika;
            this.status = StatusHotela.NaCekanju;
        }
        
        public string GetSifra() => sifra;
        public string GetIme() => ime;
        public int GetGodinaIzgradnje() => godinaIzgradnje;
        public int GetBrojZvezdica() => brojZvezdica;
        public string GetJmbgVlasnika() => jmbgVlasnika;
        public StatusHotela GetStatus() => status;
        
        public void SetIme(string ime) => this.ime = ime;
        public void SetBrojZvezdica(int broj) => this.brojZvezdica = broj;
        public void SetStatus(StatusHotela noviStatus) => this.status = noviStatus;
    }
}
