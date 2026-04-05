namespace Application.Ports.Time;

public interface IClock
{
    public DateTime UtcNow();
}