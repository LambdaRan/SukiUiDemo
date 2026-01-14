using SukiUiDemo.Common;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace SukiUiDemo.Utils;

public static class MiscUtils
{
	public static void OpenUrl(string url)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			Process.Start("xdg-open", url);
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			Process.Start("open", url);
	}

	public static int ExcelColumnToNumber(string column)
	{
		int number = 0;
		int multiple = 1;
		for (int i = column.Length - 1; i >= 0; i--) {
			int k = column[i] - 'A' + 1;
			number += k * multiple;
			multiple *= 26;
		}
		return number;
	}
	public static string ExcelNumberToColumn(int number)
	{
		var vsb = new ValueStringBuilder(stackalloc char[10]);
		while (number > 0) {
			int modulo = (number - 1) % 26;
			vsb.Append(Convert.ToChar('A' + modulo));
			number = (number - modulo) / 26;
		}
		return string.Join("", vsb.AsSpan().ToArray().Reverse());
	}
	public static (int start, int end) ExcelRangeAddressParse(string address)
	{
		//var tokenizer = new StringTokenizer(address, new char[] { ':' });
		// $B$1:$BH$1	$A$1
		int eIdx = address.IndexOf(':');
		eIdx = eIdx < 0 ? address.Length : eIdx;
		var saddr = address.AsSpan(0, eIdx);
		int lastDollar = saddr.LastIndexOf('$');
		string scol = lastDollar == 0 ? "A" : saddr.Slice(1, lastDollar - 1).ToString();
		string srow = saddr.Slice(lastDollar + 1).ToString();
		return (Convert.ToInt32(srow), MiscUtils.ExcelColumnToNumber(scol));
	}
}
