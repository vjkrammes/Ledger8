using Ledger8.Common;
using Ledger8.DataAccess;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Interfaces;

using Ookii.Dialogs.Wpf;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class BackupViewModel : ViewModelBase
{
    private string? _filename;
    public string Filename
    {
        get => _filename!;
        set => SetProperty(ref _filename, value);
    }

    private string? _directory;
    public string Directory
    {
        get => _directory!;
        set => SetProperty(ref _directory, value);
    }

    private ObservableCollection<FileInfo>? _files;
    public ObservableCollection<FileInfo> Files
    {
        get => _files!;
        set => SetProperty(ref _files, value);
    }

    private FileInfo? _selectedFile;
    public FileInfo? SelectedFile
    {
        get => _selectedFile!;
        set => SetProperty(ref _selectedFile, value);
    }

    private readonly ISettings? _settings;
    private readonly LedgerContext? _context;

    private RelayCommand? _changeCommand;
    public ICommand ChangeCommand
    {
        get
        {
            if (_changeCommand is null)
            {
                _changeCommand = new(parm => ChanceClick(), parm => AlwaysCanExecute());
            }
            return _changeCommand;
        }
    }

    private RelayCommand? _backupCommand;
    public ICommand BackupCommand
    {
        get
        {
            if (_backupCommand is null)
            {
                _backupCommand = new(parm => BackupClick(), parm => BackupCanClick());
            }
            return _backupCommand;
        }
    }

    private RelayCommand? _deleteCommand;
    public ICommand DeleteCommand
    {
        get
        {
            if (_deleteCommand is null)
            {
                _deleteCommand = new(parm => DeleteClick(), parm => FileSelected());
            }
            return _deleteCommand;
        }
    }

    public void ChanceClick()
    {
        var picker = new VistaFolderBrowserDialog();
        if (picker.ShowDialog() != true)
        {
            return;
        }
        Directory = picker.SelectedPath;
        _settings!.BackupDirectory = Directory;
        Filename = Directory + @"\" + _backupFileName;
        LoadFiles();
    }

    private bool BackupCanClick() => !string.IsNullOrWhiteSpace(Filename);

    private void BackupClick()
    {
        if (string.IsNullOrWhiteSpace(Filename))
        {
            return;
        }
        try
        {
            _context!.Backup(Filename);
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Database backup failed", Constants.DBE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        PopupManager.Popup("Database backup complete", "Backup Complete", PopupButtons.Ok, PopupImage.Information);
        LoadFiles();
    }

    private bool FileSelected() => SelectedFile is not null;

    private void DeleteClick()
    {
        if (SelectedFile is null)
        {
            return;
        }
        var msg = $"Delete backup file '{SelectedFile.FullName}'? This action cannot be undone.";
        if (PopupManager.Popup("Delete backup file?", "Delete File?", msg, PopupButtons.YesNo, PopupImage.Question) != PopupResult.Yes)
        {
            SelectedFile = null;
            return;
        }
        try
        {
            File.Delete(SelectedFile.FullName);
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Delete of file failed", Constants.IOE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            SelectedFile = null;
            return;
        }
        LoadFiles();
    }

    private void LoadFiles()
    {
        Files = new();
        try
        {
            var files = System.IO.Directory.GetFiles(Directory, "*.backup");
            files.ForEach(x => Files.Add(new(x)));
        }
        catch
        {
            Directory = string.Empty;
        }
    }

    private static string _backupFileName => $"{Constants.ProductName}-{Constants.ProductVersion:0.00}.backup";

    public BackupViewModel(ISettings settings, LedgerContext context)
    {
        _settings = settings;
        _context = context;
        var backupDirectory = settings.BackupDirectory;
        if (string.IsNullOrWhiteSpace(backupDirectory))
        {
            Filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _settings.BackupDirectory = Filename;
        }
        else
        {
            Filename = backupDirectory;
        }
        if (!Filename.EndsWith('\\'))
        {
            Filename += @"\";
        }
        Filename += _backupFileName;
        Directory = Path.GetDirectoryName(Filename)!;
        LoadFiles();
    }
}
