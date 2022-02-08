namespace Api.Models
{
    public class Author
    {
        public string? sectorId { get; set; }
        public string? nameFiSector { get; set; }
        public string? nameEnSector { get; set; }
        public string? nameSvSector { get; set; }
        public Organization[]? organization { get; set; }

    }
}
