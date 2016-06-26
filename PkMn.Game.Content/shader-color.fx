sampler s0;

float4 black;
float4 darkGray;
float4 lightGray;
float4 white;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	if(color.a == 1 && color.r == 0) color = black;
	else if(color.a == 1 && abs(color.r - 0.3333333) < 0.001 ) color = darkGray;
	else if(color.a == 1 && abs(color.r - 0.6666666) < 0.001 ) color = lightGray;
	else if(color.a == 1 && color.r == 1 ) color = white;
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
