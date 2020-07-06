using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Domain.Adapter;
using EmailValidation;
using infrastructure;

namespace FromYourPastSelf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageFromPastSelvesController : ControllerBase
    {
        private readonly MailContext _context;
        private readonly Mail _mailService;


        public MessageFromPastSelvesController(MailContext context,Mail mail)
        {
            _context = context;
            _mailService = mail;
            context.Database.EnsureCreated();
        }

        // GET: api/MessageFromPastSelves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageFromPastSelf>>> Getmessages()
        {
            return await _context.messages.ToListAsync();
        }

        // GET: api/MessageFromPastSelves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageFromPastSelf>> GetMessageFromPastSelf(long id)
        {
            var messageFromPastSelf = await _context.messages.FindAsync(id);

            if (messageFromPastSelf == null)
            {
                return NotFound();
            }

            return messageFromPastSelf;
        }

        // PUT: api/MessageFromPastSelves/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessageFromPastSelf(long id, MessageFromPastSelf messageFromPastSelf)
        {
            if (id != messageFromPastSelf.Id||!EmailValidator.Validate ( messageFromPastSelf.To)||messageFromPastSelf.Body.Length<1)
            {
                return BadRequest();
            }
            var jobid=  _mailService.updateEmail(messageFromPastSelf);
            messageFromPastSelf.jobid = jobid;
            _context.Entry(messageFromPastSelf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageFromPastSelfExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MessageFromPastSelves
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MessageFromPastSelf>> PostMessageFromPastSelf(
            MessageFromPastSelfDto messageFromPastSelfdto)
        {
            if (!EmailValidator.Validate ( messageFromPastSelfdto.To)||messageFromPastSelfdto.Body.Length<1)
            {
                return BadRequest();
            }

            MessageFromPastSelf messageFromPastSelf = new MessageFromPastSelf
            {
                Body = messageFromPastSelfdto.Body,
                IsBodyHtml = messageFromPastSelfdto.IsBodyHtml,
                Subject = messageFromPastSelfdto.Subject,
                To = messageFromPastSelfdto.To,
                when = messageFromPastSelfdto.when
            };
            _context.messages.Add(messageFromPastSelf);
            await _context.SaveChangesAsync();
           var jobid=  _mailService.scheduleEmail(messageFromPastSelf);
           messageFromPastSelf.jobid = jobid;
            return CreatedAtAction("GetMessageFromPastSelf", new {id = messageFromPastSelf.Id}, messageFromPastSelf);
        }

        // DELETE: api/MessageFromPastSelves/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MessageFromPastSelf>> DeleteMessageFromPastSelf(long id)
        {
            var messageFromPastSelf = await _context.messages.FindAsync(id);
            if (messageFromPastSelf == null)
            {
                return NotFound();
            }
            _mailService.deleteEmail(messageFromPastSelf);
            _context.messages.Remove(messageFromPastSelf);
            await _context.SaveChangesAsync();

            return messageFromPastSelf;
        }

        private bool MessageFromPastSelfExists(long id)
        {
            return _context.messages.Any(e => e.Id == id);
        }
    }
}