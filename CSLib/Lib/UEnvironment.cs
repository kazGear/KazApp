using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace CSLib.Lib
{
    /// <summary>
    /// 定数ユーティリティ
    /// </summary>
    public class UEnvironment
    {
        /// <summary>
        /// enumの定数リストを取得 
        /// </summary>
        public static string IsRuntime()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OSPlatform.Windows.ToString().ToLower();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSPlatform.OSX.ToString().ToLower();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OSPlatform.Linux.ToString().ToLower();
            else
                return "unknown";
        }

    }
}
