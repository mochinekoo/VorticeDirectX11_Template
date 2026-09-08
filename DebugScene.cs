using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Effects;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using VorticeDirectX11_Template;
using static VorticeDirectX_Sample.Manager.DX3D;
using Box = VorticeDirectX11_Template.Box;

namespace VorticeDirectX_Sample.Scene {
    internal class DebugScene : BaseScene {

        private Box Box_;

        public DebugScene() : base("DebugScene") {
            Box_ = new Box(100.0f, 100.0f);
        }

        public override void Init() {
            Box_.Init();
        }

        public override void Update() {
            Box_.Update();
        }

        public override void Draw() {

            Render.DX3D.DXContext.IASetPrimitiveTopology(Vortice.Direct3D.PrimitiveTopology.TriangleList);
            Render.DX3D.DXContext.IASetInputLayout(Render.DX3D.InputLayout);
            Render.DX3D.DXContext.VSSetShader(Render.DX3D.VetexShader);
            Render.DX3D.DXContext.PSSetShader(Render.DX3D.PixelShader);

            Box_.Draw();

            Render.DX3D.DXContext.RSSetState(null);
        }

    }
}
