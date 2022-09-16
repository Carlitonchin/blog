using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using blog.Context;
using blog.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace blog.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly BlogContext _context;

        public NotesController(BlogContext context)
        {
            _context = context;
        }

        // GET: Notes
        public IActionResult Index()
        {
            if(_context.Note == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var id = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(id == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return View(_context.Note.Where(n=>n.UserId == id.Value));
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(_context.Note == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (id == null)
                return StatusCode(StatusCodes.Status400BadRequest);
            
            var note = await _context.Note.FindAsync(id);

            if (note == null)
                return NotFound();

            // TODO: verify if current user is the note author    
            
            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(id == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            ViewBag.UserId = id.Value;

            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Body,UserId")] Note note)
        {
            var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(currentUserid == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            
            if(currentUserid.Value != note.UserId)
                return StatusCode(StatusCodes.Status401Unauthorized); //a user cannot create a note on behalf of another user

            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserId = currentUserid.Value;
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(_context.Note == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (id == null)
                return NotFound();

            var note = await _context.Note.FindAsync(id);
            if (note == null)
                return NotFound();
            
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,UserId")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(currentUserid == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            
            if(currentUserid.Value != note.UserId)
                return StatusCode(StatusCodes.Status401Unauthorized);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(_context.Note == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (id == null)
                return NotFound();

            var note = await _context.Note.FindAsync(id);

            if (note == null)
                return NotFound();
            
            var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(currentUserid == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            
            if(note.UserId != currentUserid.Value)
                return StatusCode(StatusCodes.Status401Unauthorized);

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Note == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            
            var note = await _context.Note.FindAsync(id);

            if (note != null)
            {
                var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
                if(currentUserid == null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                if(currentUserid.Value != note.UserId)
                    return StatusCode(StatusCodes.Status401Unauthorized);

                _context.Note.Remove(note);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
          return (_context.Note?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
