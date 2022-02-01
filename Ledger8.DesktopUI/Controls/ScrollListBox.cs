using System.Windows.Controls;

namespace Ledger8.DesktopUI.Controls;

public class ScrollListBox : ListBox
{
    public ScrollListBox() : base() => SelectionChanged += Scroll;

    private void Scroll(object sender, SelectionChangedEventArgs e)
    {
        if (e?.AddedItems is not null && e.AddedItems.Count > 0)
        {
            ScrollIntoView(e.AddedItems[0]);
        }
    }
}
