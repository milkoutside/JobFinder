using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace StateMachineBot
{
    public interface IState
    {
        public long Id { get; set; }
        
        public string ActionCommand { get; set; }
        
        public string StateCommand { get; set; }
        public string Message { get; set; }
       
    }
}