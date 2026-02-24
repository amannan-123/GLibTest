using SkiaSharp;
using System.Data;
using System.Diagnostics;

namespace GLibTest
{
    public partial class SkiaTestForm : Form
    {
        public SkiaTestForm()
        {
            InitializeComponent();
        }

        public static SKColor HslToColor(float h, float s, float l)
        {
            float r, g, b;
            if (s == 0)
            {
                // Achromatic (grey)
                r = g = b = l;
            }
            else
            {
                float q = l < 0.5f ? l * (1 + s) : l + s - l * s;
                float p = 2 * l - q;
                r = HueToRgb(p, q, h + 1.0f / 3.0f);
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - 1.0f / 3.0f);
            }
            return new SKColor(
                (byte)(r * 255),
                (byte)(g * 255),
                (byte)(b * 255),
                255);
        }

        private static float HueToRgb(float p, float q, float t)
        {
            if (t < 0)
                t += 1;
            if (t > 1)
                t -= 1;
            if (t < 1.0f / 6.0f)
                return p + (q - p) * 6 * t;
            if (t < 1.0f / 2.0f)
                return q;
            if (t < 2.0f / 3.0f)
                return p + (q - p) * (2.0f / 3.0f - t) * 6;
            return p;
        }

        public void DrawRainbowCircle(SKCanvas canvas, SKPoint center, float radius)
        {
            // Number of vertices to approximate a circle
            const int segments = 360;
            var vertices = new List<SKPoint>();
            var colors = new List<SKColor>();

            // Center point (white)
            vertices.Add(center);
            colors.Add(SKColors.White);

            // Generate perimeter points with rainbow colors
            for (int i = 0; i <= segments; i++)
            {
                double angle = 2 * Math.PI * i / segments;
                SKPoint point = new(
                    center.X + radius * (float)Math.Cos(angle),
                    center.Y + radius * (float)Math.Sin(angle)
                );
                vertices.Add(point);

                // HSV color based on angle
                colors.Add(SKColor.FromHsl((float)(i * 360 / segments), 100, 50));
            }

            // Create paint without shader (vertex colors will be used directly)
            using var paint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColors.White // Important: Use white to preserve vertex colors
            };

