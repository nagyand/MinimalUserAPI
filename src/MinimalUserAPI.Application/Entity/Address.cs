using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalUserAPI.Application.Entity;
public class Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string Suite { get; init; }
    public string ZipCode { get; init; }
    public Geo Geo { get; init; }
}
