namespace amulware.Graphics.Meshes
{
    /// <summary>
    /// A triplet of vertex indices, representing a triangle.
    /// </summary>
    public struct IndexTriangle
    {
        private readonly int index0;
        private readonly int index1;
        private readonly int index2;

        /// <summary>
        /// Creates a new index triangle.
        /// </summary>
        public IndexTriangle(int i0, int i1, int i2)
        {
            this.index0 = i0;
            this.index1 = i1;
            this.index2 = i2;
        }

        /// <summary>
        /// The first index.
        /// </summary>
        public int Index0 { get { return this.index0; } }

        /// <summary>
        /// The first index.
        /// </summary>
        public int Index1 { get { return this.index1; } }

        /// <summary>
        /// The first index.
        /// </summary>
        public int Index2 { get { return this.index2; } }
    }
}