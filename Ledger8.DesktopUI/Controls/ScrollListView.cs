using System.Windows.Controls;

namespace Ledger8.DesktopUI.Controls;

public class ScrollListView : ListView
{
    public ScrollListView() : base() => SelectionChanged += Scroll;

    private void Scroll(object sender, SelectionChangedEventArgs e)
    {
        if (e?.AddedItems is not null && e.AddedItems.Count > 0)
        {
            ScrollIntoView(e.AddedItems[0]);
        }
    }
}
