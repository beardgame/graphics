namespace amulware.Graphics.Animation
{
    sealed public class BoneTemplate
    {
        private readonly int id;
        private readonly string name;
        private readonly BoneTemplate parent;
        private readonly string sprite;

        internal BoneTemplate(int id, string name, BoneTemplate parent, string sprite)
        {
            this.name = name;
            this.parent = parent;
            this.sprite = sprite;
            this.id = id;
        }

        public int Id { get { return this.id; } }
        public string Name { get { return this.name; } }
        public BoneTemplate Parent { get { return this.parent; } }
        public string Sprite { get { return this.sprite; } }

    }
}
