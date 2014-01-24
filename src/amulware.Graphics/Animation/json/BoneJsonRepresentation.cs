namespace amulware.Graphics.Animation
{
    class BoneJsonRepresentation<TBoneAttributes>
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public TBoneAttributes Attributes { get; set; }
    }
}
