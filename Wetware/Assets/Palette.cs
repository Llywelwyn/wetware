using Raylib_cs;

namespace Wetware.Assets;

public static class Palette
{
    #pragma warning disable IDE0055
    public static readonly Color DarkRed =     FromHex("#a64a2e");
    public static readonly Color Red =         FromHex("#d74200");
    public static readonly Color DarkOrange =  FromHex("#f15f22");
    public static readonly Color Orange =      FromHex("#e99f10");
    public static readonly Color Brown =       FromHex("#98875f");
    public static readonly Color Gold =        FromHex("#cfc041");
    public static readonly Color DarkGreen =   FromHex("#009403");
    public static readonly Color Green =       FromHex("#00c420");
    public static readonly Color DarkBlue =    FromHex("#0048bd");
    public static readonly Color Blue =        FromHex("#0096ff");
    public static readonly Color DarkCyan =    FromHex("#40a4b9");
    public static readonly Color Cyan =        FromHex("#77bfcf");
    public static readonly Color DarkMagenta = FromHex("#b154cf");
    public static readonly Color Magenta =     FromHex("#da5bd6");
    public static readonly Color Background =  FromHex("#0f3b3a");
    public static readonly Color DarkGrey =    FromHex("#155352");
    public static readonly Color Grey =        FromHex("#b1c9c3");
    public static readonly Color White =       FromHex("#ffffff");
    #pragma warning restore IDE0055

    public static Color FromHex(string hex)
    {
        if (hex.StartsWith('#')) hex = hex[1..];

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = (hex.Length == 8)
            ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)
            : (byte)255;

        return new Color(r, g, b, a);
    }
}