            // Draw using triangle fan mode
            canvas.DrawVertices(
                SKVertexMode.TriangleFan,
                vertices.ToArray(),
                colors.ToArray(),
                paint
            );
        }

        private void skglPnl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {

            // Create a new surface for offscreen drawing using the same context
            SKImageInfo info = new(e.Info.Width, e.Info.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var offscreenSurface = SKSurface.Create(skglPnl.GRContext, true, info);
            SKCanvas canvas = offscreenSurface.Canvas;

            // Clear the canvas
            canvas.Clear(SKColors.White);

            // Create a linear gradient
            //SKPoint startPoint = new(info.Width * 0.25f, info.Height * 0.25f);
            //SKPoint endPoint = new(info.Width * 0.75f, info.Height * 0.75f);
            //var gradientColors = new[] { SKColors.Black, new SKColor(47, 87, 121), SKColors.Black, new SKColor(47, 87, 121), SKColors.Black };
            //var gradientPositions = new[] { 0f, 0.25f, 0.5f, 0.75f, 1f };

            //paint.Shader = SKShader.CreateLinearGradient(startPoint, endPoint, gradientColors, gradientPositions, SKShaderTileMode.Clamp);


            // Set anti-aliasing
            using var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            SKColor _topLeftColor = SKColors.Cyan;
            SKColor _topRightColor = SKColors.Magenta;
            SKColor _bottomLeftColor = SKColors.Yellow;
            SKColor _bottomRightColor = SKColors.Black;
            var positions = new[]
            {
                new SKPoint(info.Width/2f, 0),
                new SKPoint(info.Width/2f, info.Height*0.25f),
                new SKPoint(info.Width/2f, info.Height*0.5f),
                new SKPoint(info.Width/2f, info.Height*0.75f),
                new SKPoint(info.Width/2f, info.Height),
                new SKPoint(0, 0),
                new SKPoint(0, info.Height*0.25f),
                new SKPoint(0, info.Height*0.5f),
                new SKPoint(0, info.Height*0.75f),
                new SKPoint(0, info.Height),
                new SKPoint(info.Width, 0),
                new SKPoint(info.Width, info.Height*0.25f),
                new SKPoint(info.Width, info.Height*0.5f),
                new SKPoint(info.Width, info.Height*0.75f),
                new SKPoint(info.Width, info.Height)
                //new SKPoint(0, 0),
                //new SKPoint(info.Width, 0),
                //new SKPoint(0, info.Height),
                //new SKPoint(info.Width, info.Height)
            };
            int numColors = 15;
            float saturation = 0.9f;
            float lightness = 0.5f;

            var uniqueColors = Enumerable.Range(0, numColors)
                .Select(i =>
                {
                    // Evenly distribute hue between 0 and 1.
                    float hue = (float)i / numColors;
                    return HslToColor(hue, saturation, lightness);
                })
                .ToArray();

            // Define colors for each point
            SKColor[] colors = uniqueColors;

            // Convert colors to float arrays (RGBA)
            var colorFloats = colors.Select(c => new[]
            {
                c.Red / 255f,
                c.Green / 255f,
                c.Blue / 255f,
                c.Alpha / 255f
            }).ToArray();
            int numPoints = positions.Length;

            //            string FragmentShaderSource = $@"
            //const int countPoints = {numPoints};
            //uniform vec2 positions[countPoints];
            //uniform vec4 colors[countPoints];

            //vec4 main(vec2 fragCoord) {{
            //    vec2 pos = fragCoord.xy;
            //    float totalWeight = 0.0;
            //    vec4 result = vec4(0.0);
            //    const float smoothing = 0.1;  // Prevents division by zero

            //    // Compute weighted color blending
            //    for (int i = 0; i < countPoints; i++) {{
            //        float dist = distance(pos, positions[i]) + smoothing;
            //        float weight = 1.0 / (dist * dist);

            //        totalWeight += weight;
            //        result += colors[i] * weight;
            //    }}

            //    return result / totalWeight;  // Normalize color blending
            //}}

            //";

            //            string FragmentShaderSource = $@"
            //uniform vec2 p0;  // First point of the triangle
            //uniform vec2 p1;  // Second point of the triangle
            //uniform vec2 p2;  // Third point of the triangle

            //uniform vec4 c0;  // Color at p0
            //uniform vec4 c1;  // Color at p1
            //uniform vec4 c2;  // Color at p2

            //vec3 barycentric(vec2 p, vec2 a, vec2 b, vec2 c) {{
            //    vec2 v0 = b - a, v1 = c - a, v2 = p - a;
            //    float d00 = dot(v0, v0);
            //    float d01 = dot(v0, v1);
            //    float d11 = dot(v1, v1);
            //    float d20 = dot(v2, v0);
            //    float d21 = dot(v2, v1);
            //    float denom = d00 * d11 - d01 * d01;

            //    float v = (d11 * d20 - d01 * d21) / denom;
            //    float w = (d00 * d21 - d01 * d20) / denom;
            //    float u = 1.0 - v - w;

            //    return vec3(u, v, w);
            //}}

            //vec4 main(vec2 fragCoord) {{
            //    vec3 bary = barycentric(fragCoord, p0, p1, p2);

            //    // If barycentric coordinates are outside [0,1], it's outside the triangle
            //    if (bary.x < 0.0 || bary.y < 0.0 || bary.z < 0.0) {{
            //        return half4(0.0, 0.0, 0.0, 0.0);  // Fully transparent
            //    }}

            //    // Interpolate colors using barycentric coordinates
            //    return bary.x * c0 + bary.y * c1 + bary.z * c2;
            //}}
            //";

            const string shaderCode = @"
uniform vec2 vertices[4];      // The four corners of the rectangle, in order:
                               // 0: Top-Left, 1: Top-Right, 2: Bottom-Right, 3: Bottom-Left
uniform vec4 colors[7];        // Color stops (e.g. center to edge; you can supply 7 stops)
uniform float positions[7];    // Position stops (values between 0 and 1, sorted ascending)
const int countStops = 7;      // Number of color stops

vec4 main(vec2 fragCoord) {
    // --- Compute rectangle bounds and center ---
    float left   = min(min(vertices[0].x, vertices[1].x), min(vertices[2].x, vertices[3].x));
    float right  = max(max(vertices[0].x, vertices[1].x), max(vertices[2].x, vertices[3].x));
    float top    = min(min(vertices[0].y, vertices[1].y), min(vertices[2].y, vertices[3].y));
    float bottom = max(max(vertices[0].y, vertices[1].y), max(vertices[2].y, vertices[3].y));
    vec2 center = (vertices[0] + vertices[1] + vertices[2] + vertices[3]) / 4.0;

    // --- Compute the normalized radial distance ---
    // Compute the vector and distance from the center to the fragment:
    vec2 d = fragCoord - center;
    float dist = length(d);
    if (dist == 0.0) {
        // At the center, return the color corresponding to position 0.
        return colors[0];
    }
    vec2 nd = d / dist; // normalized direction

    // Compute the intersection of the ray (center + t*nd) with the rectangle boundaries.
    // For each edge, solve for t where the ray meets the edge and take the smallest positive t.
    float tMin = 1e9;
    // Left edge: x = left
    if (nd.x != 0.0) {
        float tCandidate = (left - center.x) / nd.x;
        if (tCandidate > 0.0) tMin = min(tMin, tCandidate);
    }
    // Right edge: x = right
    if (nd.x != 0.0) {
        float tCandidate = (right - center.x) / nd.x;
        if (tCandidate > 0.0) tMin = min(tMin, tCandidate);
    }
    // Top edge: y = top
    if (nd.y != 0.0) {
        float tCandidate = (top - center.y) / nd.y;
        if (tCandidate > 0.0) tMin = min(tMin, tCandidate);
    }
    // Bottom edge: y = bottom
    if (nd.y != 0.0) {
        float tCandidate = (bottom - center.y) / nd.y;
        if (tCandidate > 0.0) tMin = min(tMin, tCandidate);
    }
    vec2 boundary = center + nd * tMin;
    float maxDist = length(boundary - center);

    // Normalized radial coordinate: 0 at center, 1 at boundary.
    float tNorm = clamp(dist / maxDist, 0.0, 1.0);

    // --- Color interpolation based on tNorm ---
    // Find the segment in which tNorm falls and interpolate.
    for (int j = 0; j < countStops - 1; j++) {
        if (tNorm >= positions[j] && tNorm <= positions[j + 1]) {
            float range = positions[j + 1] - positions[j];
            float lerpT = (tNorm - positions[j]) / range;
            return mix(colors[j], colors[j + 1], lerpT);
        }
    }
    return colors[countStops - 1];
}


";

            using SKRuntimeEffect shaderResult = SKRuntimeEffect.CreateShader(shaderCode, out var errors);
            if (shaderResult == null)
            {
                throw new Exception("Shader compilation failed: " + errors);
            }
            SKPoint p0 = new(0, 0);
            SKPoint p1 = new(info.Width, 0);
            SKPoint p2 = new(0, info.Height);
            // Pre-process path vertices during initialization
            var cachedVertices = new SKPoint[]
            {
                new(0, 0),
                new(info.Width, 0),
                new(info.Width, info.Height),
                new(0, info.Height)
            };

            var cachedColors = new SKColor[]
            {
                SKColors.Violet,
                SKColors.Indigo,
                SKColors.Blue,
                SKColors.Lime,
                SKColors.Yellow,
                SKColors.Orange,
                SKColors.Red
            };

            var cachedPositions = new float[]
            {
                0.0f,
                0.2f,
                0.4f,
                0.5f,
                0.6f,
                0.8f,
                1.0f
            };

            // Reuse cached vertices when creating shader
            var uniforms = new SKRuntimeEffectUniforms(shaderResult);
            uniforms["vertices"] = cachedVertices.SelectMany(p => new[] { p.X, p.Y }).ToArray();
            uniforms["colors"] = cachedColors.SelectMany(c => new[] { c.Red / 255f, c.Green / 255f, c.Blue / 255f, c.Alpha / 255f }).ToArray();
            uniforms["positions"] = cachedPositions;

            // Define colors for each vertex
            SKColor c0 = SKColors.Cyan;
            SKColor c1 = SKColors.Magenta;
            SKColor c2 = SKColors.Yellow;
            //var uniforms = new SKRuntimeEffectUniforms(shaderResult)
            //{
            //    //{ "topLeftColor", ToFloatArray(_topLeftColor) },
            //    //{ "topRightColor", ToFloatArray(_topRightColor) },
            //    //{ "bottomLeftColor", ToFloatArray(_bottomLeftColor) },
            //    //{ "bottomRightColor", ToFloatArray(_bottomRightColor) },
            //    //{ "resolution", new float[] { info.Width, info.Height } }
            //    //{"positions", positions.SelectMany(p => new[] { p.X, p.Y }).ToArray() },
            //    //{ "colors", colorFloats.SelectMany(c => c).ToArray() },
            //    //{ "maxRadius", 40f   },
            //    //{"falloff", 5f }
            //    //["p0"] = p0,
            //    //["p1"] = p1,
            //    //["p2"] = p2,
            //    //["c0"] = ToFloatArray(c0),
            //    //["c1"] = ToFloatArray(c1),
            //    //["c2"] = ToFloatArray(c2)
            //};

            // Create the shader
            var shader = shaderResult.ToShader(uniforms);
            paint.Shader = shader;

            var path = new SKPath();
            path.MoveTo(0, 0);
            path.LineTo(info.Width, 0);
            path.LineTo(info.Width, info.Height);
            path.LineTo(0, info.Height);
            path.Close();
            canvas.DrawPath(path, paint);

            // Fill the canvas with the gradient
            //canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paint);

            // triangle
            // Draw the triangle using a path
            //using var path = new SKPath();
            //path.MoveTo(p0);
            //path.LineTo(p1);
            //path.LineTo(p2);
            //path.Close();

            //canvas.DrawPath(path, paint);

            // draw vertices
            //uniforms["p0"] = new SKPoint(info.Width, 0);
            //uniforms["p1"] = new SKPoint(info.Width, info.Height);
            //uniforms["p2"] = new SKPoint(0, info.Height);
            //uniforms["c0"] = ToFloatArray(c1);
            //uniforms["c1"] = ToFloatArray(c0);
            //uniforms["c2"] = ToFloatArray(c2);
            //shader = shaderResult.ToShader(uniforms);
            //paint.Shader = shader;
            //canvas.DrawVertices(SKVertexMode.Triangles, new SKPoint[] {
            //   new SKPoint(info.Width,0),
            //    new SKPoint(info.Width,info.Height),
            //    new SKPoint(0,info.Height)
            //}, new SKColor[] { SKColors.Red,
            //SKColors.Blue,
            //SKColors.Lime
            //}, paint);

            //paint.Shader = null;
            //paint.Color = SKColors.Pink;
            //paint.Style = SKPaintStyle.Fill;
            //paint.IsAntialias = true;

            //            var vertices = new SKPoint[] {
            //    new SKPoint(0, 0),    // Triangle 1: 0, 1, 2
            //    new SKPoint(100, 0),  // Triangle 2: 1, 2, 3
            //    new SKPoint(50, 100), // Triangle 3: 2, 3, 4
            //    new SKPoint(150, 100),
            //    new SKPoint(100, 200)
            //};
            //            canvas.DrawVertices(SKVertexMode.TriangleStrip, vertices, [
            //                SKColors.Cyan,
            //                SKColors.Magenta,
            //                SKColors.Yellow,
            //                SKColors.Black,
            //                SKColors.Red
            //                ], paint);

            //DrawRainbowCircle(canvas, new SKPoint(info.Width / 2, info.Height / 2), Math.Min(info.Width, info.Height) / 2f);

            shader.Dispose();
            //// Draw a filled rectangle
            //SKPoint center = new(info.Width / 2f, info.Height / 2f);
            //float rectSize = 100;
            //SKRect rect = new(center.X - rectSize, center.Y - rectSize, center.X, center.Y);
            //paint.Color = new SKColor(0, 0, 255, 204); // Semi-transparent blue
            //paint.Shader = null; // Reset shader for solid colors
            //canvas.DrawRect(rect, paint);

            //float radiusX = info.Width / 4f;
            //float radiusY = info.Height / 4f;

            //// Draw a filled ellipse overlapping the rectangle
            //paint.Color = new SKColor(255, 0, 0, 128); // Semi-transparent red
            //canvas.DrawOval(center, new SKSize(radiusX, radiusY), paint);

            //// Draw a border rectangle
            //rect = new SKRect(center.X - radiusX, center.Y - radiusY, center.X + radiusX, center.Y + radiusY);
            //paint.Style = SKPaintStyle.Stroke;
            //paint.StrokeWidth = 2;
            //paint.Color = SKColors.Blue;
            //canvas.DrawRect(rect, paint);

            //// Apply transformations (skew and rotation)
            //var skewMatrix = SKMatrix.CreateSkew(0.5f, 0.5f);
            //// also translate to 0,0
            //skewMatrix.TransX = rect.Left;
            //skewMatrix.TransY = rect.Top;
            //canvas.SetMatrix(skewMatrix);
            //paint.Color = SKColors.Lime;
            //var newRect = new SKRect(0, 0, rect.Width, rect.Height);
            //canvas.DrawRect(newRect, paint);

            //// Apply rotation
            //var rotationMatrix = skewMatrix.PostConcat(SKMatrix.CreateRotation((float)(90 * Math.PI / 180), center.X, center.Y));
            //canvas.SetMatrix(rotationMatrix);
            //paint.Color = SKColors.Red;
            //canvas.DrawRect(newRect, paint);

            //// Reset transformations
            //canvas.ResetMatrix();

            //// Draw a filled arc
            //paint.Style = SKPaintStyle.Stroke;
            //paint.StrokeWidth = 10;
            //paint.Color = SKColors.Yellow.WithAlpha(128);
            //// set cap style
            //paint.StrokeCap = SKStrokeCap.Round;
            //canvas.DrawArc(rect, 0, -180, false, paint);

            //// Draw a pie
            //paint.Color = SKColors.Cyan.WithAlpha(128);
            //// set stroke style
            //paint.Style = SKPaintStyle.Stroke;
            //paint.StrokeJoin = SKStrokeJoin.Round;
            //canvas.DrawArc(rect, 0, 270, true, paint);

            //// Draw another rect
            //rect = new SKRect(center.X, center.Y, center.X + rectSize, center.Y + rectSize);
            //paint.Color = SKColors.Lime.WithAlpha(128);
            //paint.Style = SKPaintStyle.Fill;
            //canvas.DrawRect(rect, paint);

            //// Draw a vertical line
            //paint.StrokeWidth = 5;
            //// reset path effect
            //paint.PathEffect = null;
            //canvas.DrawLine(new SKPoint(info.Width / 2, 0), new SKPoint(info.Width / 2, info.Height), paint);

            //// Draw a star (custom path)
            //paint.Style = SKPaintStyle.Fill;
            //paint.Color = SKColors.Black.WithAlpha(128);
            //using var starPath = CreateStarPath(center, info.Width / 4f, info.Width / 8f, 5);
            ////canvas.DrawPath(starPath, paint);

            //// Apply a blur effect
            //using var blurFilter = SKImageFilter.CreateBlur(5, 5);
            e.Surface.Canvas.DrawImage(offscreenSurface.Snapshot(), 0, 0, new SKPaint
            {
                //ImageFilter = blurFilter
            });

            // Draw a red border around the surface
            paint.Color = SKColors.Red;
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 10;
            //e.Surface.Canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paint);

            // Draw a horizontal middle line
            paint.Color = SKColors.Red.WithAlpha(128);
            paint.StrokeWidth = 5;
            //e.Surface.Canvas.DrawLine(new SKPoint(0, info.Height / 2), new SKPoint(info.Width, info.Height / 2), paint);
            Debug.WriteLine("SK Drawing complete");
        }

        public static float[] ToFloatArray(SKColor color)
        {
            return new float[]
            {
        color.Red / 255f,
        color.Green / 255f,
        color.Blue / 255f,
        color.Alpha / 255f
            };
        }


        private SKPath CreateStarPath(SKPoint center, float outerRadius, float innerRadius, int points)
        {
            var path = new SKPath();
            float angleStep = 360f / (points * 2);
            for (int i = 0; i < points * 2; i++)
            {
                float angle = angleStep * i;
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                float x = center.X + radius * (float)Math.Cos(angle * Math.PI / 180);
                float y = center.Y + radius * (float)Math.Sin(angle * Math.PI / 180);
                if (i == 0)
                    path.MoveTo(x, y);
                else
                    path.LineTo(x, y);
            }
            path.Close();
            return path;
        }

        private void SkiaTestForm_Load(object sender, EventArgs e)
        {
            //new D2DTestForm().Show();
        }

        private void SkiaTestForm_Click(object sender, EventArgs e)
        {
            var currentTime = DateTime.Now;
            skglPnl.Invalidate();
            Application.DoEvents();
            Debug.WriteLine($"Time taken: {DateTime.Now - currentTime}");
            Text = Location.ToString();
        }
    }
}
