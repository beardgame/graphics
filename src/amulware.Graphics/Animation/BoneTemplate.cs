namespace amulware.Graphics.Animation
{
    sealed public class BoneTemplate<TBoneAttributes>
    {
        private readonly int id;
        private readonly string name;
        private readonly BoneTemplate<TBoneAttributes> parent;
        private readonly TBoneAttributes attributes;

        internal BoneTemplate(int id, string name, BoneTemplate<TBoneAttributes> parent, TBoneAttributes attributes)
        {
            this.name = name;
            this.parent = parent;
            this.attributes = attributes;
            this.id = id;
        }

        public int Id { get { return this.id; } }
        public string Name { get { return this.name; } }
        public BoneTemplate<TBoneAttributes> Parent { get { return this.parent; } }
        public TBoneAttributes Attributes { get { return this.attributes; } }

    }
}
