using GLibTest.PathHelpers;
using GLibTest.GeometryHelpers;
using System.Numerics;
using Vortice.Direct2D1;
using Vortice.Direct2D1.Effects;
using Vortice.Mathematics;
using System.Diagnostics;

namespace GLibTest
{
    public partial class D2DTestForm : Form
    {
        public D2DTestForm()
        {
            InitializeComponent();
        }

        private void d2dControl1_OnRendering(object sender, ID2D1DeviceContext6 g)
        {
            ID2D1DeviceContext6 deviceContext = g;
            if (deviceContext == null) return;

            g.AntialiasMode = AntialiasMode.PerPrimitive;
            g.TextAntialiasMode = TextAntialiasMode.Grayscale;

            // Create a Linear Gradient Brush
            LinearGradientBrushProperties lgbp = new()
            {
                StartPoint = new Vector2(deviceContext.PixelSize.Width * 0.25f, deviceContext.PixelSize.Height * 0.25f),
                EndPoint = new Vector2(deviceContext.PixelSize.Width * 0.75f, deviceContext.PixelSize.Height * 0.75f)
            };
            GradientStop[] stops = [
                new(0, new Color4(0, 0, 0)),
                new(0.25f, new Color4(0.1842f, 0.3421f, 0.4737f)),
                new(0.5f, new Color4(0, 0, 0)),
                new(0.75f, new Color4(0.1842f, 0.3421f, 0.4737f)),
                new(1, new Color4(0, 0, 0)),
            ];
            ID2D1GradientStopCollection gradientStopCollection = deviceContext.CreateGradientStopCollection(stops);
            using ID2D1LinearGradientBrush lgb = deviceContext.CreateLinearGradientBrush(lgbp, gradientStopCollection);

            //deviceContext.FillRectangle(new Rect(0, 0, deviceContext.PixelSize.Width, deviceContext.PixelSize.Height), lgb);

            // Create a compatible render target for offscreen drawing
            using ID2D1BitmapRenderTarget bitmapRenderTarget = deviceContext.CreateCompatibleRenderTarget(
                new SizeI((int)deviceContext.Size.Width, (int)deviceContext.Size.Height),
                new SizeI((int)deviceContext.Size.Width, (int)deviceContext.Size.Height),
                new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));

            bitmapRenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            bitmapRenderTarget.TextAntialiasMode = TextAntialiasMode.Grayscale;

            using ID2D1SolidColorBrush solidColorBrush = bitmapRenderTarget.CreateSolidColorBrush(new Color4(0f, 0f, 1f, 0.8f));
            using ID2D1SolidColorBrush lineBrush = bitmapRenderTarget.CreateSolidColorBrush(new Color4(0, 1, 0, 0.5f));

            // Begin drawing to the bitmap render target
            bitmapRenderTarget.BeginDraw();

            // Assume these are your desired colors (normalized to 0-1 range)
            // cyan, magenta, yellow, black
            Color4 topLeftColor = new(0, 1, 1, 1);
            Color4 topRightColor = new(1, 0, 1, 1);
            Color4 bottomLeftColor = new(1, 1, 0, 1);
            Color4 bottomRightColor = new(0, 0, 0, 1);

            float width = deviceContext.PixelSize.Width;
            float height = deviceContext.PixelSize.Height;
            GradientMeshPatch patch = new()
            {
                // Row 0 (top edge)
                Point00 = new Vector2(0, 0),
                Point01 = new Vector2(0, 0),
                Point02 = new Vector2(width, 0),
                Point03 = new Vector2(width, 0),

                // Row 1
                Point10 = new Vector2(0, 0),
                Point11 = new Vector2(0, 0),
                Point12 = new Vector2(width, 0),
                Point13 = new Vector2(width, 0),

                // Row 2
                Point20 = new Vector2(0, height),
                Point21 = new Vector2(0, height),
                Point22 = new Vector2(width, height),
                Point23 = new Vector2(width, height),

                // Row 3 (bottom edge)
                Point30 = new Vector2(0, height),
                Point31 = new Vector2(0, height),
                Point32 = new Vector2(width, height),
                Point33 = new Vector2(width, height),

                Color00 = topLeftColor,
                Color03 = topRightColor,
                Color33 = bottomRightColor,
                Color30 = bottomLeftColor,

                BottomEdgeMode = PatchEdgeMode.Aliased,
                LeftEdgeMode = PatchEdgeMode.Aliased,
                RightEdgeMode = PatchEdgeMode.Aliased,
                TopEdgeMode = PatchEdgeMode.Aliased,
            };

            // Create an array containing just one patch.
            GradientMeshPatch[] patches = [patch];

            // Create the gradient mesh from the patch data.
            ID2D1GradientMesh gradientMesh = deviceContext.CreateGradientMesh(patches, 1);

