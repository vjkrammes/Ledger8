using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;

using System;
using System.Windows;
using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class PopupViewModel : ViewModelBase
{

    #region Properties

    private PopupResult _result;
    public PopupResult Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    private Uri? _windowIcon;
    public Uri WindowIcon
    {
        get => _windowIcon!;
        set => SetProperty(ref _windowIcon, value);
    }

    private string? _title;
    public string Title
    {
        get => _title!;
        set => SetProperty(ref _title, value);
    }

    private string? _majorText;
    public string MajorText
    {
        get => _majorText!;
        set => SetProperty(ref _majorText, value);
    }

    private string? _minorText;
    public string MinorText
    {
        get => _minorText!;
        set => SetProperty(ref _minorText, value);
    }

    private Uri? _icon;
    public Uri Icon
    {
        get => _icon!;
        set => SetProperty(ref _icon, value);
    }

    private double _maxWidth;
    public double MaxWidth
    {
        get => _maxWidth;
        set => SetProperty(ref _maxWidth, value);
    }

    private GridLength _yesWidth;
    public GridLength YesWidth
    {
        get => _yesWidth;
        set => SetProperty(ref _yesWidth, value);
    }

    private Uri? _yesIcon;
    public Uri YesIcon
    {
        get => _yesIcon!;
        set => SetProperty(ref _yesIcon, value);
    }

    private string? _yesText;
    public string YesText
    {
        get => _yesText!;
        set => SetProperty(ref _yesText, value);
    }

    private bool _yesDefault;
    public bool YesDefault
    {
        get => _yesDefault;
        set => SetProperty(ref _yesDefault, value);
    }

    private GridLength _noWidth;
    public GridLength NoWidth
    {
        get => _noWidth;
        set => SetProperty(ref _noWidth, value);
    }

    private Uri? _noIcon;
    public Uri NoIcon
    {
        get => _noIcon!;
        set => SetProperty(ref _noIcon, value);
    }

    private string? _noText;
    public string NoText
    {
        get => _noText!;
        set => SetProperty(ref _noText, value);
    }

    private bool _noCancel;
    public bool NoCancel
    {
        get => _noCancel;
        set => SetProperty(ref _noCancel, value);
    }

    private GridLength _okWidth;
    public GridLength OkWidth
    {
        get => _okWidth;
        set => SetProperty(ref _okWidth, value);
    }

    private Uri? _okIcon;
    public Uri OkIcon
    {
        get => _okIcon!;
        set => SetProperty(ref _okIcon, value);
    }

    private string? _okText;
    public string OkText
    {
        get => _okText!;
        set => SetProperty(ref _okText, value);
    }

    private bool _okDefault;
    public bool OkDefault
    {
        get => _okDefault;
        set => SetProperty(ref _okDefault, value);
    }

    private bool _okCancel;
    public bool OkCancel
    {
        get => _okCancel;
        set => SetProperty(ref _okCancel, value);
    }

    private GridLength _cancelWidth;
    public GridLength Cancelwidth
    {
        get => _cancelWidth;
        set => SetProperty(ref _cancelWidth, value);
    }

    private Uri? _cancelIcon;
    public Uri CancelIcon
    {
        get => _cancelIcon!;
        set => SetProperty(ref _cancelIcon, value);
    }

    private string? _cancelText;
    public string CancelText
    {
        get => _cancelText!;
        set => SetProperty(ref _cancelText, value);
    }

    private bool _cancelCancel;
    public bool CancelCancel
    {
        get => _cancelCancel;
        set => SetProperty(ref _cancelCancel, value);
    }

    private Visibility _iconVisibility;
    public Visibility IconVisibility
    {
        get => _iconVisibility;
        set => SetProperty(ref _iconVisibility, value);
    }

    private Visibility _minorVisibility;
    public Visibility MinorVisibility
    {
        get => _minorVisibility;
        set => SetProperty(ref _minorVisibility, value);
    }

    private double _buttonWidth;
    public double ButtonWidth
    {
        get => _buttonWidth;
        set => SetProperty(ref _buttonWidth, value);
    }

    private double _majorFontSize;
    public double MajorFontSize
    {
        get => _majorFontSize;
        set => SetProperty(ref _majorFontSize, value);
    }

    private double _minorFontSize;
    public double MinorFontSize
    {
        get => _minorFontSize;
        set => SetProperty(ref _minorFontSize, value);
    }

    #endregion

    #region Commands

    private RelayCommand? _yesCommand;
    public ICommand YesCommand
    {
        get
        {
            if (_yesCommand is null)
            {
                _yesCommand = new(param => YesClick(), param => AlwaysCanExecute());
            }
            return _yesCommand;
        }
    }

    private RelayCommand? _noCommand;
    public ICommand NoCommand
    {
        get
        {
            if (_noCommand is null)
            {
                _noCommand = new(param => NoClick(), param => AlwaysCanExecute());
            }
            return _noCommand;
        }
    }

    #endregion

    #region Command Methods

    private void YesClick()
    {
        Result = PopupResult.Yes;
        DialogResult = true;
    }

    private void NoClick()
    {
        Result = PopupResult.No;
        DialogResult = false;
    }

    public override void OK()
    {
        Result = PopupResult.Ok;
        DialogResult = true;
    }

    public override void Cancel()
    {
        Result = PopupResult.Cancel;
        DialogResult = false;
    }

    #endregion

    #region Constructors

    public void SetParameters(string title, string major, string minor, Uri icon, PopupButtons buttons,
        string[] buttonTexts, Uri[] buttonImages, double buttonWidth = 65.0, Uri? windowUri = null,
        double majorFontSize = 18.0, double minorFontSize = 12.0, double maxwidth = 600)
    {
        WindowIcon = windowUri!;
        ButtonWidth = buttonWidth;
        MajorFontSize = majorFontSize;
        MinorFontSize = minorFontSize;
        Title = title;
        MajorText = major;
        MinorText = minor;
        MaxWidth = maxwidth;
        MinorVisibility = string.IsNullOrEmpty(MinorText) ? Visibility.Collapsed : Visibility.Visible;
        Icon = icon;
        IconVisibility = Icon is null ? Visibility.Collapsed : Visibility.Visible;
        if (buttonTexts.Length != 4)
        {
            throw new InvalidOperationException("Button Texts need to be an array of 4");
        }
        if (buttonImages.Length != 4)
        {
            throw new InvalidOperationException("Button Images need to be an array of 4");
        }
        YesText = buttonTexts[0];
        YesIcon = buttonImages[0];
        NoText = buttonTexts[1];
        NoIcon = buttonImages[1];
        OkText = buttonTexts[2];
        OkIcon = buttonImages[2];
        CancelText = buttonTexts[3];
        CancelIcon = buttonImages[3];
        YesWidth = new GridLength(0);
        NoWidth = new GridLength(0);
        OkWidth = new GridLength(0);
        Cancelwidth = new GridLength(0);
        YesDefault = false;
        NoCancel = false;
        OkDefault = false;
        OkCancel = buttons == PopupButtons.Ok;
        CancelCancel = false;
        if ((buttons & PopupButtons.Yes) == PopupButtons.Yes)
        {
            YesWidth = new GridLength(1, GridUnitType.Star);
            YesDefault = true;
        }
        if ((buttons & PopupButtons.No) == PopupButtons.No)
        {
            NoWidth = new GridLength(1, GridUnitType.Star);
            NoCancel = true;
        }
        if ((buttons & PopupButtons.Ok) == PopupButtons.Ok)
        {
            OkWidth = new GridLength(1, GridUnitType.Star);
            YesDefault = false;
            OkDefault = true;
        }
        if ((buttons & PopupButtons.Cancel) == PopupButtons.Cancel)
        {
            Cancelwidth = new GridLength(1, GridUnitType.Star);
            NoCancel = false;
            CancelCancel = true;
        }
    }

    #endregion
}
