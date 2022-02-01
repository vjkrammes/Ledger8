using Ledger8.Common;
using Ledger8.Common.Enumerations;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Infrastructure;
using Ledger8.DesktopUI.Models;
using Ledger8.Models;
using Ledger8.Services.Interfaces;

using Microsoft.Win32;

using Ookii.Dialogs.Wpf;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ledger8.DesktopUI.ViewModels;

public class ImportAccountTypeViewModel : ViewModelBase
{
    private readonly IAccountTypeService _accountTypeService;

    private string? _filename;
    public string Filename
    {
        get => _filename!;
        set => SetProperty(ref _filename, value);
    }

    private ObservableCollection<ImportAccountTypeResult>? _results;
    public ObservableCollection<ImportAccountTypeResult> Results
    {
        get => _results!;
        set => SetProperty(ref _results, value);
    }

    private bool _importing { get; set; } = false;

    private RelayCommand? _selectFileCommand;
    public ICommand SelectFileCommand
    {
        get
        {
            if (_selectFileCommand is null)
            {
                _selectFileCommand = new(parm => SelectFileClick(), parm => SelectCanExecute());
            }
            return _selectFileCommand;
        }
    }

    public RelayCommand? _importCommand;

    public ICommand ImportCommand
    {
        get
        {
            if (_importCommand is null)
            {
                _importCommand = new(async parm => await ImportClick(), parm => ImportCanClick());
            }
            return _importCommand;
        }
    }

    private bool SelectCanExecute() => !_importing;

    private void SelectFileClick()
    {
        Filename = string.Empty;
        var openFileDialog = new VistaOpenFileDialog
        {
            Title = "Select a file to Import",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            CheckFileExists = true,
            CheckPathExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };
        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }
        Filename = openFileDialog.FileName;
    }

    private bool ImportCanClick() => !string.IsNullOrWhiteSpace(Filename) && File.Exists(Filename) && !_importing;

    private async Task ImportClick()
    {
        Results = new();
        if (string.IsNullOrWhiteSpace(Filename) || !File.Exists(Filename))
        {
            return;
        }
        string[] lines;
        try
        {
            lines = File.ReadAllLines(Filename);
        }
        catch (Exception ex)
        {
            PopupManager.Popup("Failed to read import file", Constants.IOE, ex.Innermost(), PopupButtons.Ok, PopupImage.Error);
            return;
        }
        if (lines is null || !lines.Any())
        {
            PopupManager.Popup("File contains no data", "Can't Import", "The selected file is empty", PopupButtons.Ok, PopupImage.Stop);
            return;
        }
        lines.ForEach(x => Results.Add(new() { Description = x, Result = "pending..." }));
        _importing = true;
        await Task.Run(() => DoImport(Results));
        _importing = false;
        
    }
    
    private void DoImport(ObservableCollection<ImportAccountTypeResult> results)
    {
        foreach (var result in results)
        {
            var at = new AccountTypeModel
            {
                Id = 0,
                Description = result.Description.Caseify(),
            };
            var importResult = _accountTypeService.Insert(at);
            var r = results.SingleOrDefault(x => x.Id == result.Id);
            if (r is not null)
            {
                r.Result = importResult.Successful ? "Success" : importResult.Message;
            }
        }
    }

    public ImportAccountTypeViewModel(IAccountTypeService accountTypeService) => _accountTypeService = accountTypeService;
}
