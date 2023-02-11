namespace TelegramBot.Core.TelegramState.StateMachine
{
    public interface IStateMachine
    {
        Task SetState(State state);
        Task UpdateState(State state);
    }
}