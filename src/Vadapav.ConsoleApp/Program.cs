using Vadapav.Validation;

namespace Vadapav.ConsoleApp
{
    internal class Program
    {
        static async Task Main()
        {
            var client = new VadapavClient("https://drunk.vadapav.mov");

            var result = VadapavUriValidator.ValidateApiURL("https://drunk.vadapav.mov/da");
            var d = client.UriBuilder.RootDirectoryUri;
            var dir = client.UriBuilder.GetUriForDirectory("asdas");
            var f = client.UriBuilder.GetUriForFile("asdas");
            var s = client.UriBuilder.GetUriForSearch("asdas");


            await RootDirectoryExample(client);
            await GetDirectoryExample(client);
            await SearchExample(client);
            await GetFileExample(client);
        }

        private static async Task RootDirectoryExample(VadapavClient client)
        {
            Console.WriteLine("# RootDirectory example");

            var root = await client.GetRootDirectoryAsync();

            Console.WriteLine($"Root directory contains {root.Directories.Count} directories and {root.Files.Count} files.");
            Console.WriteLine();
        }

        private static async Task GetDirectoryExample(VadapavClient client)
        {
            Console.WriteLine("# GetDirectory Example");
            var root = await client.GetRootDirectoryAsync();
            var directory = await client.GetDirectoryAsync(root.Directories.FirstOrDefault());

            Console.WriteLine($"Retrieved directory '{directory.Name}' which has the id '{directory.Id}'.");
            Console.WriteLine();
        }

        private static async Task SearchExample(VadapavClient client)
        {
            Console.WriteLine("# Search example");

            var searchTerm = "The Originals";
            var result = await client.SearchAsync(searchTerm);

            Console.WriteLine($"The search for term '{searchTerm}' returned {result.Directories.Count} directories and {result.Files.Count} files.");
            Console.WriteLine();
        }

        private static async Task GetFileExample(VadapavClient client)
        {
            Console.WriteLine("# GetFile example");

            var (Name, ContentStream) = await client.GetFileAsync("b5ac93cf-35e3-4981-9f0a-480714f43761");

            var path = Path.Combine(Environment.CurrentDirectory, Name);

            using (var outputFileStream = new FileStream(path, FileMode.OpenOrCreate))
                ContentStream.CopyTo(outputFileStream);

            Console.WriteLine($"Downloaded file '{Name}' to: {path}");
            Console.WriteLine();
        }
    }

}
