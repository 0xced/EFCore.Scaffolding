using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace EFCore.Scaffolding.Extensions;

/// <summary>
/// Works around Humanizer <a href="https://github.com/Humanizr/Humanizer/issues/1219">issue #1219</a>: Singularize() throws IndexOutOfRangeException.
/// </summary>
[SuppressMessage("Avoid uninstantiated internal classes", "CA1812", Justification = "It's instantiated through dependency injection")]
internal sealed class WorkaroundCSharpUtilities : CSharpUtilities
{
    public override string GenerateCSharpIdentifier(string identifier, ICollection<string>? existingIdentifiers, Func<string, string>? singularizePluralizer, Func<string, ICollection<string>?, string> uniquifier)
    {
        try
        {
            return base.GenerateCSharpIdentifier(identifier, existingIdentifiers, singularizePluralizer, uniquifier);
        }
        catch (IndexOutOfRangeException)
        {
            return identifier;
        }
    }
}