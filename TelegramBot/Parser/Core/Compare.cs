namespace TelegramBot.Parser;

public class Compare : ICompare
{
    public async Task<List<string>> CompareVacancies(List<string> currentVacancies, List<string> pastVacancies)
    {
        List<string> vacancies = new List<string>();

        for (int i = 0; i < currentVacancies.Count; i++)
        {
            if (!pastVacancies.Contains(currentVacancies[i])) 
                vacancies.Add(currentVacancies[i]);
                
        }

        return vacancies;
    }
}