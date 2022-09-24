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
        private const string ErrorView = "Error";
        public NotesController(BlogContext context)
        {
            _context = context;
        }

        // GET: Notes
        public IActionResult Index(string? sortedBy, int? order)
        {
            if(_context.Note == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }

            var id = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(id == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }

            ViewBag.sortedBy = sortedBy;
            ViewBag.order = order;

            if(sortedBy == null)
                return View(_context.Note.Where(n=>n.UserId == id.Value));
            
            if(order == null)
                order = 1;

            if(sortedBy == "date")
                return View(_context.Note.Where(n=>n.UserId == id.Value).OrderBy(m=>order*m.CreationDate.Ticks));
            
            if(order == 1)
                return View(_context.Note.Where(n=>n.UserId == id.Value).OrderBy(m=>m.Title.ToLower()));

            return View(_context.Note.Where(n=>n.UserId == id.Value).OrderByDescending(m=>m.Title.ToLower()));
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(_context.Note == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }

            if (id == null){
                Response.StatusCode = CodeError.BadRequest;
                return View(ErrorView, NoteErrorViewModel.BadRequest());
            }
            
            var note = await _context.Note.FindAsync(id);

            if (note == null){
                Response.StatusCode = CodeError.NotFound;
                return View(ErrorView, NoteErrorViewModel.NotFound((int)id));
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(currentUserId == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }  

            if(currentUserId.Value != note.UserId){
                Response.StatusCode = CodeError.NotAuthorized;
                return View(ErrorView, NoteErrorViewModel.NotAuthorized((int)id));
            }
            
            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(id == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }

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
            
            if(currentUserid == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }
            
            if(currentUserid.Value != note.UserId){
                Response.StatusCode = CodeError.NotAuthorized;
                return View(ErrorView, NoteErrorViewModel.NotAuthorized(note.Id));
            }

            if (ModelState.IsValid)
            {
                note.CreationDate = DateTime.Now;
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
            var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(currentUserid == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }

            if(_context.Note == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }

            if (id == null){
                Response.StatusCode = CodeError.BadRequest;
                return View(ErrorView, NoteErrorViewModel.BadRequest());
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null){
                Response.StatusCode = CodeError.NotFound;
                return View(ErrorView, NoteErrorViewModel.NotFound((int)id));
            }

            if(currentUserid.Value != note.UserId){
                Response.StatusCode = CodeError.NotAuthorized;
                return View(ErrorView, NoteErrorViewModel.NotAuthorized(note.Id));
            }
            
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
                Response.StatusCode = CodeError.NotFound;
                return View(ErrorView, NoteErrorViewModel.NotFound((int)id));
            }

            var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(currentUserid == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }
            
            if(currentUserid.Value != note.UserId){
                Response.StatusCode = CodeError.NotAuthorized;
                return View(ErrorView, NoteErrorViewModel.NotAuthorized(note.Id));
            }

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
                        Response.StatusCode = CodeError.NotFound;
                        return View(ErrorView, NoteErrorViewModel.NotFound(note.Id));
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
            if(_context.Note == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }

            if (id == null){
                Response.StatusCode = CodeError.BadRequest;
                return View(ErrorView, NoteErrorViewModel.BadRequest());
            }

            var note = await _context.Note.FindAsync(id);

            if (note == null){
                Response.StatusCode = CodeError.NotFound;
                return View(ErrorView, NoteErrorViewModel.NotFound((int)id));
            }
            
            var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if(currentUserid == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }
            
            if(note.UserId != currentUserid.Value){
                Response.StatusCode = CodeError.NotAuthorized;
                return View(ErrorView, NoteErrorViewModel.NotAuthorized(note.Id));
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Note == null){
                Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
            }
            
            var note = await _context.Note.FindAsync(id);

            if (note != null)
            {
                var currentUserid = User.FindFirst(ClaimTypes.NameIdentifier);
            
                if(currentUserid == null){
                    Response.StatusCode = CodeError.Internal;
                return View(ErrorView, NoteErrorViewModel.Internal());
                }

                if(currentUserid.Value != note.UserId){
                    Response.StatusCode = CodeError.NotAuthorized;
                return View(ErrorView, NoteErrorViewModel.NotAuthorized(note.Id));
                }

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
