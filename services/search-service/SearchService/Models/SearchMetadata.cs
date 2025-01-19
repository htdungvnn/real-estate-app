namespace SearchService.Models
{
    public class SearchMetadata
    {
        public Guid Id { get; set; }
        public string PropertyId { get; set; }
        public string SearchTerm { get; set; }
        public DateTime IndexedAt { get; set; }
    }
}