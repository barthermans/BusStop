using Microsoft.Extensions.Hosting;

namespace BusStop
{
    internal class Application : IHostedService
    {
        private readonly string[] _args;
        private readonly IFileService _fileService;

        public Application(string[] args, IFileService fileService)
            => (_args, _fileService) = (args, fileService);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_args is not { Length: 1 }) throw new Exception("Invalid parameter: path");
                
                var serviceLines = await _fileService.ReadInput(_args[0], cancellationToken);

                var result = Process(serviceLines);

                await _fileService.WriteOutput(Path.Combine(Directory.GetCurrentDirectory(), "output.txt"), result, cancellationToken);

                Console.WriteLine("Output file created successfully.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error has occurred: " + ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        internal string Process(string[] serviceLines)
        {
            var serviceRecords = serviceLines.Select(x => new ServiceRecord(x)).Where(x => x.IsValid).ToList();
            serviceRecords.Sort();

            var result = new Dictionary<ServiceRecord.CompanyEnum, List<string>> {
                { ServiceRecord.CompanyEnum.Posh, new() },
                { ServiceRecord.CompanyEnum.Grotty, new() }};

            for (var i = 0; i < serviceRecords.Count; i++)
            {
                if (i == 0 || !serviceRecords[i].IsEqualOrWorseThan(serviceRecords[i - 1]))
                {
                    result[serviceRecords[i].Company].Add(serviceRecords[i].ToString());
                }
            }

            return (
                string.Join(Environment.NewLine, result[ServiceRecord.CompanyEnum.Posh]) +
                Environment.NewLine + Environment.NewLine +
                string.Join(Environment.NewLine, result[ServiceRecord.CompanyEnum.Grotty])
            ).Trim();
        }
    }
}
