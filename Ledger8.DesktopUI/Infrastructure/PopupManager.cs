using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.ViewModels;
using Ledger8.DesktopUI.Views;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

namespace Ledger8.DesktopUI.Infrastructure;

public static class PopupManager
{
    #region Properties

    private const string _applicationBase = "/Ledger8.DesktopUI";

    private static Uri _windowIcon;
    private static Uri _informationImage;
    private static Uri _questionImage;
    private static Uri _warningImage;
    private static Uri _stopImage;
    private static Uri _errorImage;

    private readonly static string[] _buttons = new string[] { "Yes", "No", "Ok", "Cancel" };

    private readonly static Uri[] _icons;

    private static double _maxWidth = 600.0;
    private static double _buttonWidth = 65.0;
    private static double _majorFontSize = 18.0;
    private static double _minorFontSize = 12.0;

    #endregion

    #region Constuctor

    static PopupManager()
    {
        _windowIcon = new Uri($"{_applicationBase};component/resources/skull-32.png", UriKind.Relative);
        _informationImage = new Uri($"{_applicationBase};component/resources/info-512.png", UriKind.Relative);
        _questionImage = new Uri($"{_applicationBase};component/resources/question-512.png", UriKind.Relative);
        _warningImage = new Uri($"{_applicationBase};component/resources/warning-512.png", UriKind.Relative);
        _stopImage = new Uri($"{_applicationBase};component/resources/stop-512.png", UriKind.Relative);
        _errorImage = new Uri($"{_applicationBase};component/resources/error-512.png", UriKind.Relative);
        _icons = new Uri[]
        {
            new Uri($"{_applicationBase};component/resources/thumbs-up-32.png", UriKind.Relative),
            new Uri($"{_applicationBase};component/resources/thumbs-down-32.png", UriKind.Relative),
            new Uri($"{_applicationBase};component/resources/checkmark-32.png", UriKind.Relative),
            new Uri($"{_applicationBase};component/resources/cancel-32.png", UriKind.Relative)
        };
    }

    #endregion

    #region Public Methods

    public static void SetInformationImage(Uri uri) => _informationImage = uri;
    public static void SetQuestionImage(Uri uri) => _questionImage = uri;
    public static void SetWarningImage(Uri uri) => _warningImage = uri;
    public static void SetStopImage(Uri uri) => _stopImage = uri;
    public static void SetErrorImage(Uri uri) => _errorImage = uri;

    public static void SetYesText(string msg) => _buttons[0] = msg;
    public static void SetNoText(string msg) => _buttons[1] = msg;
    public static void SetOkText(string msg) => _buttons[2] = msg;
    public static void SetCancelText(string msg) => _buttons[3] = msg;

    public static void SetYesImage(Uri uri) => _icons[0] = uri;
    public static void SetNoImage(Uri uri) => _icons[1] = uri;
    public static void SetOkImage(Uri uri) => _icons[2] = uri;
    public static void SetCancelImage(Uri uri) => _icons[3] = uri;

    public static void SetMaxWidth(double maxwidth) => _maxWidth = maxwidth;
    public static void SetButtonWidth(double width) => _buttonWidth = width;
    public static void SetMajorFontSize(double fontSize) => _majorFontSize = fontSize;
    public static void SetMinorFontSize(double fontSize) => _minorFontSize = fontSize;

    public static void SetWindowIcon(Uri uri) => _windowIcon = uri;

    public static PopupResult Popup(Window owner, string major, string title, string minor, PopupButtons buttons, PopupImage icon)
    {
        var image = icon switch
        {
            PopupImage.Information => _informationImage,
            PopupImage.Question => _questionImage,
            PopupImage.Warning => _warningImage,
            PopupImage.Stop => _stopImage,
            PopupImage.Error => _errorImage,
            _ => null!,
        };
        var vm = (Application.Current as App)?.ServiceProvider?.GetRequiredService<PopupViewModel>()!;
        vm.SetParameters(title, major, minor, image, buttons, _buttons, _icons, _buttonWidth,
            _windowIcon, _majorFontSize, _minorFontSize, _maxWidth);
        if (owner is null)
        {
            owner = Application.Current.MainWindow;
        }
        DialogSupport.ShowDialog<PopupWindow>(vm, owner);
        return vm.Result;
    }

    public static PopupResult Popup(string major, string title, string minor, PopupButtons buttons,
        PopupImage icon) => Popup(null!, major, title, minor, buttons, icon);

    public static PopupResult Popup(Window owner, string major, string title, PopupButtons buttons,
        PopupImage icon) => Popup(owner, major, title, null!, buttons, icon);

    public static PopupResult Popup(string major, string title, PopupButtons buttons, PopupImage icon) =>
        Popup(null!, major, title, null!, buttons, icon);

    public static PopupResult Popup(Window owner, string major, string title, string minor, PopupImage icon) =>
        Popup(owner, major, title, minor, PopupButtons.Ok, icon);

    public static PopupResult Popup(string major, string title, string minor, PopupImage icon) =>
        Popup(null!, major, title, minor, PopupButtons.Ok, icon);

    public static PopupResult Popup(Window owner, string major, string title, PopupImage icon) =>
        Popup(owner, major, title, null!, PopupButtons.Ok, icon);

    public static PopupResult Popup(string major, string title, PopupImage icon) =>
        Popup(null!, major, title, null!, PopupButtons.Ok, icon);

    public static PopupResult Popup(Window owner, string major, string title, string minor) =>
        Popup(owner, major, title, minor, PopupButtons.Ok, PopupImage.None);

    public static PopupResult Popup(string major, string title) =>
        Popup(null!, major, title, null!, PopupButtons.Ok, PopupImage.None);


    #endregion
}
