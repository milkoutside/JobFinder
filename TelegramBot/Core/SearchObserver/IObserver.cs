namespace TelegramBot.Core.SearchObserver;

public interface IObserver
{
    void Update(ISubject subject);
}