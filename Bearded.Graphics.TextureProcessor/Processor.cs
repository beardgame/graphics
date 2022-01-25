using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Bearded.Graphics.Content;
using Bearded.Utilities;
using OpenTK.Mathematics;

namespace Bearded.Graphics.TextureProcessor
{
    record struct NamedBitmap(Bitmap Bitmap, string Name, TimeSpan Time = default);

    sealed class Processor
    {
        private readonly Bitmap<Color4> source;
        private readonly Rectangle sourceRectangle;

        public static Processor From(Bitmap bitmap)
        {
            return new Processor(Bitmap<Color4>.From(bitmap, c => new Color4(c.R, c.G, c.B, c.A)));
        }

        private Processor(Bitmap<Color4> source)
        {
            this.source = source;
            sourceRectangle = new Rectangle(0, 0, source.Width, source.Height);
        }

        public IEnumerable<NamedBitmap> Process()
        {
            yield return new (source.ToSystemBitmap(Color4Extensions.ToColor), "Input");

            var closestAlphaStopwatch = Stopwatch.StartNew();
            var closestAlpha = getClosestAlphaMap();
            closestAlphaStopwatch.Stop();
            var maxDistanceToAlpha = closestAlpha.Max(c => c.Pixel.Distance);

            var edges = source.To((_, xy) => edgeDetect(xy));
            yield return new(edges.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Edges");

            var heightmap = closestAlpha.To((p, xy) => 1 - 1 / (p.Distance / maxDistanceToAlpha * 1 + 1));
            var blurredHeightmap = blurHeightmap(heightmap, edges);
            blurredHeightmap = blurHeightmap(blurredHeightmap, edges);

            var finalHeightmap = blurredHeightmap.To((p, xy) =>
                Interpolate.Lerp(p, heightFromColor4(source[xy]), 0.05f)
            );

            var normalsFromH = heightmap.To((_, xy) => normalFromHeightmap(heightmap, xy, maxDistanceToAlpha));
            var normalsFromHBlurred = blurredHeightmap.To((_, xy) => normalFromHeightmap(blurredHeightmap, xy, maxDistanceToAlpha));
            var normalsFromFinalH = finalHeightmap.To((_, xy) => normalFromHeightmap(finalHeightmap, xy, maxDistanceToAlpha));

            yield return new(heightmap.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Heightmap");
            yield return new(blurredHeightmap.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Heightmap Blurred");
            yield return new(blurredHeightmap.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Heightmap Blurred With Details");

            yield return new(normalsFromH.ToSystemBitmap(p => p), "Normal From Heightmap");
            yield return new(normalsFromHBlurred.ToSystemBitmap(p => p), "Normal From Blurred Heightmap");
            yield return new(normalsFromFinalH.ToSystemBitmap(p => p), "Normal From Heightmap Blurred With Details");

            var lightFromTheTopRight = light(normalsFromFinalH, new Vector3(-2, 1, -1));
            var lightFromTheTopLeft = light(normalsFromFinalH, new Vector3(2, 1, -1));
            var lightFromTheBottomLeftBehind = light(normalsFromFinalH, new Vector3(2, -1, 1));
            var lightFromTheBottom = light(normalsFromFinalH, new Vector3(0, -2, -2));

            yield return new(lightFromTheTopRight.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Light from TR");
            yield return new(lightFromTheTopLeft.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Light from TL");
            yield return new(lightFromTheBottomLeftBehind.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Light from BLB");
            yield return new(lightFromTheBottom.ToSystemBitmap(p => Color.FromHSVA(0, 0, p)), "Light from B");

            var litTR = applyLight(lightFromTheTopRight);
            var litTL = applyLight(lightFromTheTopLeft);
            var litBLB = applyLight(lightFromTheBottomLeftBehind);
            var litB = applyLight(lightFromTheBottom);

            yield return new(litTR.ToSystemBitmap(p => p), "Lit from TR");
            yield return new(litTL.ToSystemBitmap(p => p), "Lit from TL");
            yield return new(litBLB.ToSystemBitmap(p => p), "Lit from BLB");
            yield return new(litB.ToSystemBitmap(p => p), "Lit from B");

            yield return new (source.ToSystemBitmap(Color4Extensions.ToColor), "Output");
        }

        private Bitmap<Color> applyLight(Bitmap<float> light)
        {
            return light.To((p, xy) =>
            {
                var c = source[xy].ToColor();
                return (c * p).WithAlpha(c.A);
            });
        }

        private Bitmap<float> light(Bitmap<Color> normals, Vector3 lightDir)
        {
            lightDir = -lightDir.NormalizedSafe();
            return normals.To(
                p =>
                {
                    var normal = p.AsRGBAVector.Xyz * 2 - new Vector3(1);
                    var light = Vector3.Dot(normal, lightDir);
                    return light.Clamped(0, 1);
                });
        }

        private Bitmap<float> blurHeightmap(Bitmap<float> heightmap, Bitmap<float> edges)
        {
            var (width, height) = (heightmap.Width, heightmap.Height);
            var blurH = new Bitmap<float>(width, height);
            var blurV = new Bitmap<float>(width, height);

            var kernelRadius = 5;
            var kernelWeights = new[]
            {
                0.19859610213125314f,
                0.17571363439579307f,
                0.12170274650962626f,
                0.06598396774984912f,
                0.028001560233780885f,
                0.009300040045324049f,
            };

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (edges[x, y] > 0.1)
                    {
                        blurH[x, y] = heightmap[x, y];
                        continue;
                    }

                    var sum = heightmap[x, y] * kernelWeights[0];
                    var weight = kernelWeights[0];

                    for (var k = 1; k <= kernelRadius; k++)
                    {
                        if (x + k >= width)
                            break;
                        sum += heightmap[x + k, y] * kernelWeights[k];
                        weight += kernelWeights[k];
                        if (edges[x + k, y] > 0.1)
                            break;
                    }
                    for (var k = 1; k <= kernelRadius; k++)
                    {
                        if (x - k < 0)
                            break;
                        sum += heightmap[x - k, y] * kernelWeights[k];
                        weight += kernelWeights[k];
                        if (edges[x - k, y] > 0.1)
                            break;
                    }

                    blurH[x, y] = sum / weight;
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (edges[x, y] > 0)
                    {
                        blurV[x, y] = blurH[x, y];
                        continue;
                    }

                    var sum = blurH[x, y] * kernelWeights[0];
                    var weight = kernelWeights[0];

                    for (var k = 1; k <= kernelRadius; k++)
                    {
                        if (y + k >= height)
                            break;
                        sum += blurH[x, y + k] * kernelWeights[k];
                        weight += kernelWeights[k];
                        if (edges[x, y + k] > 0.1)
                            break;
                    }
                    for (var k = 1; k <= kernelRadius; k++)
                    {
                        if (y - k < 0)
                            break;
                        sum += blurH[x, y - k] * kernelWeights[k];
                        weight += kernelWeights[k];
                        if (edges[x, y - k] > 0.1)
                            break;
                    }

                    blurV[x, y] = sum / weight;
                }
            }

            return blurV;
        }

        private float heightFromColor4(Color4 color)
        {
            var c = color.ToColor();

            return c.Value * color.A;
        }

        private Color normalFromHeightmap(Bitmap<float> heightmap, Vector2i xy, float maxDistance)
        {
            if (xy.X == 0 || xy.Y == 0 || xy.X == source.Width - 1 || xy.Y == source.Height - 1)
                return Color.Blue;

            var dx = heightmap[xy + new Vector2i(1, 0)] - heightmap[xy + new Vector2i(-1, 0)];
            var dy = heightmap[xy + new Vector2i(0, 1)] - heightmap[xy + new Vector2i(0, -1)];

            var direction = Vector3.Cross(
                new Vector3(2f / maxDistance, 0, dx),
                new Vector3(0, 2f / maxDistance, dy)
                ).NormalizedSafe();

            return normalToColor(direction);
        }
        private float edgeDetect(Vector2i xy)
        {
            if (xy.X == 0 || xy.Y == 0 || xy.X == source.Width - 1 || xy.Y == source.Height - 1)
                return 0;

            var dx = source[xy + new Vector2i(1, 0)].ToVector() - source[xy + new Vector2i(-1, 0)].ToVector();
            var dy = source[xy + new Vector2i(0, 1)].ToVector() - source[xy + new Vector2i(0, -1)].ToVector();

            var d = dx.LengthSquared + dy.LengthSquared;

            return d > 0.1 ? 1 : 0;
        }

        static Color normalToColor(Vector3 normal)
        {
            normal = normal * 0.5f + new Vector3(0.5f);
            return new Color4(normal.X, normal.Y, normal.Z, 1).ToColor();
        }

        record struct PixelWithDistance(Vector2i Xy, float Distance);

        private Bitmap<PixelWithDistance> getClosestAlphaMap()
        {
            var pixelsToExpand = new Queue<Vector2i>();

            var closestMap = source.To((c, xy) =>
            {
                if (c.A > 0)
                    return new PixelWithDistance(Vector2i.Zero, float.PositiveInfinity);
                pixelsToExpand.Enqueue(xy);
                return new PixelWithDistance(xy, 0);
            });

            while (pixelsToExpand.TryDequeue(out var xy))
            {
                var closest = closestMap[xy.X, xy.Y].Xy;

                tryExpand(closestMap, closest, xy + new Vector2i(0, 1), pixelsToExpand);
                tryExpand(closestMap, closest, xy + new Vector2i(1, 0), pixelsToExpand);
                tryExpand(closestMap, closest, xy + new Vector2i(0, -1), pixelsToExpand);
                tryExpand(closestMap, closest, xy + new Vector2i(-1, 0), pixelsToExpand);
            }

            return closestMap;
        }

        private void tryExpand(Bitmap<PixelWithDistance> closestMap, Vector2i from, Vector2i to, Queue<Vector2i> pixelsToExpand)
        {
            if (!sourceRectangle.Contains(to.X, to.Y))
                return;

            var currentDistance = closestMap[to].Distance;
            var proposedDistance = (from - to).EuclideanLength;

            if (proposedDistance >= currentDistance)
                return;

            closestMap[to] = new PixelWithDistance(from, proposedDistance);
            pixelsToExpand.Enqueue(to);
        }
    }
}
