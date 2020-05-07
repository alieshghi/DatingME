using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApi.Models;
using  Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ToDoContext _context;              

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,ToDoContext context)
        {
            _logger = logger;
            _context =context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var toDoItems = await _context.TodoItems.ToListAsync();
            return Ok(toDoItems);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async  Task<IActionResult> Get(long id)
        {
            var toDoItem = await _context.TodoItems.FirstOrDefaultAsync(x=>x.Id==id);
            return Ok(toDoItem);
        }
    }
}
