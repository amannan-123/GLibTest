using SkiaSharp;
using System.Data;

namespace GLibTest
{
    public partial class PathGradientForm : Form
    {
        SKShader? shader;
        SKPath path = new();

        public PathGradientForm()
        {
            InitializeComponent();
        }

        private void BuildShader()
        {
            shader?.Dispose();
            path?.Dispose();

            // Create dynamic path
            path = new SKPath();
            path.MoveTo(0, 0);
            path.LineTo(skglPnl.Width, 0);
            path.LineTo(skglPnl.Width, skglPnl.Height);
            path.LineTo(skglPnl.Width / 2, skglPnl.Height / 2);
            path.LineTo(0, skglPnl.Height);
            path.Close();

            SKPoint[] vertices = path.Points.Take(path.PointCount).ToArray();
            int vertexCount = vertices.Length;

            // Rainbow color stops (7 colors)
            SKColor[] colors =
            [
                SKColors.Violet,
        SKColors.Indigo,
        SKColors.Blue,
        SKColors.Lime,
        SKColors.Yellow,
        SKColors.Orange,
        SKColors.Red
            ];
            float[] positions = [0.0f, 0.2f, 0.4f, 0.5f, 0.6f, 0.8f, 1.0f];

            // Generate dynamic shader code
            string edgeBlocks = "";
            for (int i = 0; i < vertexCount; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= vertexCount) nextIndex = 0;

                edgeBlocks += $@"
            // Edge {i}
            {{
                vec2 v1 = vertices[{i}];
                vec2 v2 = vertices[{nextIndex}];
                vec2 edge = v2 - v1;
                vec2 toPoint = fragCoord - v1;
                float t = clamp(dot(toPoint, edge) / dot(edge, edge), 0.0, 1.0);
                vec2 proj = v1 + t * edge;
                minDist = min(minDist, length(fragCoord - proj));
                
                vec2 mid = (v1 + v2) * 0.5;
                maxEdgeDist = max(maxEdgeDist, length(mid - center));
            }}
        ";
            }

            string shaderCode = $@"
        uniform vec2 vertices[{vertexCount}];
        uniform vec4 colors[7];
        uniform float positions[7];
        const int countStops = 7;

        vec4 main(vec2 fragCoord) {{
            // Calculate true geometric center
            vec2 center = vec2(0.0);
            for(int i = 0; i < {vertexCount}; i++) {{
                center += vertices[i];
            }}
            center /= float({vertexCount});

            // Initialize distance calculations
            float maxEdgeDist = 0.0;
            float minDist = 1e6;

            // Edge calculations
            {edgeBlocks}

            // Normalized position
            float tNorm = clamp(minDist / maxEdgeDist, 0.0, 1.0);

            // Color interpolation
            if(tNorm <= positions[0]) return colors[0];
            for(int j = 1; j < countStops; j++) {{
                if(tNorm <= positions[j]) {{
                    float range = positions[j] - positions[j-1];
                    float lerpT = (tNorm - positions[j-1]) / range;
                    return mix(colors[j-1], colors[j], lerpT);
                }}
            }}
            return colors[countStops-1];
        }}";
            using SKRuntimeEffect shaderResult = SKRuntimeEffect.CreateShader(shaderCode, out string errors)
                ?? throw new Exception($"Shader compilation failed: {errors}");

            using SKRuntimeEffectUniforms uniforms = new(shaderResult)
            {
                ["vertices"] = vertices.SelectMany(v => new[] { v.X, v.Y }).ToArray(),
                ["colors"] = colors.SelectMany(c => new[] {
            c.Red / 255f,
            c.Green / 255f,
            c.Blue / 255f,
            c.Alpha / 255f
        }).ToArray(),
                ["positions"] = positions.OrderBy(p => p).ToArray() // Ensure sorted
            };

            shader = shaderResult.ToShader(uniforms);
        }
        private void PathGradientForm_Load(object sender, EventArgs e)
        {
            //BuildShader();
        }
        private void PathGradientForm_Resize(object sender, EventArgs e)
        {
            //BuildShader();
        }

