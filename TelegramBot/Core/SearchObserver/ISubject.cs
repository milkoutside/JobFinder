namespace TelegramBot.Core.SearchObserver;

public interface ISubject
{
    void Attach(IObserver observer, long userId);

    // Отсоединяет наблюдателя от издателя.
    void Detach(long userId);

    // Уведомляет всех наблюдателей о событии.
    void Notify(long id);
}