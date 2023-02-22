using JobFinder.TelegramState.StateMachine;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JobFinder.Parser.Core;

public class ParserWorkUa : Parser
{
    public override async Task<List<string>> SearchVacancies(List<SettingState> settings)
    {
        List<string> result = new List<string>();

        List<string> timePublished = new List<string>()
        {
            "1 день назад", "1 день тому",
            "2 дня назад", "2 дні тому","вчера","вчора"
        };

        for (int j = 1; j < 24; j++)
        {
            switch (j % 10)
            {
                case var num when j > 5 && j < 21:
                        
                    timePublished.Add($"{j} часов назад" );
                    timePublished.Add($"{j} ч. назад" );
                    timePublished.Add($"{j} год. тому" );
                        
                    break;
                    
                case 1:
                        
                    timePublished.Add($"{j} час назад" );
                    timePublished.Add($"{j} ч. назад" );
                    timePublished.Add($"{j} год. тому" );
                        
                    break;
                    
                case 2:
                case 3:
                case 4:
                        
                    timePublished.Add($"{j} часа назад" );
                    timePublished.Add($"{j} ч. назад" );
                    timePublished.Add($"{j} год. тому" );
                        
                    break;
                    
                default:
                        
                    timePublished.Add($"{j} часов назад" );
                    timePublished.Add($"{j} ч. назад" );
                    timePublished.Add($"{j} год. тому" );
                        
                    break;
            }
      
        }

        IWebDriver _driver = new ChromeDriver();

        var repeat = 1;

        for (int i = 0; i < settings.Count; i++)
        {

            _driver.Url = settings[i].Path + $"?page={repeat}";;

            for (int k = 0; k < 30; k++)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                js.ExecuteScript("window.scrollTo(0, scrollY+350)");

                Thread.Sleep(100);

            }

            List<IWebElement> cards = _driver.FindElements(By.XPath(settings[i].XCard)).ToList();

            List<IWebElement> published = _driver.FindElements(By.XPath(settings[i].XPublished)).ToList();

            List<IWebElement> hotCards = _driver
                .FindElements(By.XPath("//div[contains(@class,'wordwrap job-link')]//div//div//span[contains(@class,'label label-orange-light')]"))
                .ToList();
            
            for (int j =  0; j < cards.Count - hotCards.Count; j++)
            {
                
                if (timePublished.Contains(published[j].Text))
                {
                    result.Add(cards[j + hotCards.Count].GetAttribute("href"));
                }
                
            }

            if (repeat < 3 && result.Count < 14)
            {
                i--;
                repeat++;
            }



        }

        _driver.Quit();
        _driver.Dispose();
        return result;
    }
}