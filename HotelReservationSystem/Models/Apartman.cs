namespace HotelReservationSystem.Models
{
    public class Apartman
    {
        private string ime;
        private string opis;
        private int brojSoba;
        private int maxBrojGostiju;
        private string sifraHotela;
        
        public Apartman()
        {
            ime = "";
            opis = "";
            brojSoba = 0;
            maxBrojGostiju = 0;
            sifraHotela = "";
        }
        
        public Apartman(string ime, string opis, int brojSoba, int maxBrojGostiju, string sifraHotela)
        {
            this.ime = ime;
            this.opis = opis;
            this.brojSoba = brojSoba;
            this.maxBrojGostiju = maxBrojGostiju;
            this.sifraHotela = sifraHotela;
        }
        
        public string GetIme() => ime;
        public string GetOpis() => opis;
        public int GetBrojSoba() => brojSoba;
        public int GetMaxBrojGostiju() => maxBrojGostiju;
        public string GetSifraHotela() => sifraHotela;
        
        public void SetOpis(string opis) => this.opis = opis;
    }
}
