namespace ImageAPI.Repository
{
    public class ImageRepositoryDB
    {
        private ImageDbContext _context;

        public ImageRepositoryDB(ImageDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Models.Image> GetAll()
        {
            IQueryable<Models.Image> query = _context.Images.ToList().AsQueryable();
            return query;
        }
        public Models.Image? GetById(int id)
        {
            return _context.Images.Find(id);
        }
        public Models.Image Add(Models.Image image)
        {
            image.Timestamp = DateTime.Now;
            image.Id = 0;
            //image.Path = "https://www.silasstilling.dk/images/" + image.Name; // For later use
            _context.Images.Add(image);
            _context.SaveChanges();
            return image;
        }

        public Models.Image Update(Models.Image image)
        {
            _context.Images.Update(image);
            _context.SaveChanges();
            return image;
        }
        public Models.Image? Remove(int id)
        {
            Models.Image? image = _context.Images.Find(id);
            if (image == null)
            {
                return null;
            }
            _context.Images.Remove(image);
            _context.SaveChanges();
            return image;
        }
    }
}
