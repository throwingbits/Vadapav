namespace Vadapav
{
    internal class VadapavRouteProvider
    {
        internal const string ApiPath = "/api";

        internal const string DirectoryPathSpecifier = "d";
        internal const string FilePathSpecifier = "f";
        internal const string SearchPathSpecifier = "s";

        internal static readonly List<string> AllSpecifiers =
        [
            DirectoryPathSpecifier,
            FilePathSpecifier,
            SearchPathSpecifier,
        ];
    }
}