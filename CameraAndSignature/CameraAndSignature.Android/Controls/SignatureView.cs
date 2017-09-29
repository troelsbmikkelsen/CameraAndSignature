using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace CameraAndSignature.Droid.Controls {
    public class SignatureView : View {

        public Color LineColor { get; set; } = Color.Black;
        public float PenWidth { get; set; } = 3.0f;

        private Path DrawPath;
        private Paint DrawPaint;
        private Paint CanvasPaint;
        private Canvas DrawCanvas;
        private Bitmap CanvasBitmap;

        public SignatureView(Context context) : base(context) {
            Start();
        }

        private void Start() {

            DrawPath = new Path();
            DrawPaint = new Paint {
                Color = LineColor,
                AntiAlias = true,
                StrokeWidth = PenWidth
            };

            DrawPaint.SetStyle(Paint.Style.Stroke);
            DrawPaint.StrokeJoin = Paint.Join.Round;
            DrawPaint.StrokeCap = Paint.Cap.Round;

            CanvasPaint = new Paint {
                Dither = true
            };
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
            base.OnSizeChanged(w, h, oldw, oldh);

            CanvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
            DrawCanvas = new Canvas(CanvasBitmap);
        }

        protected override void OnDraw(Canvas canvas) {
            base.OnDraw(canvas);

            DrawPaint.Color = LineColor;
            canvas.DrawBitmap(CanvasBitmap, 0, 0, CanvasPaint);
            canvas.DrawPath(DrawPath, DrawPaint);
        }

        public override bool OnTouchEvent(MotionEvent e) {
            Single touchX = e.GetX();
            Single touchY = e.GetY();

            switch (e.Action) {
                case MotionEventActions.Down:
                    DrawPath.MoveTo(touchX, touchY);
                    break;
                case MotionEventActions.Move:
                    DrawPath.LineTo(touchX, touchY);
                    break;
                case MotionEventActions.Up:
                    DrawCanvas.DrawPath(DrawPath, DrawPaint);
                    DrawPath.Reset();
                    break;
                default:
                    return false;
            }

            Invalidate();

            return true;
        }
    }
}