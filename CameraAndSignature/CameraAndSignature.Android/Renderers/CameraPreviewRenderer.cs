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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;


[assembly: ExportRenderer(typeof(CameraAndSignature.Views.CameraPreview), typeof(CameraAndSignature.Droid.Renderers.CameraPreviewRenderer))]
namespace CameraAndSignature.Droid.Renderers {
    public class CameraPreviewRenderer : ViewRenderer<CameraAndSignature.Views.CameraPreview, CameraAndSignature.Droid.Controls.CameraPreview> {
        Controls.CameraPreview cameraPreview;

        protected override void OnElementChanged(ElementChangedEventArgs<Views.CameraPreview> e) {
            base.OnElementChanged(e);

            if (Control == null) {
                cameraPreview = new Controls.CameraPreview(Context);
                SetNativeControl(cameraPreview);
            }

            if (e.OldElement != null) {
                cameraPreview.Click -= CameraPreview_Click;
            }

            if (e.NewElement != null) {
                Control.Preview = Android.Hardware.Camera.Open((int)e.NewElement.Camera);

                cameraPreview.Click += CameraPreview_Click;
            }
        }

        private void CameraPreview_Click(object sender, EventArgs e) {
            if (cameraPreview.IsPreviewing) {
                cameraPreview.Preview.StopPreview();
                cameraPreview.IsPreviewing = false;
            } else {
                cameraPreview.Preview.StartPreview();
                cameraPreview.IsPreviewing = true;
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Control.Preview.Release();
            }
            base.Dispose(disposing);
        }
    }
}