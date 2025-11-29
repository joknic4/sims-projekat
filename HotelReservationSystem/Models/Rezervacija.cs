using System;

namespace HotelReservationSystem.Models
{
    public class Rezervacija
    {
        private string id;
        private string jmbgGosta;
        private string imeApartmana;
        private string sifraHotela;
        private DateTime datum;
        private StatusRezervacije status;
        private string razlogOdbijanja;
        
        public Rezervacija()
        {
            id = Guid.NewGuid().ToString();
            jmbgGosta = string.Empty;
            imeApartmana = string.Empty;
            sifraHotela = string.Empty;
            datum = DateTime.Now;
            status = StatusRezervacije.NaCekanju;
            razlogOdbijanja = string.Empty;
        }
        
        public Rezervacija(string jmbgGosta, string imeApartmana, string sifraHotela, DateTime datum)
        {
            this.id = Guid.NewGuid().ToString();
            this.jmbgGosta = jmbgGosta;
            this.imeApartmana = imeApartmana;
            this.sifraHotela = sifraHotela;
            this.datum = datum;
            this.status = StatusRezervacije.NaCekanju;
            this.razlogOdbijanja = string.Empty;
        }
        
        public string GetId() => id;
        public string GetJmbgGosta() => jmbgGosta;
        public string GetImeApartmana() => imeApartmana;
        public string GetSifraHotela() => sifraHotela;
        public DateTime GetDatum() => datum;
        public StatusRezervacije GetStatus() => status;
        public string GetRazlogOdbijanja() => razlogOdbijanja;
        
        public void Potvrdi()
        {
            status = StatusRezervacije.Potvrdjeno;
        }
        
        public void Odbij(string razlog)
        {
            status = StatusRezervacije.Odbijeno;
            razlogOdbijanja = razlog;
        }
        
        public void Otkazi()
        {
            status = StatusRezervacije.Otkazano;
        }
        
        public void SetStatus(StatusRezervacije noviStatus)
        {
            status = noviStatus;
        }
    }
}
