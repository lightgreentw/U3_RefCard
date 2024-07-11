using System.Runtime.InteropServices;
using System.Text;

public static class IniFile
{
    // 載入 Win32 API 函式庫
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedString,
        int nSize,
        string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern bool WritePrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern bool WritePrivateProfileSection(
        string lpAppName,
        string lpString,
        string lpFileName);

    // 讀取 INI 檔案中指定節點及鍵的值
    public static string Read(string section, string key, string defaultValue, string filePath)
    {
        StringBuilder sb = new StringBuilder(255);
        int len = GetPrivateProfileString(section, key, defaultValue, sb, 255, filePath);
        return sb.ToString().Substring(0, len);
    }

    // 寫入指定節點及鍵的值到 INI 檔案中
    public static bool Write(string section, string key, string value, string filePath)
    {
        return WritePrivateProfileString(section, key, value, filePath);
    }

    // 新增一個節點及其對應的鍵值到 INI 檔案中
    public static bool Add(string section, string key, string value, string filePath)
    {
        return Write(section, key, value, filePath);
    }

    // 將多個鍵值對寫入 INI 檔案中指定的節點
    public static bool WriteSection(string section, string content, string filePath)
    {
        return WritePrivateProfileSection(section, content, filePath);
    }
}