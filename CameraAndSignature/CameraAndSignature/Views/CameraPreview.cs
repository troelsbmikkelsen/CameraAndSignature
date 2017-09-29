using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CameraAndSignature.Views {
    public enum CameraOptions {
        Front,
        Back
    }

    public class CameraPreview : Xamarin.Forms.View {
        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: "Camera",
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraPreview),
            defaultValue: CameraOptions.Back
        );

        public CameraOptions Camera {
            get {
                return (CameraOptions)GetValue(CameraProperty);
            }
            set {
                SetValue(CameraProperty, value);
            }
        }
    }
}
