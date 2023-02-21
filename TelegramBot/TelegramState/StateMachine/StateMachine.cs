using JobFinder.Data;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JobFinder.TelegramState.StateMachine
{
    [BsonIgnoreExtraElements]
    public class StateMachine : IStateMachine
    {

        private readonly DataContext _context;

        public StateMachine(DataContext context)
        {
            _context = context;
        }
        

        public async Task SetState(State state)
        {
            await _context.StateMachine.InsertOneAsync(state);
        }

        public async Task UpdateState(State state)
        {
            await _context.StateMachine.ReplaceOneAsync((s => s.Id == state.Id), state);
        }

        public async Task DeleteState(long userId)
        {
            await _context.StateMachine.DeleteOneAsync((s => s.Id == userId));
        }
    }
}