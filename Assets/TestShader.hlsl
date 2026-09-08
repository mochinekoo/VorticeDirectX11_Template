cbuffer ConstantBuffer : register(b0)
{
    matrix wvpMatrix;
    float4 diffuse;
    float4 ambient;
    float4 specular;
    float3 emission;
    float shininess;
    int hasTexture;
    float3 lightDirection;
    int enableGray;
};

struct VSInput {
    float3 POSTION : POSTION;
    float4 COLOR : COLOR;
};

struct VSOutput {
    float4 POSTION : SV_Position;
    float4 COLOR : COLOR;
};

VSOutput VSMain(VSInput input) {
    VSOutput output;
    output.POSTION = mul(float4(input.POSTION, 1.0f), wvpMatrix);
    output.COLOR = input.COLOR;
    return output;
}

float4 PSMain(VSOutput input) : SV_TARGET {
    return input.COLOR;
}