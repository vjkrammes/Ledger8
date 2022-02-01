using Ledger8.Common;
using Ledger8.Common.Interfaces;
using Ledger8.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ledger8.DesktopUI.Models;

public class DisplayedAccountModel : NotifyBase
{
    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private string? _accountType;
    public string AccountType
    {
        get => _accountType!;
        set => SetProperty(ref _accountType, value);
    }

    private string? _dueDate;
    public string DueDate
    {
        get => _dueDate!;
        set => SetProperty(ref _dueDate, value);
    }

    private bool _isPayable;
    public bool IsPayable
    {
        get => _isPayable;
        set => SetProperty(ref _isPayable, value);
    }

    private bool _isClosed;
    public bool IsClosed
    {
        get => _isClosed;
        set => SetProperty(ref _isClosed, value);
    }

    private DateTime _closedDate;
    public DateTime ClosedDate
    {
        get => _closedDate;
        set => SetProperty(ref _closedDate, value);
    }

    private string? _comments;
    public string Comments
    {
        get => _comments!;
        set => SetProperty(ref _comments, value);
    }

    private string? _tag;
    public string Tag
    {
        get => _tag!;
        set => SetProperty(ref _tag, value);
    }

    private string? _description;
    public string Description
    {
        get => _description!;
        set => SetProperty(ref _description, value);
    }

    private bool _canDelete;
    public bool CanDelete
    {
        get => _canDelete;
        set => SetProperty(ref _canDelete, value);
    }

    public DisplayedAccountModel()
    {
        Id = 0;
        AccountType = string.Empty;
        DueDate = string.Empty;
        IsPayable = true;
        IsClosed = false;
        ClosedDate = default;
        Comments = string.Empty;
        Tag = string.Empty;
        Description = string.Empty;
        CanDelete = true;
    }

    public DisplayedAccountModel(AccountModel model, string password, IStringCypherService cypher)
    {
        Id = model.Id;
        AccountType = model.AccountType?.Description ?? "Unknown";
        DueDate = model.DueDate();
        IsPayable = model.IsPayable;
        IsClosed = model.IsClosed;
        ClosedDate = model.ClosedDate;
        Comments = model.Comments;
        Tag = model.Tag;
        Description = model.Description(password, cypher);
        CanDelete = model.CanDelete;
    }

    public static IEnumerable<DisplayedAccountModel> FromModels(IEnumerable<AccountModel> models, string password, IStringCypherService cypher) =>
        models.Select(x => new DisplayedAccountModel(x, password, cypher)).ToList();
}
