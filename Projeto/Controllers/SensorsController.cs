using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto;
using Projeto.Data;

namespace Projeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly ProjetoContext _context;

        public SensorsController(ProjetoContext context) {
            _context = context;
        }

        // GET: api/Sensors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensor() {
            return await _context.Sensor.ToListAsync();
        }

        // GET: api/Sensors/data1/data2
        [HttpGet("{stringStart}/{stringEnd}")]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensor(string stringStart, string stringEnd) {
            // As datas vêm no formato 'MM/dd/yyyy HH:mm:ss', porém a URL não reconhece '/' e troca por '%2F'
            stringStart = HttpUtility.UrlDecode(stringStart);
            stringEnd = HttpUtility.UrlDecode(stringEnd);

            // Converte as datas formatadas em string para DateTime
            DateTime dateStart = DateTime.ParseExact(stringStart, "MM/dd/yyyy HH:mm:ss", null);
            DateTime dateEnd = DateTime.ParseExact(stringEnd, "MM/dd/yyyy HH:mm:ss", null);

            var sensors = await _context.Sensor.ToListAsync();           

            if (sensors == null)
                return NotFound();
            
            if (dateStart > dateEnd)
                return BadRequest("As datas precisam ser válidas.");

            List<Sensor> returnSensors = [];

            foreach(Sensor sensor in sensors)
                if(dateStart <= DateTime.ParseExact(sensor.Date, "MM/dd/yyyy HH:mm:ss", null) && DateTime.ParseExact(sensor.Date, "MM/dd/yyyy HH:mm:ss", null) <= dateEnd)
                    returnSensors.Add(sensor);

            return returnSensors;
        }

        // PUT: api/Sensors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensor(int id, Sensor sensor) {
            if (id != sensor.Id)
                return BadRequest();

            _context.Entry(sensor).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!SensorExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Sensors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensor) {
            _context.Sensor.Add(sensor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSensor", new { id = sensor.Id }, sensor);
        }

        // DELETE: api/Sensors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id) {
            var sensor = await _context.Sensor.FindAsync(id);
            if (sensor == null)
                return NotFound();

            _context.Sensor.Remove(sensor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorExists(int id) {
            return _context.Sensor.Any(e => e.Id == id);
        }
    }
}
