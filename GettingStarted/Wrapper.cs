using System.Text.Json;
using System.Text.Json.Serialization;

namespace GettingStarted;

//[JsonConverter(typeof(WrapperJsonConverter))]
public class Wrapper
{
    public string Part1 { get; }
    public string Part2 { get; }

    public Wrapper(string? parts)
    {
        if (parts == null)
        {
            Part1 = "";
            Part2 = "";
            return;
        }

        string[] split = parts.Split(",");
        Part1 = split[0];
        Part2 = split[1];
    }

    public Wrapper(string part1, string part2)
    {
        Part1 = part1;
        Part2 = part2;
    }

    public override string ToString() => Part1 + "," + Part2;
}

public class WrapperJsonConverter : JsonConverter<Wrapper>
{
    public override Wrapper Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return new Wrapper(reader.GetString());
    }

    public override void Write(
        Utf8JsonWriter writer,
        Wrapper value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value.ToString());
    }
}