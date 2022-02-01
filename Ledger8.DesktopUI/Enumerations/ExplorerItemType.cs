using Ledger8.DesktopUI.Infrastructure;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ledger8.DesktopUI.Enumerations;

public enum ExplorerItemType
{
    [Description("Unspecified")]
    [ExplorerIcon("/resources/question-32.png")]
    Unspecified = 0,
    [Description("This Computer")]
    [ExplorerIcon("/resources/device-32.png")]
    ThisComputer = 1,
    [Description("Disk Drive")]
    [ExplorerIcon("/resources/save-32.png")]
    Drive = 2,
    [Description("Directory")]
    [ExplorerIcon("/resources/folder-32.png")]
    Directory = 3,
    [Description("File")]
    [ExplorerIcon("/resources/script-32.png")]
    File = 4,
    [Description("Placeholder")]
    [ExplorerIcon("/resources/ellipsis-32.png")]
    Placeholder = 999
}
