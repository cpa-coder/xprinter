using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace FastReceipt.Client;

public static class Extensions
{
    public static void OpenDrawer(string printer)
    {
        var ptr = Marshal.AllocCoTaskMem(5);
        byte[] bytes = { 27, 112, 48, 55, 121 };
        Marshal.Copy(bytes, 0, ptr, 5);
        PrinterHelper.SendBytesToPrinter("Opening Drawer", printer, ptr, 5);
        Marshal.FreeCoTaskMem(ptr);
    }
}