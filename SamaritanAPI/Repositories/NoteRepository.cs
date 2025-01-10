using Microsoft.EntityFrameworkCore;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext context;

        public NoteRepository(ApplicationDbContext context)
            => this.context = context;

        public async Task CreateNote(Note note)
        {
           await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();
        }

        public async Task DeleteNote(int id)
        {
            var note = await context.Notes.FirstOrDefaultAsync(not => not.Id == id);
            if (note != null)
                context.Notes.Remove(note);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
            => await context.Notes.ToListAsync();

        public async Task<Note?> GetNote(int id)
            => await context.Notes.FindAsync(id);

        public async Task<IEnumerable<Note>> GetNotesByDonorId(int donorId)
            => await context.Notes.Where(note => note.DonorId == donorId).ToListAsync();

        public async Task UpdateNote(Note note)
        {
            context.Notes.Update(note);
            await context.SaveChangesAsync();
        }
    }
}
