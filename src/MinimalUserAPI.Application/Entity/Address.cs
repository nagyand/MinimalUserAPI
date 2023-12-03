namespace MinimalUserAPI.Application.Entity;
public record Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string Suite { get; init; }
    public string ZipCode { get; init; }
    public Geo Geo { get; init; }
}
