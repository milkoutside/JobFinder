namespace JobFinder.SearchObserver;

public class Search : ISubject
{
    public bool State { get; set; } = false;
    



    private static List<SearchInfo> _searchInfos = new List<SearchInfo>();
    public void Attach(IObserver observer,long id)
    {
        _searchInfos.Add(new SearchInfo(){Observers = observer,UserId = id });
    }

    public void Detach(long id)
    {
        foreach (var obs in _searchInfos.Where(obs => obs.UserId == id))
        {
            _searchInfos.Remove(obs);
        }
    }


    public void Notify(long id)
    {
        foreach (var observer in _searchInfos.Where(observer => observer.UserId == id))
            observer.Observers.Update(this);
    }

 
}