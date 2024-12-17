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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllImages()
        {
            var images = _imageRepositoryDB.GetAll().Select(image => new
            {
                image.Id,
                image.Name,
                Timestamp = image.Timestamp,
                Data = Convert.ToBase64String(image.Data) // Konverter til base64
            });

            return Ok(images);
        }


        // GET api/<ImagesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var image = _imageRepositoryDB.GetById(id);
            if (image == null)
            {
                return NotFound("Image not found.");
            }

            return File(image.Data, "image/jpeg"); // Returnér binære data som billede
        }

        // POST api/<ImagesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string name)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var image = new Models.Image
                {
                    Name = name,
                    Data = memoryStream.ToArray(), // Gem som binary data
                    Timestamp = DateTime.Now
                };

                _imageRepositoryDB.Add(image); // Gem i databasen
                return CreatedAtAction(nameof(GetById), new { id = image.Id }, image);
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
