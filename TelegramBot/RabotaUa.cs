using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBot;
[BsonNoId]  
public class RabotaUa : ISetting
{
   

    public string SiteName { get;} = "//Rabota.ua";

    
    private string _category;
 
    
    public string Path { get; set; } = "https://rabota.ua/ua/zapros/";

    public string Job {get; set; }

    public string Category { get; set; } 

    public string City { get; set; }


    public string XCard { get; set; } = "//alliance-jobseeker-desktop-vacancies-list//a[contains(@href, '/company')][contains(@class,'card')]";

    public string XPublished { get; set; } =
        "//div[contains(@class, 'santa-flex santa-justify-between santa-items-center')]//div[contains(@class, 'santa-typo-secondary santa-text-black-500')]";

}