            // Now fill the entire drawing area with the gradient mesh.
            deviceContext.DrawGradientMesh(gradientMesh);

            Vector2 center = new(bitmapRenderTarget.Size.Width / 2f, bitmapRenderTarget.Size.Height / 2f);
            float radiusX = bitmapRenderTarget.Size.Width / 4f;
            float radiusY = bitmapRenderTarget.Size.Height / 4f;

            // Draw a filled rectangle
            RectangleF rect = new(center.X - 100, center.Y - 100, 100, 100);
            //bitmapRenderTarget.FillRectangle(rect, solidColorBrush);

            // Draw a filled ellipse overlapping the rectangle
            solidColorBrush.Color = new Color4(1f, 0f, 0f, 0.5f);
            //bitmapRenderTarget.FillEllipse(new Ellipse(center, radiusX, radiusY), solidColorBrush);

            // Define the rectangle around the ellipse
            RectangleF rectt = new(center.X - radiusX, center.Y - radiusY, radiusX * 2, radiusY * 2);

            // Draw the rectangle (border only)
            using ID2D1SolidColorBrush borderBrush = bitmapRenderTarget.CreateSolidColorBrush(new Color4(0f, 0f, 1f, 1f));
            //bitmapRenderTarget.DrawRectangle(rectt, borderBrush, 2f);

            // Skew transformation matrix
            Matrix3x2 skewMatrix = new(
                1.0f, 0.5f, // SkewY
                0.5f, 1.0f, // SkewX
                rectt.X, rectt.Y  // Translation
            );

            // Rotation transformation matrix
            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation((float)(90 * Math.PI / 180), center);

            // Save the current transform
            Matrix3x2 originalTransform = bitmapRenderTarget.Transform;

            // Apply skew transformations
            //bitmapRenderTarget.Transform *= skewMatrix;

            // Draw the skewed rectangle
            using ID2D1SolidColorBrush skewedBorderBrush = bitmapRenderTarget.CreateSolidColorBrush(new Color4(0f, 1f, 0f, 1f));
            //bitmapRenderTarget.DrawRectangle(new RectangleF(0, 0, rectt.Width, rectt.Height), skewedBorderBrush, 2f);

            // Apply rotation transformations
            //bitmapRenderTarget.Transform *= rotationMatrix;

            // Draw the rotated rectangle
            using ID2D1SolidColorBrush rotatedBorderBrush = bitmapRenderTarget.CreateSolidColorBrush(new Color4(1f, 0f, 0f, 1f));
            bitmapRenderTarget.DrawRectangle(new RectangleF(0, 0, rectt.Width, rectt.Height), rotatedBorderBrush, 2f);

            // Restore the original transform
            //bitmapRenderTarget.Transform = originalTransform;

            // Draw an arc geometry
            solidColorBrush.Color = new Color4(1f, 1f, 0f, 0.5f);
            using ID2D1PathGeometry? arcGeometry = GeometryHelper.CreateArc(bitmapRenderTarget, center.X - radiusX, center.Y - radiusY, radiusX * 2, radiusY * 2, 0, -180);
            using ID2D1StrokeStyle strokeStyle = bitmapRenderTarget.Factory.CreateStrokeStyle(new StrokeStyleProperties
            {
                StartCap = CapStyle.Round,
                EndCap = CapStyle.Round,
            });
            if (arcGeometry != null)
            {
                bitmapRenderTarget.DrawGeometry(arcGeometry, solidColorBrush, 10f, strokeStyle);
            }

            // Draw a pie geometry
            solidColorBrush.Color = new Color4(0f, 1f, 1f, 0.5f);
            using ID2D1PathGeometry? pieGeometry = GeometryHelper.CreatePie(bitmapRenderTarget, center.X - radiusX, center.Y - radiusY, radiusX * 2, radiusY * 2, 0, 270);
            ID2D1StrokeStyle strokeStyle2 = bitmapRenderTarget.Factory.CreateStrokeStyle(new StrokeStyleProperties
            {
                LineJoin = LineJoin.Round,
            });
            if (pieGeometry != null)
            {
                //bitmapRenderTarget.DrawGeometry(pieGeometry, solidColorBrush, 10f, strokeStyle2);
                //DrawPathPointsWithIndices(bitmapRenderTarget, pieGeometry);
            }

            strokeStyle2.Dispose();

            // Draw another filled rectangle
            rect = new RectangleF(center.X, center.Y, 100, 100);
            solidColorBrush.Color = new Color4(0f, 1f, 0f, 0.5f);
            //bitmapRenderTarget.FillRectangle(rect, solidColorBrush);

            // Draw a vertical middle line
            //bitmapRenderTarget.DrawLine(
            //    new Vector2(bitmapRenderTarget.Size.Width / 2, 0),
            //    new Vector2(bitmapRenderTarget.Size.Width / 2, bitmapRenderTarget.Size.Height),
            //lineBrush, 5);

