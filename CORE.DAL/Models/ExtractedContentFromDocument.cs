namespace CORE.DAL.Models
{
    public class ExtractedContentFromDocument
    {
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public int Department { get; set; }
        public int Stakeholder { get; set; }
        public int Event { get; set; }
        public int Location { get; set; }
        public int Well { get; set; }
        public bool IsAbleToExtract { get; set; }
        public string Content { get; set; }
    }
}