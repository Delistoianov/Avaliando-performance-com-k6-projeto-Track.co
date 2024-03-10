namespace Web.Api.Track.co.Data;

public class Widget
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Link { get; set; }
    
    public string Question { get; set; }
    
    public string Color { get; set; } = "#e5e7eb";
    
    public List<Nps> Nps { get; set; } = new List<Nps>();
}
