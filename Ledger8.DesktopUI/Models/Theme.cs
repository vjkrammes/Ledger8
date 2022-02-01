namespace Ledger8.DesktopUI.Models;

internal class Theme
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Alt0 { get; set; }
    public string Alt1 { get; set; }
    public string Background { get; set; }
    public string Border { get; set; }
    public string Foreground { get; set; }

    public Theme()
    {
        Name = string.Empty;
        Description = string.Empty;
        Alt0 = "White";
        Alt1 = "White";
        Background = "Black";
        Border = "Black";
        Foreground = "White";
    }
}
