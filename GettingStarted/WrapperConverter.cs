using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GettingStarted;

public class WrapperConverter : ValueConverter<Wrapper, string>
{
    public WrapperConverter()
        : base(
            v => v.ToString(),
            v => new Wrapper(v)
        )
    {
    }
}