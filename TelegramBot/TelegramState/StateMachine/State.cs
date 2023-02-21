using MongoDB.Bson.Serialization.Attributes;

namespace JobFinder.TelegramState.StateMachine;
[BsonIgnoreExtraElements]
public class State : IState
{
    public long Id { get; set; } = 0;

    public string ActionCommand { get; set; } = "";
        
    public string StateCommand { get; set; } = "";
    
    public string Message { get; set; } = "";
}
