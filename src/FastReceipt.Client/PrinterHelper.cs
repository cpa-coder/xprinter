using System.Runtime.InteropServices;

namespace FastReceipt.Client;

// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Local
internal static class PrinterHelper
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private class DocumentInfo
    {
        [MarshalAs(UnmanagedType.LPStr)] public string DocumentName = string.Empty;
        [MarshalAs(UnmanagedType.LPStr)] public string OutputFile = string.Empty;
        [MarshalAs(UnmanagedType.LPStr)] public string DataType = string.Empty;
    }

    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter,
        IntPtr pd);

    [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
    private static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    private static extern bool StartDocPrinter(IntPtr hPrinter, int level,
        [In] [MarshalAs(UnmanagedType.LPStruct)]
        DocumentInfo info);

    [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
    private static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
    private static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
    private static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
    private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

    internal static void SendBytesToPrinter(string documentName, string printerName, IntPtr bytes, int count)
    {
        var di = new DocumentInfo { DocumentName = documentName, DataType = "RAW" };

        if (OpenPrinter(printerName.Normalize(), out var printer, IntPtr.Zero) is false) return;
        if (StartDocPrinter(printer, 1, di))
        {
            if (StartPagePrinter(printer))
            {
                WritePrinter(printer, bytes, count, out _);
                EndPagePrinter(printer);
            }

            EndDocPrinter(printer);
        }

        ClosePrinter(printer);
    }
}