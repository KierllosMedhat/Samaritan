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
        private readonly INoteRepository noteRepository;

        public NoteController(INoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await noteRepository.GetAllNotes();
            return Ok(notes);
        }

        [HttpGet("donor/{donorId}")]
        public async Task<ActionResult<List<Note>>> GetNotesByDonorId(int donorId)
        {
            var notes = await noteRepository.GetNotesByDonorId(donorId);
            if (notes == null)
            {
                return NotFound();
            }
            return Ok(notes);
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetNote(int noteId)
        {
            var note = await noteRepository.GetNote(noteId);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            if(ModelState.IsValid)
            {
                await noteRepository.CreateNote(note);
                return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
            }
            ModelState.AddModelError("","Invalid Format!");
            return BadRequest(ModelState);
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(int noteId, [FromBody] Note note)
        {
            if(ModelState.IsValid)
            {
                var existingNote = await noteRepository.GetNote(noteId);
                if (existingNote == null)
                    return NotFound();
                await noteRepository.UpdateNote(note);
                return Ok(note);
            }
            ModelState.AddModelError("","Invalid Format!");
            return BadRequest(ModelState);
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