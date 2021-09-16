using System;

namespace shopify_image_repository.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public int UserId { get; set; }
        public string blobId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageDescription { get; set; }
        public string Location { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsPublic { get; set; }
    }
}