using CommunityToolkit.Mvvm.Input;
using mjnotas.ViewModels;
using mjnotas.Models;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace mjnotas.ViewModels;

internal class MJnotesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.MJnoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public MJnotesViewModel()
    {
        AllNotes = new ObservableCollection<ViewModels.MJnoteViewModel>(Models.MJnote.LoadAll().Select(n => new MJnoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.MJnoteViewModel>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.MJnotepage));
    }

    private async Task SelectNoteAsync(ViewModels.MJnoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.MJnotepage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            MJnoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            MJnoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new MJnoteViewModel(Models.MJnote.Load(noteId)));
        }
    }
}