namespace mysecondshop.Models.ViewModels
{
    public class ImageViewModel
    {
        public string ImageCaption { set; get; }
        public string ImageDescription { set; get; }
        public IFormFile MyImage { set; get; }
    }
}