            // Draw a star geometry
            solidColorBrush.Color = new Color4(0, 0, 0, 0.5f);
            ID2D1PathGeometry1? starGeometry = GeometryHelper.CreateStar(bitmapRenderTarget, center, radiusX - (radiusX / 2f), radiusY - (radiusY / 2f), radiusX, radiusY, 10);
            if (starGeometry != null)
            {
                //bitmapRenderTarget.FillGeometry(starGeometry, solidColorBrush);
            }

            // End offscreen drawing
            bitmapRenderTarget.EndDraw();

            // Apply a Gaussian blur effect
            using ID2D1Bitmap bitmap = bitmapRenderTarget.Bitmap;
            using GaussianBlur effect = new(deviceContext);
            effect.SetValue((int)GaussianBlurProperties.StandardDeviation, 0f);
            effect.SetInput(0, bitmap, false);

            DropShadow.Register(d2dControl1.Direct2DFactory);
            using DropShadow dropShadow = new(deviceContext);
            dropShadow.ShadowBlur = 10f;
            dropShadow.SetInput(0, effect.Output, false);

            // Draw the blurred image to the render target
            deviceContext.DrawImage(dropShadow, InterpolationMode.HighQualityCubic, CompositeMode.SourceOver);
            // Draw a border around the render target
            using ID2D1SolidColorBrush borderBrushh = deviceContext.CreateSolidColorBrush(new Color4(1, 0, 0, 1));
            //deviceContext.DrawRectangle(new RectangleF(0, 0, deviceContext.Size.Width, deviceContext.Size.Height), borderBrushh, 10f);

            // Draw a horizontal middle line
            using ID2D1SolidColorBrush lineBrush2 = deviceContext.CreateSolidColorBrush(new Color4(1, 0, 0, 0.5f));
            //deviceContext.DrawLine(
            //    new Vector2(0, deviceContext.Size.Height / 2),
            //    new Vector2(deviceContext.Size.Width, deviceContext.Size.Height / 2),
            //    lineBrush2, 5);
            Debug.WriteLine("D2D Drawing complete");
        }

        public static void DrawPathPointsWithIndices(ID2D1RenderTarget renderTarget, ID2D1PathGeometry geometry)
        {
            CustomGeometrySink sink = new(renderTarget);
            geometry.Simplify(GeometrySimplificationOption.CubicsAndLines, sink);
            sink.Draw();
        }

        private void D2DTestForm_Click(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            d2dControl1.Invalidate();
            Application.DoEvents();
            Debug.WriteLine($"Time taken: {DateTime.Now - currentTime}");
            Text = Location.ToString();
        }
    }

    public class DropShadow(ID2D1DeviceContext context) : ID2D1Effect(context.CreateEffect(typeof(Implementation).GUID))
    {
        public static void Register(ID2D1Factory7 d2Factory)
        {
            d2Factory.RegisterEffect<Implementation>();
        }

        public float ShadowBlur
        {
            set => SetValue(0, value);
            get => GetFloatValue(0);
        }

        [CustomEffect(1)]
        sealed class Implementation : CustomEffectBase
        {
            [CustomEffectProperty(PropertyType.Float, 0)]
            public float ShadowBlur { get; set; } = 5f;

            Shadow? ShadowEffect;

            public override void Initialize(ID2D1EffectContext effectContext, ID2D1TransformGraph transformGraph)
            {
                ShadowEffect = new Shadow(effectContext) { BlurStandardDeviation = ShadowBlur };
                Composite CompositeEffect = new(effectContext);

                ID2D1TransformNode ShadowTransform = effectContext.CreateTransformNodeFromEffect(ShadowEffect);
                ID2D1TransformNode OffsetTransform = effectContext.CreateOffsetTransform(new Int2(10, 10));
                ID2D1TransformNode CompositeTransform = effectContext.CreateTransformNodeFromEffect(CompositeEffect);

                transformGraph.AddNode(ShadowTransform);
                transformGraph.AddNode(OffsetTransform);
                transformGraph.AddNode(CompositeTransform);

                transformGraph.ConnectToEffectInput(0, ShadowTransform, 0);
                transformGraph.ConnectNode(ShadowTransform, OffsetTransform, 0);
                transformGraph.ConnectNode(OffsetTransform, CompositeTransform, 0);
                transformGraph.ConnectToEffectInput(0, CompositeTransform, 1);
                transformGraph.SetOutputNode(CompositeTransform);
            }

            public override void SetGraph(ID2D1TransformGraph transformGraph) { }

            public override void PrepareForRender(ChangeType changeType)
            {
                if (changeType == ChangeType.Properties)
                    ShadowEffect!.BlurStandardDeviation = ShadowBlur;
            }
        }
    }

}