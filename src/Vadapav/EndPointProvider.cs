namespace Vadapav
{
    internal static class EndPointProvider
    {
        private const string Base = "/api";

        internal const string Root = $"{Base}/d";
        internal const string Search = $"{Base}/s";
        internal const string Directory = $"{Base}/d";
        internal const string File = $"/f";

        internal static string Create(string @base, string value)
        {
            var v = value.StartsWith('/') || @base.EndsWith('/') ?
                value :
                $"/{value}";

            return $"{@base}{v}";
        }
    }
}
