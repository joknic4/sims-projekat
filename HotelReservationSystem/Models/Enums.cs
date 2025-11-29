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
    
    // Dodato za potrebe praćenja statusa hotela
    public enum StatusHotela
    {
        NaCekanju,
        Odobren
    }
}
