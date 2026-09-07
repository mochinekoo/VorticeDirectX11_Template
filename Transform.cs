using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vortice.Mathematics;

namespace VorticeDirectX11_Template {
    public class Transform {

        public Double3 location_;
        public Double3 velocity_;
        public Double3 rotation_;
        public Double3 scale_;

        public Transform() {
            location_ = new Double3(0, 0, 0);
            velocity_ = new Double3(0, 0, 0);
            rotation_ = new Double3(0, 0, 0);
            scale_ = new Double3(1, 1, 1);
        }

        Matrix4x4 GetWorldMatrix() {
            Matrix4x4 scale = Matrix4x4.CreateScale((float) scale_.X, (float)scale_.Y, (float)scale_.Z);
            Matrix4x4 rotation = Matrix4x4.CreateFromYawPitchRoll((float)rotation_.Y, (float)rotation_.X, (float)rotation_.Z);
            Matrix4x4 translation = Matrix4x4.CreateTranslation((float)location_.X, (float)location_.Y, (float)location_.Z);
            return scale * rotation * translation;

        }

    }
}
