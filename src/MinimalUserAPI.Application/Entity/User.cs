namespace MinimalUserAPI.Application.Entity;
public record User
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string UserName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Website { get; init; }
    public Company Company { get; init; }
    public Address Address { get; init; }
}
