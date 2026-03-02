using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Models;

public class User : IdentityUser<long>
{
    public List<IdentityRole<long>>? Roles { get; set; }
    public string Name { get; set; } = string.Empty;
}