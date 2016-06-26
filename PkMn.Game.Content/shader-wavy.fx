sampler s0;

float4x4 matrixTransform;

float screenWidth;
float screenHeight;
float lineOffsets[32];

void SpriteVertexShader(inout float4 color : COLOR0, inout float2 texCoord : TEXCOORD0, inout float4 position : SV_Position)
{
    position = mul(position, matrixTransform);
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords + float2(lineOffsets[((coords.y * screenHeight) / 2) % 32] * 4 / screenWidth, 0.0));
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
		VertexShader = compile vs_3_0 SpriteVertexShader();
    }
}
