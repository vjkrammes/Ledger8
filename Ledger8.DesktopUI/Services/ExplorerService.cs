using Ledger8.DesktopUI.Interfaces;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ledger8.DesktopUI.Services;

internal class ExplorerService : IExplorerService
{
    public IEnumerable<DirectoryInfo> GetDirectories(string path) => Directory.GetDirectories(path, "*.*").Select(x => new DirectoryInfo(x));
    public IEnumerable<DriveInfo> GetDrives() => Directory.GetLogicalDrives().Select(x => new DriveInfo(x));
    public IEnumerable<FileInfo> GetFiles(string path) => Directory.GetFiles(path, "*.*").Select(x => new FileInfo(x));
}
