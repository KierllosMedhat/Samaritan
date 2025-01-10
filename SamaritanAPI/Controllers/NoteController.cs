using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Data;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly INoteRepository noteRepository;

        public NoteController(ApplicationDbContext context, INoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> GetAllNotes()
        {
            var notes = await noteRepository.GetAllNotes();
            return Ok(notes);
        }

        [HttpGet($"{{donorId}}")]
        public async Task<ActionResult<List<Note>>> GetNotesByDonorId(int donorId)
        {
            var notes = await noteRepository.GetNotesByDonorId(donorId);
            if (notes == null)
            {
                return NotFound();
            }
            return Ok(notes);
        }

        [HttpGet($"{{noteId}}")]
        public async Task<ActionResult<Note>> GetNote(int noteId)
        {
            var note = await noteRepository.GetNote(noteId);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] Note note)
        {
            await noteRepository.CreateNote(note);
            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
        }

        [HttpPut("{noteId}")]
        public async Task<ActionResult<Note>> UpdateNote(int noteId, [FromBody] Note note)
        {
            var existingNote = await noteRepository.GetNote(noteId);
            if (existingNote == null)
            {
                return NotFound();
            }
            await noteRepository.UpdateNote(note);
            return Ok(note);
        }

        [HttpDelete("{noteId}")]
        public async Task<ActionResult> DeleteNote(int noteId)
        {
            var existingNote = await noteRepository.GetNote(noteId);
            if (existingNote == null)
            {
                return NotFound();
            }
            await noteRepository.DeleteNote(noteId);
            return NoContent();
        }
    }
}