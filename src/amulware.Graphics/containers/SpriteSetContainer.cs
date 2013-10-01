using System;
using System.Collections.Generic;

namespace amulware.Graphics
{
    public class SpriteSetContainer<TVertexData> : ISpriteSet<TVertexData>
        where TVertexData : struct, IVertexData
    {
        private readonly List<SpriteSet<TVertexData>> spriteSets;

        private readonly Dictionary<string, Sprite<TVertexData>> sprites;

        public readonly IEnumerable<SpriteSet<TVertexData>> SpriteSets { get { return this.spriteSets; } }

        public Sprite<TVertexData> this[string spriteName]
        {
            get
            {
                Sprite<TVertexData> s;
                this.sprites.TryGetValue(spriteName, out s);
                return s;
            }
        }

        public SpriteSetContainer()
        {
            this.spriteSets = new List<SpriteSet<TVertexData>>();
            this.sprites = new Dictionary<string, Sprite<TVertexData>>();
        }

        public void Add(SpriteSet<TVertexData> set)
        {
            foreach (var pair in set.Sprites)
                this.sprites.Add(pair.Key, pair.Value);
            this.spriteSets.Add(set);
        }
    }
}
