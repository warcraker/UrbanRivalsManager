namespace Warcraker.UrbanRivals.Interfaces
{
    public interface ISettingsManager
    {
        string AccessKey { get; set; }
        string AccessSecret { get; set; }
        string ConsumerKey { get; set; }
        string ConsumerSecret { get; set; }
        string Language { get; set; }
    }
}
