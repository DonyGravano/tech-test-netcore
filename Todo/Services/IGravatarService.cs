using System.Threading.Tasks;
using Todo.Models.Gravatar;

namespace Todo.Services;

public interface IGravatarService
{
    Task<GravatarProfile> GetProfileAsync(string emailAddress);
}