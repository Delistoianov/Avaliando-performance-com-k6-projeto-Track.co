using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Api.Track.co.Data;
using Web.Api.Track.co.DTOs.NPS;
using Web.Api.Track.co.DTOs.Widget;

namespace Web.Api.Track.co.Controllers
{
    [ApiController]
    [Route("/widgets")]
    public class WidgetController : ControllerBase
    {
        private readonly ILogger<WidgetController> _logger;
        private readonly AppDbContext _context;

        public WidgetController(ILogger<WidgetController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Getting all widgets with corresponding NPS records");
                var widgets = _context.Widgets
                    .Include(w => w.Nps)
                    .ToList();
                
                return Ok(widgets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching widgets with NPS records");
                return StatusCode(500,
                    new
                    {
                        error = true, message = "Internal server error occurred while fetching widgets with NPS records"
                    });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.LogInformation($"Getting widget with ID {id} with corresponding NPS records");
                var widget = _context.Widgets
                    .Include(w => w.Nps) // Include the Nps records in the query.
                    .FirstOrDefault(w => w.Id == id);

                if (widget == null)
                {
                    return NotFound(new { message = "Widget not found" });
                }

                return Ok(widget);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching widget with ID {id} with NPS records");
                return StatusCode(500,
                    new
                    {
                        error = true,
                        message = $"Internal server error occurred while fetching widget with ID {id} with NPS records"
                    });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] WidgetCreateDto widgetDto)
        {
            try
            {
                _logger.LogInformation("Creating a new widget");

                var widget = new Widget
                {
                    Title = widgetDto.Title,
                    Link = widgetDto.Link,
                    Question = widgetDto.Question,
                    Color = "#e5e7eb",
                };

                _context.Widgets.Add(widget);
                _context.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = widget.Id }, widget);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new widget");
                return StatusCode(500,
                    new { error = true, message = "Internal server error occurred while creating a new widget" });
            }
        }

        [HttpPost("{widgetId}/nps")]
        public IActionResult PostNps(int widgetId, [FromBody] NPSCreateDto npsDto)
        {
            if (!_context.Widgets.Any(w => w.Id == widgetId))
            {
                return NotFound(new { message = "Widget not found" });
            }

            try
            {
                _logger.LogInformation($"Creating a new NPS for widget ID {widgetId}");

                var nps = new Nps
                {
                    WidgetId = widgetId,
                    Answer = npsDto.Answer,
                    Rating = npsDto.Rating
                };

                _context.Nps.Add(nps);
                _context.SaveChanges();

                return Created($"nps", nps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating a new NPS for widget ID {widgetId}");
                return StatusCode(500,
                    new
                    {
                        error = true,
                        message = $"Internal server error occurred while creating a new NPS for widget ID {widgetId}"
                    });
            }
        }

        [HttpGet("{widgetId}/nps/{npsId}")]
        public IActionResult GetNpsById(int widgetId, int npsId)
        {
            var npsRecord = _context.Nps
                .Where(n => n.WidgetId == widgetId && n.Id == npsId)
                .FirstOrDefault();

            if (npsRecord == null)
            {
                return NotFound(new { message = "NPS record not found" });
            }

            try
            {
                return Ok(npsRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError
                    (ex, $"Error occurred while fetching NPS record with ID {npsId} for widget ID {widgetId}");
                return StatusCode(500,
                    new
                    {
                        error = true,
                        message =
                            $"Internal server error occurred while fetching NPS record with ID {npsId} for widget ID {widgetId}"
                    });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] WidgetUpdateDto widgetUpdateDto)
        {
            if (widgetUpdateDto == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            var widgetFromDb = _context.Widgets.Find(id);
            if (widgetFromDb == null)
            {
                return NotFound(new { message = "Widget not found" });
            }

            // Update the properties if they are provided in the DTO
            if (!string.IsNullOrEmpty(widgetUpdateDto.Title))
            {
                widgetFromDb.Title = widgetUpdateDto.Title;
            }

            if (!string.IsNullOrEmpty(widgetUpdateDto.Link))
            {
                widgetFromDb.Link = widgetUpdateDto.Link;
            }

            if (!string.IsNullOrEmpty(widgetUpdateDto.Question))
            {
                widgetFromDb.Question = widgetUpdateDto.Question;
            }

            if (!string.IsNullOrEmpty(widgetUpdateDto.Color))
            {
                widgetFromDb.Color = widgetUpdateDto.Color;
            }

            try
            {
                _context.SaveChanges();
                return Ok(widgetFromDb);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the widget");
                return StatusCode(500,
                    new { error = true, message = "Internal server error occurred while updating the widget" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var widget = _context.Widgets.Find(id);
            if (widget == null)
            {
                return NotFound(new { message = "Widget not found" });
            }

            try
            {
                _context.Widgets.Remove(widget);
                _context.SaveChanges();
                return Ok(new { message = "Widget deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting widget with ID {id}");
                return StatusCode(500,
                    new
                    {
                        error = true, message = $"Internal server error occurred while deleting widget with ID {id}"
                    });
            }
        }
    }
}