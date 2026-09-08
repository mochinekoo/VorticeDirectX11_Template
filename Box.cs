using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using VorticeDirectX_Sample;
using VorticeDirectX_Sample.Manager;
using static VorticeDirectX_Sample.Manager.DX3D;

namespace VorticeDirectX11_Template {
    public class Box : BaseObject {

        private ConstantBuffer constantBuffer_;
        private ID3D11Buffer d3D11ConstantBuffer_;
        private float width_;
        private float height_;
        public Double3 Color {
            get; set;
        } = new Double3(1.0f, 0.0f, 0.0f);
        private ID3D11Buffer vertexBuffer_;

        public Box(float width, float height) : base("Box") {
            width_ = width;
            height_ = height;
        }

        public override void Init() {
            InitVertexBuffer();
            InitConstantBuffer();
        }

        public override void Draw() {
            unsafe {
                Render.DX3D.DXContext.IASetPrimitiveTopology(Vortice.Direct3D.PrimitiveTopology.TriangleList);
                Render.DX3D.DXContext.IASetInputLayout(Render.DX3D.InputLayout);
                Render.DX3D.DXContext.VSSetShader(Render.DX3D.VetexShader);
                Render.DX3D.DXContext.IASetVertexBuffer(0, vertexBuffer_, (uint)sizeof(Vertex));
                Render.DX3D.DXContext.PSSetShader(Render.DX3D.PixelShader);
                Render.DX3D.DXContext.VSSetConstantBuffer(0, d3D11ConstantBuffer_);
                Render.DX3D.DXContext.PSSetConstantBuffer(0, d3D11ConstantBuffer_);

                Render.DX3D.DXContext.Draw(6, 0);

                Render.DX3D.DXContext.RSSetState(null);
            }
        }

        public override void Update() {
            Matrix4x4 world = Transform.GetWorldMatrix();
            Matrix4x4 view = Matrix4x4.Identity;
            Matrix4x4 projection = Matrix4x4.CreateOrthographicOffCenterLeftHanded(
                0.0f, 1280.0f,
                720.0f, 0.0f,
                0.0f, 100.0f
            );

            constantBuffer_.wvpMatrix_ = Matrix4x4.Transpose(world * view * projection);
            constantBuffer_.diffuse_ = Color;
            constantBuffer_.hasTexture_ = 0;
            Render.DX3D.DXContext.UpdateSubresource(constantBuffer_, d3D11ConstantBuffer_, 0, 0);
        }

        private void InitVertexBuffer() {

            unsafe {
                Vertex[] vertices = [
                new Vertex(0.0f, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.0f, height_, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(width_, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),

                new Vertex(width_, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.0f, height_, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(width_, height_, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f))
  ];

                int size = (int)sizeof(Vertex) * vertices.Length;

                BufferDescription bufferDescription = new BufferDescription {
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

        private void InitConstantBuffer() {
            unsafe {
                int size = sizeof(ConstantBuffer);

                size = (size + 15) & ~15;

                Console.WriteLine($"ConstantBuffer size = {sizeof(ConstantBuffer)}");
                Console.WriteLine($"ConstantBuffer buffer size = {size}");

                BufferDescription constantDesc = new BufferDescription {
                    Usage = ResourceUsage.Default,
                    ByteWidth = (uint)size,
                    BindFlags = BindFlags.ConstantBuffer,
                    CPUAccessFlags = CpuAccessFlags.None,
                    MiscFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                d3D11ConstantBuffer_ =
                    Render.DX3D.DXDevice.CreateBuffer(constantDesc, 0);
            }
        }
    }
}
