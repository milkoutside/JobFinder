using MongoDB.Bson.Serialization.Attributes;

namespace JobFinder.Models;

[BsonIgnoreExtraElements]
public class Vacancies
{
    [BsonElement("UserId")]
    public long UserId { get; set; }
    [BsonElement("Href")]
    public string Href { get; set; } = "";
    [BsonElement("Date")]
    
    public DateTime DateTime { get; set; } = DateTime.Now;
}