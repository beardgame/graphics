namespace amulware.Graphics
{
    public interface ISpriteSet<TVertexData>
        where TVertexData : struct, IVertexData
    {
        Sprite<TVertexData> this[string spriteName] { get; }
    }
}
