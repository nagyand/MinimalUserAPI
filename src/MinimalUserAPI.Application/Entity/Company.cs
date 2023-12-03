using System.Text.Json.Serialization;

namespace MinimalUserAPI.Application.Entity;
public record Company
{
    [JsonConstructor]
    public Company()
    {
        
    }
    public string Name { get; init; }
    public string CatchPhrase { get; init; }
    public string Bs {  get; init; }
}
