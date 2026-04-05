namespace Application.Ports.Id;

public interface IIdGenerator
{
    public Guid New();
}