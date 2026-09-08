using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using VorticeDirectX_Sample;
using static VorticeDirectX_Sample.Manager.DX3D;

namespace VorticeDirectX11_Template {
    public class Box : BaseObject {

        private ID3D11Buffer vertexBuffer_;

        public Box() : base("Box") {
        }

        public override void Init() {
            InitVertexBuffer();
        }

        public override void Draw() {
            Render.DX3D.DXContext.IASetPrimitiveTopology(Vortice.Direct3D.PrimitiveTopology.TriangleList);
            Render.DX3D.DXContext.IASetInputLayout(Render.DX3D.InputLayout);
            Render.DX3D.DXContext.VSSetShader(Render.DX3D.VetexShader);
            Render.DX3D.DXContext.IASetVertexBuffer(0, vertexBuffer_, Vertex.SizeInBytes);
            Render.DX3D.DXContext.PSSetShader(Render.DX3D.PixelShader);

            Render.DX3D.DXContext.Draw(6, 0);

            Render.DX3D.DXContext.RSSetState(null);
        }

        public override void Update() {

        }

        private void InitVertexBuffer() {
            Vertex[] vertices = [
                new Vertex(0.0f, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.0f, 0.5f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.5f, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),

                new Vertex(0.5f, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.0f, 0.5f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.5f, 0.5f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f))
              ];
            int size = Marshal.SizeOf<Vertex>() * vertices.Length;

            BufferDescription bufferDescription =
                new BufferDescription {
                    Usage = ResourceUsage.Default,
                    ByteWidth = (uint)size,
                    BindFlags = BindFlags.VertexBuffer,
                    CPUAccessFlags = CpuAccessFlags.None,
                    MiscFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };


            GCHandle handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);

            try {
                IntPtr pointer = handle.AddrOfPinnedObject();

                unsafe {
                    SubresourceData subresourceData = new SubresourceData(pointer.ToPointer());

                    vertexBuffer_ = Render.DX3D.DXDevice.CreateBuffer(bufferDescription, subresourceData);
                }
            } finally {
                handle.Free();
            }
        }
    }
}
