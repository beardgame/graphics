using OpenTK;

namespace amulware.Graphics.Meshes
{
    partial class Mesh<TVertex>
        where TVertex : struct
    {
        /// <summary>
        /// Returns a simple tetrahedron centered around (0, 0, 0).
        /// The first vertex is at (0, 0, 1).
        /// </summary>
        public static Mesh<MeshVertex> Tetrahedron()
        {
            const float sqrt2 = 1.41421356237f;
            const float sqrt6 = 2.44948974278f;

            return create(
                new[]
                {
                    new MeshVertex(new Vector3(0, 0, 1)),
                    new MeshVertex(new Vector3(sqrt2 * 2f / 3f, 0, -1f / 3f)),
                    new MeshVertex(new Vector3(-sqrt2 / 3f, sqrt6 / 3, -1f / 3f)),
                    new MeshVertex(new Vector3(-sqrt2 / 3f, -sqrt6 / 3, -1f / 3f)),
                },
                new[]
                {
                    new IndexTriangle(0, 1, 2),
                    new IndexTriangle(0, 2, 3),
                    new IndexTriangle(0, 3, 1),
                    new IndexTriangle(1, 3, 2),
                });
        }

        /// <summary>
        /// Returns a simple octahedron centered around (0, 0, 0).
        /// All vertices lie on a euclidian axes at 1 or -1.
        /// </summary>
        public static Mesh<MeshVertex> Octahedron()
        {
            return create(
                new[]
                {
                    new MeshVertex(new Vector3(1, 0, 0)),
                    new MeshVertex(new Vector3(-1, 0, 0)),
                    new MeshVertex(new Vector3(0, 1, 0)),
                    new MeshVertex(new Vector3(0, -1, 0)),
                    new MeshVertex(new Vector3(0, 0, 1)),
                    new MeshVertex(new Vector3(0, 0, -1)),
                },
                new[]
                {
                    new IndexTriangle(4, 0, 2),
                    new IndexTriangle(4, 2, 1),
                    new IndexTriangle(4, 1, 3),
                    new IndexTriangle(4, 3, 0),
                    new IndexTriangle(5, 2, 0),
                    new IndexTriangle(5, 1, 2),
                    new IndexTriangle(5, 3, 1),
                    new IndexTriangle(5, 0, 3),
                });
        }

        /// <summary>
        /// Returns a simple axis-aligned hexahedron/cube centered around (0, 0, 0).
        /// All edges are length 1.
        /// </summary>
        public static Mesh<MeshVertex> Cube()
        {
            return Hexahedron();
        }
        /// <summary>
        /// Returns a simple axis-aligned hexahedron/cube centered around (0, 0, 0).
        /// All edges are length 1.
        /// </summary>
        public static Mesh<MeshVertex> Hexahedron()
        {
            const float u = 0.5f;

            return create(
                new[]
                {
                    new MeshVertex(new Vector3(-u, -u, -u)),
                    new MeshVertex(new Vector3(u, -u, -u)),
                    new MeshVertex(new Vector3(u, u, -u)),
                    new MeshVertex(new Vector3(-u, u, -u)),
                    
                    new MeshVertex(new Vector3(-u, -u, u)),
                    new MeshVertex(new Vector3(u, -u, u)),
                    new MeshVertex(new Vector3(u, u, u)),
                    new MeshVertex(new Vector3(-u, u, u)),
                },
                new[]
                {
                    new IndexTriangle(0, 3, 2),
                    new IndexTriangle(0, 2, 1),
                    new IndexTriangle(0, 1, 5),
                    new IndexTriangle(0, 5, 4),
                    new IndexTriangle(0, 4, 7),
                    new IndexTriangle(0, 7, 3),
                    new IndexTriangle(6, 5, 1),
                    new IndexTriangle(6, 1, 2),
                    new IndexTriangle(6, 2, 3),
                    new IndexTriangle(6, 3, 7),
                    new IndexTriangle(6, 7, 4),
                    new IndexTriangle(6, 4, 5),
                });
        }

        /// <summary>
        /// Returns a simple dodecahedron centered around (0, 0, 0).
        /// </summary>
        public static Mesh<MeshVertex> Dodecahedron()
        {
            const float a = 0.57735026919f; // 1/sqrt(3)
            const float b = 0.35682208977f; // sqrt((3-sqrt(5)/6)
            const float c = 0.93417235896f; // sqrt((3+sqrt(5)/6)

            return create(
                new[]
                {
					new MeshVertex(new Vector3(a,a,a)),
                    new MeshVertex(new Vector3(a,a,-a)),
                    new MeshVertex(new Vector3(a,-a,a)),
                    new MeshVertex(new Vector3(a,-a,-a)),
                    new MeshVertex(new Vector3(-a,a,a)),
                    new MeshVertex(new Vector3(-a,a,-a)),
                    new MeshVertex(new Vector3(-a,-a,a)),
                    new MeshVertex(new Vector3(-a,-a,-a)),
                    new MeshVertex(new Vector3(b,c,0)),
                    new MeshVertex(new Vector3(-b,c,0)),
                    new MeshVertex(new Vector3(b,-c,0)),
                    new MeshVertex(new Vector3(-b,-c,0)),
                    new MeshVertex(new Vector3(c,0,b)),
                    new MeshVertex(new Vector3(c,0,-b)),
                    new MeshVertex(new Vector3(-c,0,b)),
                    new MeshVertex(new Vector3(-c,0,-b)),
                    new MeshVertex(new Vector3(0,b,c)),
                    new MeshVertex(new Vector3(0,-b,c)),
                    new MeshVertex(new Vector3(0,b,-c)),
                    new MeshVertex(new Vector3(0,-b,-c)),
                },
                new[]
                {
					new IndexTriangle(0, 8, 9),
					new IndexTriangle(0, 9, 4),
					new IndexTriangle(0, 4, 16),
					new IndexTriangle(0, 12, 13),
					new IndexTriangle(0, 13, 1),
					new IndexTriangle(0, 1, 8),
					new IndexTriangle(0, 16, 17),
					new IndexTriangle(0, 17, 2),
					new IndexTriangle(0, 2, 12),
					new IndexTriangle(8, 1, 18),
					new IndexTriangle(8, 18, 5),
					new IndexTriangle(8, 5, 9),
					new IndexTriangle(12, 2, 10),
					new IndexTriangle(12, 10, 3),
					new IndexTriangle(12, 3, 13),
					new IndexTriangle(16, 4, 14),
					new IndexTriangle(16, 14, 6),
					new IndexTriangle(16, 6, 17),
					new IndexTriangle(9, 5, 15),
					new IndexTriangle(9, 15, 14),
					new IndexTriangle(9, 14, 4),
					new IndexTriangle(6, 11, 10),
					new IndexTriangle(6, 10, 2),
					new IndexTriangle(6, 2, 17),
					new IndexTriangle(3, 19, 18),
					new IndexTriangle(3, 18, 1),
					new IndexTriangle(3, 1, 13),
					new IndexTriangle(7, 15, 5),
					new IndexTriangle(7, 5, 18),
					new IndexTriangle(7, 18, 19),
					new IndexTriangle(7, 11, 6),
					new IndexTriangle(7, 6, 14),
					new IndexTriangle(7, 14, 15),
					new IndexTriangle(7, 19, 3),
					new IndexTriangle(7, 3, 10),
					new IndexTriangle(7, 10, 11),
                });
        }

        /// <summary>
        /// Returns a simple icosahedron centered around (0, 0, 0).
        /// </summary>
        public static Mesh<MeshVertex> Icosahedron()
        {
            const float t = 1.61803398875f; // (1+sqrt(5))/2
            const float s = 1.90211303259f; // sqrt(1+t^2)

            const float a = t / s;
            const float b = 1f / s;

            return create(
                new[]
                {
					new MeshVertex(new Vector3(a,b,0)),
                    new MeshVertex(new Vector3(-a,b,0)),
                    new MeshVertex(new Vector3(a,-b,0)),
                    new MeshVertex(new Vector3(-a,-b,0)),
                    new MeshVertex(new Vector3(b,0,a)),
                    new MeshVertex(new Vector3(b,0,-a)),
                    new MeshVertex(new Vector3(-b,0,a)),
                    new MeshVertex(new Vector3(-b,0,-a)),
                    new MeshVertex(new Vector3(0,a,b)),
                    new MeshVertex(new Vector3(0,-a,b)),
                    new MeshVertex(new Vector3(0,a,-b)),
                    new MeshVertex(new Vector3(0,-a,-b)),
                },
                new[]
                {
					new IndexTriangle(0, 8, 4),
					new IndexTriangle(0, 5, 10),
					new IndexTriangle(2, 4, 9),
					new IndexTriangle(2, 11, 5),
					new IndexTriangle(1, 6, 8),
					new IndexTriangle(1, 10, 7),
					new IndexTriangle(3, 9, 6),
					new IndexTriangle(3, 7, 11),
					new IndexTriangle(0, 10, 8),
					new IndexTriangle(1, 8, 10),
					new IndexTriangle(2, 9, 11),
					new IndexTriangle(3, 11, 9),
					new IndexTriangle(4, 2, 0),
					new IndexTriangle(5, 0, 2),
					new IndexTriangle(6, 1, 3),
					new IndexTriangle(7, 3, 1),
					new IndexTriangle(8, 6, 4),
					new IndexTriangle(9, 4, 6),
					new IndexTriangle(10, 5, 7),
					new IndexTriangle(11, 7, 5),
                });
        }

        private static Mesh<T> create<T>(T[] vertices, IndexTriangle[] triangles)
            where T : struct
        {
            return new Mesh<T>(vertices, triangles);
        }
    }
}