using SamaritanAPI.Models;

namespace SamaritanAPI.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotes();
        Task<IEnumerable<Note>> GetNotesByDonorId(int donorId);
        Task<Note?> GetNote(int id);
        Task DeleteNote(int id);
        Task CreateNote(Note note);
        Task UpdateNote(Note note);
    }
}
