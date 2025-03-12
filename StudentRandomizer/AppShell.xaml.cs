namespace StudentRandomizer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.AddClass), typeof(Views.AddClass));
            Routing.RegisterRoute(nameof(Views.AddStudent), typeof(Views.AddStudent));
            Routing.RegisterRoute(nameof(Views.StudentList), typeof(Views.StudentList));
        }
    }
}
