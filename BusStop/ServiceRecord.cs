namespace BusStop;

internal class ServiceRecord : IComparable<ServiceRecord>
{
    public enum CompanyEnum
    {
        Posh,
        Grotty
    }

    private CompanyEnum _company;
    private TimeOnly _departureTime;
    private TimeOnly _arrivalTime;

    public bool IsValid { get; set; }

    public CompanyEnum Company { get { return _company; } }

    public ServiceRecord(string serviceLine)
    {
        var serviceColumns = serviceLine.Split(" ");

        IsValid =
            serviceColumns.Length == 3
            && Enum.TryParse<CompanyEnum>(serviceColumns[0], out _company)
            && serviceColumns[1].Length == 5
            && TimeOnly.TryParse(serviceColumns[1], out _departureTime)
            && serviceColumns[2].Length == 5
            && TimeOnly.TryParse(serviceColumns[2], out _arrivalTime)
            && _arrivalTime > _departureTime
            && _arrivalTime - _departureTime <= TimeSpan.FromHours(1);
    }

    public override string ToString()
    {
        return $"{_company} {_departureTime:HH:mm} {_arrivalTime:HH:mm}";
    }

    public int CompareTo(ServiceRecord? other)
    {
        return other == null ? 1 : 
            _arrivalTime > other._arrivalTime ? 1 :
            _arrivalTime < other._arrivalTime ? -1 :
                _departureTime < other._departureTime ? 1 :
                _departureTime > other._departureTime ? -1 :
                    _company == CompanyEnum.Posh ? -1 : 1;
    }

    public bool IsEqualOrWorseThan(ServiceRecord other)
    {
        return _arrivalTime >= other._arrivalTime && _departureTime <= other._departureTime;
    }
}
