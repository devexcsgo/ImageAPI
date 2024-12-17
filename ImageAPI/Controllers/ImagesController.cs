using ImageAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ImageRepositoryDB _imageRepositoryDB;

        public ImagesController(ImageRepositoryDB imageRepositoryDB)
        {
            _imageRepositoryDB = imageRepositoryDB;
        }

        // GET ALL: api/<ImagesController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<IEnumerable<Models.Image>> GetAll()
        {
            try
            {
                IEnumerable<Models.Image> images = _imageRepositoryDB.GetAll();
                if (images == null || !images.Any())
                {
                    return NotFound("No images found.");
                }
                return Ok(images);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // GET api/<ImagesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Models.Image> Get(int id)
        {
            Models.Image? image = _imageRepositoryDB.GetById(id);
            if (image == null)
            {
                return NotFound();
            }
            return Ok(image);
        }

        // POST api/<ImagesController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Models.Image>> Post([FromForm] IFormFile file, [FromForm] string name)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                // Læs filen som binære data
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }

                // Opret nyt billede-objekt
                var newImage = new Models.Image
                {
                    Name = name,
                    Path = file.FileName, // Filnavnet kan også gemmes i Path
                    Data = fileData,      // Gem binære data i databasen
                    Timestamp = DateTime.Now
                };

                // Gem i databasen
                var createdImage = _imageRepositoryDB.Add(newImage);
                return Created("/" + createdImage.Id, createdImage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // PUT api/<ImagesController>/5
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public ActionResult<Models.Image> Put(int id, [FromBody] Models.Image updatedImage)
        {
            try
            {
                Models.Image? image = _imageRepositoryDB.GetById(id);
                if (image == null)
                {
                    return NotFound();
                }
                updatedImage.Id = id;
                Models.Image newImage = _imageRepositoryDB.Update(updatedImage);
                return Ok(newImage);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ImagesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public ActionResult<Models.Image> Delete(int id)
        {
            if (_imageRepositoryDB.GetById(id) == null)  
            {
                return NotFound();
            }
            Models.Image? image = _imageRepositoryDB.Remove(id);
            return Ok(image);
        }
    }
}