        private void skglPnl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            //if (shader == null)
            //{
            //    return;
            //}
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Black);
            using SKPaint paint = new();
            paint.Shader = shader;
            //paint.Color = SKColors.Blue;
            //canvas.DrawPath(path, paint);

            // Rainbow colors with custom positions
            var colors = new[]
            {
                SKColors.Red,
                SKColors.Orange,
                SKColors.Yellow,
                SKColors.Lime,
                SKColors.Blue,
                SKColors.Indigo,
                SKColors.Violet
            };

            var positions = new[]
            {
                0.0f, 0.17f, 0.33f, 0.5f, 0.67f, 0.83f, 1.0f
            };

            DrawGradientArc(canvas, 0, 180,  new SKRect(50, 50, 400, 400), colors, positions, 5, 50);
        }

        public static void DrawGradientArc(SKCanvas canvas, float startAngle, float endAngle,
                                        SKRect bounds, SKColor[] colors, float[] positions,
                                        float strokeWidth, float glowIntensity = 0.5f)
        {
            string shaderCode = $@"
        uniform vec2 iResolution;
        uniform vec2 center;
        uniform float startAngle;
        uniform float endAngle;
        uniform vec4 colors[{colors.Length}];
        uniform float positions[{positions.Length}];
        uniform float glowIntensity;

        const float PI = 3.141592653589793;
        const float TAU = 2.0 * PI;

        vec4 getGradientColor(float t) {{
            for(int i = 0; i < {positions.Length - 1}; i++) {{
                if(t >= positions[i] && t <= positions[i+1]) {{
                    float range = positions[i+1] - positions[i];
                    float lerpT = (t - positions[i]) / range;
                    return mix(colors[i], colors[i+1], lerpT);
                }}
            }}
            return colors[{positions.Length - 1}];
        }}

        vec4 main(vec2 fragCoord) {{
            // Convert to centered coordinates
            vec2 uv = (fragCoord - center) / iResolution.yy * 2.0;
            float angle = atan(uv.y, uv.x);
            float radius = length(uv);

            // Map angle to arc range
            float arcStart = radians(startAngle);
            float arcEnd = radians(endAngle);
            float angleNorm = (angle - arcStart) / (arcEnd - arcStart);
            
            // Calculate glow effect
            float beam = (0.7 + 0.5 * cos(angle * 10.0)) * 
                       abs(1.0 / (30.0 * radius)) * 
                       glowIntensity * 2.0;

            // Get gradient color
            vec4 color = getGradientColor(clamp(angleNorm, 0.0, 1.0));
            
            // Fade edges and combine with glow
            float alpha = smoothstep(-0.2, 0.0, angleNorm) * 
                        smoothstep(1.2, 1.0, angleNorm) * 
                        smoothstep(0.0, 0.2, beam);
            
            return vec4(color.rgb * (1.0 + beam), alpha);
        }}";

            // Compile shader
            var effect = SKRuntimeEffect.CreateShader(shaderCode, out var errors);
            if (effect == null) throw new Exception(errors);

            // Prepare uniforms
            var center = new SKPoint(bounds.MidX, bounds.MidY);
            var uniforms = new SKRuntimeEffectUniforms(effect)
            {
                ["iResolution"] = new[] { bounds.Width, bounds.Height },
                ["center"] = new[] { center.X, center.Y },
                ["startAngle"] = startAngle,
                ["endAngle"] = endAngle,
                ["colors"] = colors.SelectMany(c => new[] { c.Red / 255f, c.Green / 255f, c.Blue / 255f, c.Alpha / 255f }).ToArray(),
                ["positions"] = positions,
                ["glowIntensity"] = glowIntensity
            };

            using var paint = new SKPaint
            {
                Shader = effect.ToShader(uniforms),
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                ColorFilter = SKColorFilter.CreateBlendMode(SKColors.Transparent, SKBlendMode.SrcOver)
            };

            using var path = new SKPath();
            path.AddArc(bounds, startAngle, endAngle - startAngle);
            canvas.DrawPath(path, paint);
        }
    }
}