using System.Windows;

namespace HotelReservationSystem.Models
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Inicijalizacija servisa
            var serviceLocator = Helpers.ServiceLocator.Instance;
            
            // Inicijalizacija početnih podataka (administrator i test podaci)
            Helpers.InitialDataSeeder.SeedData();
        }
    }
}
