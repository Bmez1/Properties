using Properties.Domain.Entities;

namespace Properties.Application.Interfaces;

public interface ITokenProvider
{
    string Create(User user);
}
