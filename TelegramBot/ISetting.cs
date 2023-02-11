namespace TelegramBot;

public interface ISetting
{

    public string SiteName { get; }
    public string Path { get; set; }
    
    public string Job { get; set; }
    
    public string Category { get; set; }
    public string City { get; set; }
    public string XCard { get; set; }
    
    public string XPublished { get; set; }
    
}