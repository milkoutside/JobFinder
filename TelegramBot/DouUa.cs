using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBot;
[BsonNoId]  
public class DouUa : ISetting
{
    

    public string SiteName { get; } = "//Dou.ua";
    public string Path { get; set; } = "https://jobs.dou.ua/vacancies/";
    
    public string Job { get; set; }
    
    public string Category { get; set; }
    
    public string City { get; set; }
    public string XCard { get; set; }  = "//div[@class='vacancy']//a[contains(@class,'vt')]";
    public string XPublished { get; set; }



}