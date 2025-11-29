namespace HotelReservationSystem.Models
{
    public enum KorisnikTip
    {
        Administrator,
        Gost,
        Vlasnik
    }
    
    public enum StatusRezervacije
    {
        NaCekanju,
        Potvrdjeno,
        Odbijeno,
        Otkazano
    }
    
    public enum StatusHotela
    {
        NaCekanju,
        Odobren
    }
}
