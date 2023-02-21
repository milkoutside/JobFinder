namespace JobFinder.Parser.Core;

public class Compare : ICompare
{
    public async Task<List<string>> CompareVacancies(List<string> currentVacancies, List<string> pastVacancies)
    {
        return currentVacancies.Where(t => !pastVacancies.Contains(t)).ToList();
    }
}