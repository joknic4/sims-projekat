using System;

namespace HotelReservationSystem.Models
{
    public class Rezervacija
    {
        private string id;
        private string jmbgGosta;
        private string idApartmana;
        private DateTime datumOd;
        private DateTime datumDo;
        private StatusRezervacije status;
        private string razlogOdbijanja;

        public Rezervacija()
        {
            id = Guid.NewGuid().ToString();
            jmbgGosta = string.Empty;
            idApartmana = string.Empty;
            datumOd = DateTime.Today;
            datumDo = DateTime.Today;
            status = StatusRezervacije.NaCekanju;
            razlogOdbijanja = string.Empty;
        }

        public Rezervacija(string id, string jmbgGosta, string idApartmana, DateTime datumOd, 
                          DateTime datumDo, StatusRezervacije status, string razlogOdbijanja)
        {
            this.id = id;
            this.jmbgGosta = jmbgGosta;
            this.idApartmana = idApartmana;
            this.datumOd = datumOd;
            this.datumDo = datumDo;
            this.status = status;
            this.razlogOdbijanja = razlogOdbijanja ?? string.Empty;
        }

        public string GetId() => id;
        public string GetJmbgGosta() => jmbgGosta;
        public string GetIdApartmana() => idApartmana;
        public DateTime GetDatumOd() => datumOd;
        public DateTime GetDatumDo() => datumDo;
        public StatusRezervacije GetStatus() => status;
        public string GetRazlogOdbijanja() => razlogOdbijanja;

        public void SetStatus(StatusRezervacije noviStatus)
        {
            status = noviStatus;
        }

        public void SetRazlogOdbijanja(string razlog)
        {
            razlogOdbijanja = razlog ?? string.Empty;
        }

        public void SetDatumOd(DateTime datum)
        {
            if (datum < DateTime.Today)
                throw new ArgumentException("Datum početka ne može biti u prošlosti");
            
            datumOd = datum;
        }

        public void SetDatumDo(DateTime datum)
        {
            if (datum < datumOd)
                throw new ArgumentException("Datum kraja ne može biti pre datuma početka");
            
            datumDo = datum;
        }

        public int GetBrojDana()
        {
            return (datumDo - datumOd).Days + 1;
        }

        public override string ToString()
        {
            return $"Rezervacija {id.Substring(0, 8)}... od {datumOd:dd.MM.yyyy} do {datumDo:dd.MM.yyyy} - Status: {status}";
        }
    }
}
