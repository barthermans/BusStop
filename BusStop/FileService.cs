namespace BusStop;

internal interface IFileService
{
    Task<string[]> ReadInput(string path, CancellationToken cancellationToken);
    Task WriteOutput(string path, string contents, CancellationToken cancellationToken);
}

internal class FileService : IFileService
{
    public async Task<string[]> ReadInput(string path, CancellationToken cancellationToken)
    {
        return await File.ReadAllLinesAsync(path, cancellationToken);
    }

    public async Task WriteOutput(string path, string contents, CancellationToken cancellationToken)
    {
        await File.WriteAllTextAsync(path, contents, cancellationToken);
    }
}
