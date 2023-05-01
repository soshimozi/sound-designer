using System.Windows.Markup;
using System;

namespace SoundDesigner.Lib;

/// <summary>
/// A markup extension that returns a collection of values of a specific <see langword="enum"/>
/// </summary>
[MarkupExtensionReturnType(typeof(Array))]
public sealed class EnumValuesExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the <see cref="System.Type"/> of the target <see langword="enum"/>
    /// </summary>
    public Type Type { get; set; }

    /// <inheritdoc/>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(Type);
    }
}