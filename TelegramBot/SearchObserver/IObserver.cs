namespace JobFinder.SearchObserver;

public interface IObserver
{
    Task Update(ISubject subject);
}