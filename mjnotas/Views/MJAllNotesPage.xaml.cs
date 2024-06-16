namespace mjnotas.Views;

public partial class MJAllNotesPage : ContentPage
{
    public MJAllNotesPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
}