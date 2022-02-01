using Ledger8.Common;

namespace Ledger8.Models;

public abstract class ModelBase : NotifyBase
{
    private bool _canDelete;
    public bool CanDelete
    {
        get => _canDelete;
        set => SetProperty(ref _canDelete, value);
    }

    public ModelBase() => CanDelete = true;
}
