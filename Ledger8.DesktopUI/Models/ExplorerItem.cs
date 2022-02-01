using Ledger8.Common;
using Ledger8.DesktopUI.Enumerations;
using Ledger8.DesktopUI.Interfaces;
using Ledger8.DesktopUI.Services;

using System.Collections.Generic;
using System.IO;

namespace Ledger8.DesktopUI.Models;

public class ExplorerItem : NotifyBase
{
    private ExplorerItemType _type;
    public ExplorerItemType Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private string? _name;
    public string Name
    {
        get => _name!;
        set => SetProperty(ref _name, value);
    }

    private string? _path;
    public string Path
    {
        get => _path!;
        set => SetProperty(ref _path, value);
    }

    private string? _extension;
    public string Extension
    {
        get => _extension!;
        set => SetProperty(ref _extension, value);
    }

    private string? _root;
    public string Root
    {
        get => _root!;
        set => SetProperty(ref _root, value);
    }

    private long _size;
    public long Size
    {
        get => _size;
        set => SetProperty(ref _size, value);
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            SetProperty(ref _isExpanded, value);
            if (Children != null)
            {
                if (IsExpanded)
                {
                    LoadChildren();
                }
                else
                {
                    Children.Clear();
                    if (Type is ExplorerItemType.Directory or ExplorerItemType.Drive)
                    {
                        Children.Add(Placeholder);
                    }
                }
            }
        }
    }

    private IList<ExplorerItem>? _children;
    public IList<ExplorerItem> Children
    {
        get => _children!;
        set => SetProperty(ref _children, value);
    }

    private readonly bool _includeFiles = true;

    private readonly IExplorerService? _explorerService;

    public override string ToString() => Name;

    private void LoadChildren()
    {
        if (Children != null)
        {
            Children.Clear();
            switch (Type)
            {
                case ExplorerItemType.Drive:
                case ExplorerItemType.Directory:
                    foreach (var dir in Directories(_explorerService!, _explorerService!.GetDirectories(Path), _includeFiles))
                    {
                        dir.Children.Add(Placeholder);
                        Children.Add(dir);
                    }
                    if (_includeFiles)
                    {
                        foreach (var file in Files(_explorerService!, _explorerService!.GetFiles(Path), _includeFiles))
                        {
                            file.Children.Add(Placeholder);
                            Children.Add(file);
                        }
                    }
                    break;
                case ExplorerItemType.ThisComputer:
                    foreach (var drive in Drives(_explorerService!, _explorerService!.GetDrives(), _includeFiles))
                    {
                        drive.Children.Add(Placeholder);
                        Children.Add(drive);
                    }
                    break;
            }
        }
    }

    public ExplorerItem()
    {
        Type = ExplorerItemType.Unspecified;
        Name = string.Empty;
        Path = string.Empty;
        Extension = string.Empty;
        Root = string.Empty;
        Size = 0L;
        IsSelected = false;
        IsExpanded = false;
        Children = new List<ExplorerItem>();
        _includeFiles = true;
        _explorerService = null;
    }

    public ExplorerItem(IExplorerService explorerService) : this() => _explorerService = explorerService;

    public ExplorerItem(IExplorerService explorerService, bool includefiles) : this(explorerService) => _includeFiles = includefiles;

    public ExplorerItem(IExplorerService explorerService, string directory) : this(explorerService) => _name = directory;

    public ExplorerItem(IExplorerService explorerService, string directory, bool includefiles) : this(explorerService, includefiles) => _name = directory;

    public ExplorerItem(IExplorerService explorerService, DirectoryInfo dirinfo) : this(explorerService)
    {
        Type = ExplorerItemType.Directory;
        Name = dirinfo.Name;
        Path = dirinfo.FullName;
        Root = dirinfo.Root.Name;
    }

    public ExplorerItem(IExplorerService explorerService, DirectoryInfo dirinfo, bool includefiles) : this(explorerService, includefiles)
    {
        Type = ExplorerItemType.Directory;
        Name = dirinfo.Name;
        Path = dirinfo.FullName;
        Root = dirinfo.Root.Name;
    }

    public ExplorerItem(IExplorerService explorerService, FileInfo fileinfo) : this(explorerService)
    {
        Type = ExplorerItemType.File;
        Name = fileinfo.Name;
        Path = fileinfo.FullName;
        Extension = fileinfo.Extension;
        Size = fileinfo.Length;
    }

    public ExplorerItem(IExplorerService explorerService, FileInfo fileinfo, bool includefiles) : this(explorerService, includefiles)
    {
        Type = ExplorerItemType.File;
        Name = fileinfo.Name;
        Path = fileinfo.FullName;
        Extension = fileinfo.Extension;
        Size = fileinfo.Length;
    }

    public ExplorerItem(IExplorerService explorerService, DriveInfo driveinfo) : this(explorerService)
    {
        Type = ExplorerItemType.Drive;
        Name = driveinfo.Name.EndsWith("\\") ? driveinfo.Name[..^1] : driveinfo.Name;
        Path = driveinfo.Name;
    }

    public ExplorerItem(IExplorerService explorerService, DriveInfo driveinfo, bool includefiles) : this(explorerService, includefiles)
    {
        Type = ExplorerItemType.Drive;
        Name = driveinfo.Name.EndsWith("\\") ? driveinfo.Name[..^1] : driveinfo.Name;
        Path = driveinfo.Name;
    }

    public static ExplorerItem Placeholder => new(new ExplorerService()) { Type = ExplorerItemType.Placeholder };

    public static IEnumerable<ExplorerItem> Directories(IExplorerService explorerService, IEnumerable<DirectoryInfo> directories, bool includefiles = true)
    {
        var ret = new List<ExplorerItem>();
        foreach (var directory in directories)
        {
            ret.Add(new ExplorerItem(explorerService, directory, includefiles));
        }
        return ret;
    }

    public static IEnumerable<ExplorerItem> Files(IExplorerService explorerService, IEnumerable<FileInfo> files, bool includefiles = true)
    {
        var ret = new List<ExplorerItem>();
        foreach (var file in files)
        {
            ret.Add(new ExplorerItem(explorerService, file, includefiles));
        }
        return ret;
    }

    public static IEnumerable<ExplorerItem> Drives(IExplorerService explorerService, IEnumerable<DriveInfo> drives, bool includefiles = true)
    {
        var ret = new List<ExplorerItem>();
        foreach (var drive in drives)
        {
            ret.Add(new ExplorerItem(explorerService, drive, includefiles));
        }
        return ret;
    }
}
