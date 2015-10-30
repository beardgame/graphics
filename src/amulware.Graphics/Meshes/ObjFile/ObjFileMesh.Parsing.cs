using System;
using System.Globalization;
using System.IO;
using OpenTK;

namespace amulware.Graphics.Meshes.ObjFile
{
    partial class ObjFileMesh
    {
        private static readonly char[] whiteSpaces = { ' ', '\t' };
        private static readonly char[] faceSplitCharacters = { '/' };


        /// <summary>
        /// Loads a mesh from a file.
        /// </summary>
        public static ObjFileMesh FromFile(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                return FromStream(stream);
            }
        }

        /// <summary>
        /// Loads a mesh from a stream.
        /// </summary>
        public static ObjFileMesh FromStream(Stream stream)
        {
            return FromStreamReader(new StreamReader(stream));
        }

        /// <summary>
        /// Loads a mesh from a stream reader.
        /// </summary>
        public static ObjFileMesh FromStreamReader(StreamReader reader)
        {
            var builder = new Builder();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                parseLine(builder, line);
            }

            return builder.Build();
        }

        private static void parseLine(Builder builder, string line)
        {
            string strippedLine;
            if (isEmptyOrComment(line, out strippedLine))
                return;

            var splitLine = strippedLine.Split(
                whiteSpaces, StringSplitOptions.RemoveEmptyEntries);
            var keyword = splitLine[0];
            var parameterCount = splitLine.Length - 1;

            switch (keyword)
            {
                case "v":
                {
                    if (parameterCount != 4 && parameterCount != 3)
                        throw new InvalidDataException(
                            "Vertex must have 3 or 4 coordinates.");
                    builder.AddVertex(parseVertex(splitLine));
                    break;
                }
                case "vt":
                {
                    if (parameterCount != 3 && parameterCount != 2)
                        throw new InvalidDataException(
                            "UV Coordinate must have 2 or 3 coordinates.");
                    builder.AddUV(parseUV(splitLine));
                    break;
                }
                case "vn":
                {
                    if (parameterCount != 3)
                        throw new InvalidDataException(
                            "Normal must have 3 coordinates.");
                    builder.AddNormal(parseNormal(splitLine));
                    break;
                }
                case "f":
                {
                    if (parameterCount < 3)
                        throw new InvalidDataException(
                            "Face must have at least 3 positions.");
                    builder.AddFace(parseFace(splitLine));
                    break;
                }
                default:
                {
                    throw new InvalidDataException(
                        string.Format("Unknown keyword '{0}'.", keyword));
                }
            }
        }

        private static bool isEmptyOrComment(string line, out string strippedLine)
        {
            strippedLine = null;

            var commentSignId = line.IndexOf('#');

            if (commentSignId == 0)
                return true;

            strippedLine = (commentSignId == -1
                    ? line
                    : line.Substring(0, commentSignId)
                 ).Trim();

            return string.IsNullOrWhiteSpace(strippedLine);
        }

        private static Vector4 parseVertex(string[] splitLine)
        {
            return new Vector4(
                parseFloat(splitLine[1]),
                parseFloat(splitLine[2]),
                parseFloat(splitLine[3]),
                splitLine.Length == 4 ? 1 : parseFloat(splitLine[4])
                );
        }

        private static Vector3 parseUV(string[] splitLine)
        {
            return new Vector3(
                parseFloat(splitLine[1]),
                parseFloat(splitLine[2]),
                splitLine.Length == 3 ? 0 : parseFloat(splitLine[3])
                );
        }

        private static Vector3 parseNormal(string[] splitLine)
        {
            return new Vector3(
                parseFloat(splitLine[1]),
                parseFloat(splitLine[2]),
                parseFloat(splitLine[3])
                );
        }

        private static Face parseFace(string[] splitLine)
        {
            var vertexIds = new Face.VertexIds[
                splitLine.Length - 1];

            for (int i = 1; i < splitLine.Length; i++)
            {
                var ids = splitLine[i].Split(
                    faceSplitCharacters, StringSplitOptions.None);

                var p = parseIndex(ids[0]);

                var uv = -1;
                var n = -1;

                if (ids.Length > 1)
                {
                    uv = parseIndex(ids[1]) - 1;

                    if (ids.Length > 2)
                        n = parseIndex(ids[2]);
                }

                vertexIds[i - 1] = new Face.VertexIds(p, uv, n);
            }
            return new Face(vertexIds);
        }

        private static float parseFloat(string s)
        {
            return float.Parse(s, CultureInfo.InvariantCulture);
        }

        private static int parseIndex(string s)
        {
            if (s == "")
                return -1;

            return int.Parse(s, CultureInfo.InvariantCulture) - 1;
        }
    }
}