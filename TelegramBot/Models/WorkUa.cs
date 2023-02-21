using MongoDB.Bson.Serialization.Attributes;

namespace JobFinder.Models;
[BsonNoId]  
public class WorkUa : ISetting
{


    public string SiteName { get; } = "//Work.ua";

    public string Path { get; set; } = "https://www.work.ua/ru/jobs-";
    
    public string Job { get; set; }

    public string Category { get; set; } 

    public string City { get; set; }

    public string XCard { get; set; } = "//div[contains(@class,'wordwrap job-link')]//h2//a";
    public string XPublished { get; set; } = "//div[contains(@class,'wordwrap job-link')]//div//span[contains(@class,'text-muted small')]";


}