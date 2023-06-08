using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace GettingStarted.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
[Resource]
public sealed class Book : Identifiable<int>
{
    [Attr]
    public string Title { get; set; } = null!;

    [Attr]
    public int PublishYear { get; set; }

    [Attr]
    public string? Wrapper { get; set; }

    [NotMapped]
    public Wrapper InternalWrapper
    {
        get => new(Wrapper);
        set => Wrapper = value.ToString();
    }

    [HasOne]
    public Person Author { get; set; } = null!;
}
