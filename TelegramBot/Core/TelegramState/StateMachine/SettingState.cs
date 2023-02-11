using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBot.Core.TelegramState.StateMachine;

[BsonIgnoreExtraElements]
public class SettingState : ISetting
{
    [BsonElement("UserId")]
    public long UserId { get; set; }

    [BsonElement("SiteName")]
    public string SiteName { get; set; }

    [BsonElement("Path")]
    public string Path { get; set; }
    [BsonElement("Job")]
    public string Job { get; set; }
    [BsonElement("Category")]
    public string Category { get; set; }
    [BsonElement("City")]
    public string City { get; set; }
    [BsonElement("XCard")]
    public string XCard { get; set; } 
    [BsonElement("XPublished")]
    public string XPublished { get; set; } 
}