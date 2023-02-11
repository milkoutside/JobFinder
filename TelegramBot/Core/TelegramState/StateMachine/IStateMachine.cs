using System.Threading.Tasks;
using TelegramBot.Core.TelegramState.StateMachine;

namespace StateMachineBot
{
    public interface IStateMachine
    {
        Task SetState(State state);
        Task UpdateState(State state);
    }
}