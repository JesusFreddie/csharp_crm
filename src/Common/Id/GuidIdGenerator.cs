using Application.Ports.Id;

namespace Common.Id;

public class GuidIdGenerator : IIdGenerator
{
    public Guid New()
    {
        return Guid.NewGuid();
    }
}