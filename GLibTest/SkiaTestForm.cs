using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GLibTest
{
    public partial class SkiaTestForm : Form
    {
        public SkiaTestForm()
        {
            InitializeComponent();
        }

        private void skglPnl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {

            // Create a new surface for offscreen drawing using the same context
            SKImageInfo info = new(e.Info.Width, e.Info.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var offscreenSurface = SKSurface.Create(skglPnl.GRContext, true, info);
            SKCanvas canvas = offscreenSurface.Canvas;

            // Clear the canvas
            canvas.Clear(SKColors.White);

            // Set anti-aliasing
            using var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            // Create a linear gradient
            SKPoint startPoint = new(info.Width * 0.25f, info.Height * 0.25f);
            SKPoint endPoint = new(info.Width * 0.75f, info.Height * 0.75f);
            var gradientColors = new[] { SKColors.Black, new SKColor(47, 87, 121), SKColors.Black, new SKColor(47, 87, 121), SKColors.Black };
            var gradientPositions = new[] { 0f, 0.25f, 0.5f, 0.75f, 1f };

            paint.Shader = SKShader.CreateLinearGradient(startPoint, endPoint, gradientColors, gradientPositions, SKShaderTileMode.Clamp);

            // Fill the canvas with the gradient
            canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paint);

            // Draw a filled rectangle
            SKPoint center = new(info.Width / 2f, info.Height / 2f);
            float rectSize = 100;
            SKRect rect = new(center.X - rectSize, center.Y - rectSize, center.X, center.Y);
            paint.Color = new SKColor(0, 0, 255, 204); // Semi-transparent blue
            paint.Shader = null; // Reset shader for solid colors
            canvas.DrawRect(rect, paint);

            float radiusX = info.Width / 4f;
            float radiusY = info.Height / 4f;

            // Draw a filled ellipse overlapping the rectangle
            paint.Color = new SKColor(255, 0, 0, 128); // Semi-transparent red
            canvas.DrawOval(center, new SKSize(radiusX, radiusY), paint);

            // Draw a border rectangle
            rect = new SKRect(center.X - radiusX, center.Y - radiusY, center.X + radiusX, center.Y + radiusY);
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 2;
            paint.Color = SKColors.Blue;
            canvas.DrawRect(rect, paint);

            // Apply transformations (skew and rotation)
            var skewMatrix = SKMatrix.CreateSkew(0.5f, 0.5f);
            // also translate to 0,0
            skewMatrix.TransX = rect.Left;
            skewMatrix.TransY = rect.Top;
            canvas.SetMatrix(skewMatrix);
            paint.Color = SKColors.Lime;
            var newRect = new SKRect(0, 0, rect.Width, rect.Height);
            canvas.DrawRect(newRect, paint);

            // Apply rotation
            var rotationMatrix = skewMatrix.PostConcat(SKMatrix.CreateRotation((float)(90 * Math.PI / 180), center.X, center.Y));
            canvas.SetMatrix(rotationMatrix);
            paint.Color = SKColors.Red;
            canvas.DrawRect(newRect, paint);

            // Reset transformations
            canvas.ResetMatrix();

            // Draw a filled arc
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 10;
            paint.Color = SKColors.Yellow.WithAlpha(128);
            // set cap style
            paint.StrokeCap = SKStrokeCap.Round;
            canvas.DrawArc(rect, 0, -180, false, paint);

            // Draw a pie
            paint.Color = SKColors.Cyan.WithAlpha(128);
            // set stroke style
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeJoin = SKStrokeJoin.Round;
            canvas.DrawArc(rect, 0, 270, true, paint);

            // Draw another rect
            rect = new SKRect(center.X, center.Y, center.X + rectSize, center.Y + rectSize);
            paint.Color = SKColors.Lime.WithAlpha(128);
            paint.Style = SKPaintStyle.Fill;
            canvas.DrawRect(rect, paint);

            // Draw a vertical line
            paint.StrokeWidth = 5;
            // reset path effect
            paint.PathEffect = null;
            canvas.DrawLine(new SKPoint(info.Width / 2, 0), new SKPoint(info.Width / 2, info.Height), paint);

            // Draw a star (custom path)
            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Black.WithAlpha(128);
            using var starPath = CreateStarPath(center, info.Width / 4f, info.Width / 8f, 5);
            //canvas.DrawPath(starPath, paint);

            // Apply a blur effect
            using var blurFilter = SKImageFilter.CreateBlur(5, 5);
            e.Surface.Canvas.DrawImage(offscreenSurface.Snapshot(), 0, 0, new SKPaint
            {
                ImageFilter = blurFilter
            });

            // Draw a red border around the surface
            paint.Color = SKColors.Red;
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 10;
            e.Surface.Canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paint);

            // Draw a horizontal middle line
            paint.Color = SKColors.Red.WithAlpha(128);
            paint.StrokeWidth = 5;
            e.Surface.Canvas.DrawLine(new SKPoint(0, info.Height / 2), new SKPoint(info.Width, info.Height / 2), paint);
            Debug.WriteLine("SK Drawing complete");
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
            new D2DTestForm().Show();
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
