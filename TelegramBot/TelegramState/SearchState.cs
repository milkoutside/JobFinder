using MongoDB.Bson.Serialization.Attributes;

namespace JobFinder.TelegramState;

[BsonIgnoreExtraElements]
public class SearchState
{   
    [BsonElement("UserId")]
    public long UserId { get; set; } = 0;
    
    public string IsWork { get; set; } = "false";

}