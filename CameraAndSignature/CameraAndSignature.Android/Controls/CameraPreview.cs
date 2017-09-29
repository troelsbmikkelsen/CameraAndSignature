using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CameraAndSignature.Droid.Controls {
    public class CameraPreview : ViewGroup, ISurfaceHolderCallback {
        SurfaceView surfaceView;
        ISurfaceHolder holder;
        Android.Hardware.Camera.Size previewSize;
        IList<Android.Hardware.Camera.Size> supportedPreviewSizes;
        Android.Hardware.Camera camera;
        IWindowManager windowManager;

        public bool IsPreviewing { get; set; }

        public Android.Hardware.Camera Preview {
            get { return camera; }
            set {
                camera = value;
                if (camera != null) {
                    supportedPreviewSizes = Preview.GetParameters().SupportedPreviewSizes;
                    RequestLayout();
                }
            }
        }

        public CameraPreview(Context context) : base(context) {
            surfaceView = new SurfaceView(context);
            AddView(surfaceView);

            windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            IsPreviewing = false;
            holder = surfaceView.Holder;
            holder.AddCallback(this);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
            int width = ResolveSize(SuggestedMinimumWidth, widthMeasureSpec);
            int height = ResolveSize(SuggestedMinimumHeight, heightMeasureSpec);
            SetMeasuredDimension(width, height);

            if (supportedPreviewSizes != null) {
                previewSize = GetOptimalPreviewSize(supportedPreviewSizes, width, height);
            }
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom) {
            int msw = MeasureSpec.MakeMeasureSpec(right - left, MeasureSpecMode.Exactly);
            int msh = MeasureSpec.MakeMeasureSpec(bottom - top, MeasureSpecMode.Exactly);

            surfaceView.Measure(msw, msh);
            surfaceView.Layout(0, 0, right - left, bottom - top);
        }

        public void SurfaceCreated(ISurfaceHolder holder) {
            try {
                if (Preview != null) {
                    Preview.SetPreviewDisplay(holder);
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error in SurfaceCreated in CameraPreview.Android: \r\n{ex.Message}");
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {
            if (Preview != null) {
                Preview.StopPreview();
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height) {
            Android.Hardware.Camera.Parameters parameters = Preview.GetParameters();
            parameters.SetPreviewSize(previewSize.Width, previewSize.Height);
            RequestLayout();

            switch (windowManager.DefaultDisplay.Rotation) {
                case SurfaceOrientation.Rotation0:
                    camera.SetDisplayOrientation(90);
                    break;
                case SurfaceOrientation.Rotation90:
                    camera.SetDisplayOrientation(0);
                    break;
                case SurfaceOrientation.Rotation270:
                    camera.SetDisplayOrientation(180);
                    break;
                default:
                    break;
            }

            Preview.SetParameters(parameters);
            Preview.StartPreview();
            IsPreviewing = true;
        }

        private Android.Hardware.Camera.Size GetOptimalPreviewSize(IList<Android.Hardware.Camera.Size> supportedPreviewSizes, int width, int height) {
            const double AspectTolerance = 0.1;
            double targetRatio = (double)width / height;

            if (supportedPreviewSizes == null) {
                return null;
            }

            Android.Hardware.Camera.Size optimalSize = null;
            double minDiff = double.MaxValue;

            int targetHeight = height;
            foreach (Android.Hardware.Camera.Size size in supportedPreviewSizes) {
                double ratio = (double)size.Width / size.Height;

                if (Math.Abs(ratio - targetRatio) > AspectTolerance) {
                    continue;
                }
                if (Math.Abs(size.Height - targetHeight) < minDiff) {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null) {
                minDiff = double.MaxValue;
                foreach (Android.Hardware.Camera.Size size in supportedPreviewSizes) {
                    if (Math.Abs(size.Height - targetHeight) < minDiff) {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }

            return optimalSize;

        }
    }
}