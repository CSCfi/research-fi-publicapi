namespace Api
{
    public class Publication
    {
        public string Name { get; set; } = $"Julkaisu {Random.Shared.Next(1, 100)}";
    }
}