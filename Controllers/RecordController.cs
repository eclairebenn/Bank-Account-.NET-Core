using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bank_account.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace bank_account.Controllers
{
    public class RecordController : Controller
    {
        private BankContext _context;
    
        public RecordController(BankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("account/{userid}")]
        public IActionResult Account(int userid)
        {
            int? id = HttpContext.Session.GetInt32("userid");
            if(userid != id || id == null)
            {
                return RedirectToAction("UserLogin", "User");
            }
            List<Record> records = _context.Records.Where(r => r.UserId == userid).OrderByDescending(r => r.CreatedAt).Include(rec => rec.User).ToList();
            ViewBag.Records = records;
            User user = _context.Users.SingleOrDefault(u => u.UserId == userid);
            ViewBag.User = user;
            return View("Account");
        }

        [HttpPost]
        [Route("add/record")]
        public IActionResult AddRecord(Record record)
        {
            if(ModelState.IsValid)
            {
                User Customer = _context.Users.SingleOrDefault(user => user.UserId == record.UserId);
                if(Customer.Balance + record.Amount < 0)
                {
                    TempData["balance"] = "You cannot withdraw more than your current balance";
                }
                else
                {
                    _context.Records.Add(record);
                    Customer.Balance = Customer.Balance + record.Amount;
                    _context.SaveChanges();
                    return Redirect($"/account/{record.UserId}");
                }
            }
            return Redirect($"/account/{record.UserId}");
        }
    }
}