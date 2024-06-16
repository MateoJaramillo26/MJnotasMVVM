namespace mjnotas
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.MJnotepage), typeof(Views.MJnotepage));
        }
    }
}
