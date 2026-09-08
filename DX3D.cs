using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Effects;
using Vortice.D3DCompiler;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Vortice.Wpf;

namespace VorticeDirectX_Sample.Manager {
    public class DX3D {

        public DX3D() {
        }

        /// DirectX
        public ID3D11Device DXDevice {
            get; private set;
        }
        public ID3D11DeviceContext DXContext {
            get; private set;
        }
        public ID3D11DepthStencilState DepthStencilState {
            get; private set;
        }

        public ID3D11RasterizerState RasterizerState {
            get; private set;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex {
            public Vector3 Location;
            public Color4 Color;

            public Vertex(Vector3 location, Color4 color) {
                Location = location;
                Color = color;
            }

            public Vertex(float x, float y, float z, Color4 color) {
                Location.X = x;
                Location.Y = y;
                Location.Z = z;
                Color = color;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConstantBuffer {
            public Matrix4x4 wvpMatrix_ = Matrix4x4.Identity;
            public Double3 diffuse_ = new Double3();
            public Double3 ambient_ = new Double3();
            public Double3 specular_ = new Double3();
            public Double3 emission_ = new Double3();
            public float shininess_ = 0.0f;
            public int hasTexture_ = 0;
            public Double3 lightDirection_ = new Double3();
            public int enableGray_ = 0;

            public ConstantBuffer() {
            }
        }

        /// シェーダー
        public ID3D11VertexShader VetexShader {
            get; private set;
        }
        public ID3D11PixelShader PixelShader {
            get; private set;
        }
        public ID3D11InputLayout InputLayout {
            get; private set;
        }


        public void Init(DrawingSurfaceEventArgs e) {
            DXDevice = e.Device;
            DXContext = e.Context;
            InitShader();
            InitRasterizerState();
        }

        public void Release(DrawingSurfaceEventArgs e) {

        }

        public void InitShader() {

            string shaderPath = Path.Combine(AppContext.BaseDirectory, "Assets", "TestShader.hlsl");

            Debug.Print($"Shader Path: {shaderPath}");
            Debug.Print($"Exists: {File.Exists(shaderPath)}");

            ReadOnlyMemory<byte> vertexShaderData = Compiler.CompileFromFile(
                Path.Combine(AppContext.BaseDirectory, "Assets", "TestShader.hlsl"),
                "VSMain",
                "vs_4_0");

            ReadOnlyMemory<byte> pixelShaderData = Compiler.CompileFromFile(
                Path.Combine(AppContext.BaseDirectory, "Assets", "TestShader.hlsl"),
                "PSMain",
                "ps_4_0");

            VetexShader = DXDevice.CreateVertexShader(vertexShaderData.Span);
            PixelShader = DXDevice.CreatePixelShader(pixelShaderData.Span);

            InputElementDescription[] inputElements = [
                    new InputElementDescription("POSTION", 0, Vortice.DXGI.Format.R32G32B32_Float, 0, 0),
                    new InputElementDescription("COLOR", 0, Vortice.DXGI.Format.R32G32B32A32_Float, 12, 0)
                ];
            InputLayout = DXDevice.CreateInputLayout(inputElements, vertexShaderData.Span);
        }

        public void InitRasterizerState() {
            RasterizerDescription rasterizerDescription = new RasterizerDescription {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                FrontCounterClockwise = false,
                DepthClipEnable = true,
                ScissorEnable = false,
                MultisampleEnable = false,
                AntialiasedLineEnable = false
            };
            RasterizerState = DXDevice.CreateRasterizerState(rasterizerDescription);
        }
    }
}
