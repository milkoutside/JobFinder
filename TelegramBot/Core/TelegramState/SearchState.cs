using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBot.Core.TelegramState;
[BsonIgnoreExtraElements]
public class SearchState
{   
    [BsonElement("UserId")]
    public long UserId { get; set; } = 0;
    
    [BsonElement("CancellationTokenSource")]
    public CancellationTokenSource _cts;
    public string IsWork { get; set; } = "false";

}