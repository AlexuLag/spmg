using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using spmg.API.Data;
using spmg.API.Models;

namespace spmg.API.Controllers
{
    [Authorize]
     [Route("api/[controller]")]
    [ApiController]   
    public class ValuesController:ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult>  GetValues()
        {
        var values = await _context.Values.ToListAsync();
        return  new OkObjectResult( values);

        
        }

    [HttpGet("{id}")]
    [AllowAnonymous]
        public  async Task<IActionResult> GetValue(int id)
    {
        var value = await _context.Values.FirstOrDefaultAsync(p=>p.Id==id);

        return new OkObjectResult(value);


    }


    }
}