using System.Diagnostics.CodeAnalysis;
using Api = Evergine.Bindings.RenderDoc.RenderDoc;

namespace Bearded.Graphics.Windowing;

public static partial class RenderDoc
{
    public static bool TryLoad([NotNullWhen(true)] out IRenderDoc? renderDoc)
    {
        var success = Api.Load(out var api);
        renderDoc = success ? new Implementation(api) : null;
        return success;
    }

    public static IRenderDoc LoadOrDummy()
    {
        var success = Api.Load(out var renderDoc);

        return success
            ? new Implementation(renderDoc)
            : Dummy;
    }

    public static IRenderDoc Dummy { get; } = new DummyImplementation();
}
