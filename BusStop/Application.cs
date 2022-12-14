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

            var position = 1;
            while (position < serviceRecords.Count)
            {
                if (serviceRecords[position].IsEqualOrWorseThan(serviceRecords[position - 1]))
                {
                    serviceRecords.RemoveAt(position);
                }
                else
                {
                    position++;
                }
            }

            return (
                string.Join(Environment.NewLine, serviceRecords.Where(x => x.Company == ServiceRecord.CompanyEnum.Posh)) +
                Environment.NewLine + Environment.NewLine +
                string.Join(Environment.NewLine, serviceRecords.Where(x => x.Company == ServiceRecord.CompanyEnum.Grotty))
            ).Trim();
        }
    }
}
