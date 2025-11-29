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
            jmbgGosta = "";
            imeApartmana = "";
            sifraHotela = "";
            datum = DateTime.Now;
            status = StatusRezervacije.NaCekanju;
            razlogOdbijanja = "";
        }
        
        public Rezervacija(string jmbgGosta, string imeApartmana, string sifraHotela, DateTime datum)
        {
            this.id = Guid.NewGuid().ToString();
            this.jmbgGosta = jmbgGosta;
            this.imeApartmana = imeApartmana;
            this.sifraHotela = sifraHotela;
            this.datum = datum;
            this.status = StatusRezervacije.NaCekanju;
            this.razlogOdbijanja = "";
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
        
        public void SetStatus(StatusRezervacije noviStatus)
        {
            status = noviStatus;
        }
    }
}
