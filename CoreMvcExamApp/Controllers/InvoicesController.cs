using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreMvcExamApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoreMvcExamApp.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly StoreContext _context;

        public InvoicesController(StoreContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {

            var data = await _context.Invoices.Include(i=> i.Items).ToListAsync();


            ViewBag.Count = data.Count;
            ViewBag.GrandTotal = data.Sum(i=> i.Items.Sum(l=> l.ItemTotal));

            //ViewBag.Average = data.Average(i=> i.Items.Sum(l=> l.ItemTotal)) ;

            ViewBag.Average = data.Count>0? data.Average(i=> i.Items.Sum(l=> l.ItemTotal)) : 0;



			return View(data);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.Include(i => i.Items)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
			}




			return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View(new Invoice());
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(" InvoiceDate,CustomerName,Address,ContactNo, Items")] Invoice invoice, string command = "")
        {
            if (command == "Add")
            {
                invoice.Items.Add(new ());
				return View(invoice);
			}
            else if (command.Contains( "delete"))// delete-3-sdsd-5   ["delete", "3"]
			{
                int idx = int.Parse(command.Split( '-')[1]);

                invoice.Items.RemoveAt(idx);
				return View(invoice);
			}

			if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.Include(i=> i.Items).FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceDate,CustomerName,Address,ContactNo, Items")] Invoice invoice, string command = "")
        {
            if (command == "Add")
            {
                invoice.Items.Add(new());
                return View(invoice);
            }
            else if (command.Contains("delete"))// delete-3-sdsd-5   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                invoice.Items.RemoveAt(idx);
                return View(invoice);
            }
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.Include(i => i.Items)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var invoice = await _context.Invoices.FindAsync(id);
            //if (invoice != null)
            //{             

            //    //_context.Invoices.Remove(invoice);
            //}
            await _context.Database.ExecuteSqlAsync($"exec spDeleteInvoice {id}");

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
