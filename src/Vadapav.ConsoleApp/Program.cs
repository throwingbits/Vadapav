namespace Vadapav.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var x = new VadapavClient();

            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("Sending request: #" + i);
                //var y = await x.GetRootDirectoryAsync();
                var results = await x.SearchFilesAsync(Guid.NewGuid().ToString());
            }
        }
    }sds
}
