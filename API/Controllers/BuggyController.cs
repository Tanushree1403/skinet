using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController: BaseApiController
    {
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest(){
            var thing= _context.Products.Find(33);
            if(thing==null){
                return NotFound(new ApiResponse(400));
            }
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError(){
            var thing= _context.Products.Find(33);
            var thingToReturn= thing.ToString();
            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult BadRequest(){
                return BadRequest();
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequrst(int id){
            return Ok();
        }
    }
}