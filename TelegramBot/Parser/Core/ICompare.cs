namespace TelegramBot.Parser;

public interface ICompare
{
    public Task<List<string>> CompareVacancies(List<string> currentVacancies, List<string> pastVacancies);
}