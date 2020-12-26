using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesAPI.Data;
using QuotesAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotesController : ControllerBase
    {
        private QuotesDbContext _quotesDbContext;

        public QuotesController(QuotesDbContext quotesDbContext)
        {
            this._quotesDbContext = quotesDbContext;
        }

        // GET: api/values
        //[HttpGet]
        //public IEnumerable<Quote> Get()
        //{
        //    return _quotesDbContext.Quotes;
        //}

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [AllowAnonymous]
        public IActionResult Get(string sort)
        {
            IQueryable<Quote> quotes;

            switch(sort)
            {
                case "desc":
                    quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedOn);
                    break;

                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedOn);
                    break;

                default:
                    quotes = _quotesDbContext.Quotes;
                    break;
            }

            return Ok(quotes);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var _quote = _quotesDbContext.Quotes.Find(id);
            if (_quote == null)
            {
                return NotFound("No record found");
            }

            return Ok(_quote);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            quote.UserId = userId;
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();
            return Created("Successfully saved new quote", quote);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var _quote = _quotesDbContext.Quotes.Find(id);

            if(_quote == null)
            {
                return NotFound("No record found");
            }

            if(userId != _quote.UserId)
            {
                return BadRequest("Sorry you are not authorized to update this record");
            }

            _quote.Title = quote.Title;
            _quote.Author = quote.Author;
            _quote.Description = quote.Description;
            _quotesDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status200OK);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var _quote = _quotesDbContext.Quotes.Find(id);

            if (_quote == null)
            {
                return NotFound("No record found");
            }

            if (userId != _quote.UserId)
            {
                return BadRequest("Sorry you are not authorized to update this record");
            }

            _quotesDbContext.Quotes.Remove(_quote);
            _quotesDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet("action")]
        // Route[("action")] int? -> nullable type
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _quotesDbContext.Quotes;

            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 3;

            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }
        
        [HttpGet("action")]
        public IActionResult SearchQuote(string type)
        {
            var quotes = _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(quotes);
        }

        [HttpGet("action")]
        public IActionResult MyQuote()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var _quotes = _quotesDbContext.Quotes.Where(q => q.UserId == userId);
            return Ok(_quotes);
        }
    }
}
