using Application.Ports.Time;

namespace Common.Time;

public class SystemClock : IClock
{
    public DateTime UtcNow()
    {
        return DateTime.UtcNow;
    }
}