
sampler textureSampler : register(s2);
Texture2D sTexture;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 PixelShaderFunction(VertexShaderOutput input): SV_Target
{
    float4 color = sTexture.Sample(textureSampler, input.TextureCoordinates);

    color.rgba = color.agbr;



    return color;
}


technique Technique1
{
	pass Pass0
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}

}