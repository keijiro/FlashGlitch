Shader "Hidden/URPCameraEffect/FlashGlitch"
{
HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

half _Effect1;
half _Effect2;
uint _Seed;
half _Hue;

float2 FG_Distortion(float2 uv, float aspect, uint seed, out float random)
{
    const float cells = 16;
    float2 grid = floor(uv * float2(aspect, 1) * cells) / cells;

    float2 nPos = float2(grid * float2(0.3, 8)) + float(seed) * 3.1545;
    uint hashSeed = (uint)((SimplexNoise(nPos) + 2) * 0.8 + float(seed)) * 4;

    float2 offs1 = float2(Hash(hashSeed), Hash(hashSeed + 1));

    float skew = (Hash(hashSeed + 2) - 0.5) * 2;
    float skew7 = skew * skew * skew * skew * skew * skew * skew;
    float2 offs2 = float2(uv.y * skew7 * 8, 0);

    random = Hash(hashSeed + 3);
    return frac(uv + offs1 + offs2);
}

float2 FG_Displace(float2 uv, uint seed)
{
    float d = Hash((uint)(uv.y * 6) + seed) * 2 - 1;
    float d5 = d * d * d * d * d;
    return float2(uv.x + d5 * 0.03, uv.y);
}

float3 FG_ApplyHue(float3 color, float shift)
{
    float3 hsv = RgbToHsv(saturate(color));
    hsv.x = frac(hsv.x + shift);
    return SRGBToLinear(HsvToRgb(hsv));
}

half FG_Sample(half2 uv, half threshold, uint seed)
{
    float aspect = _ScreenParams.x / _ScreenParams.y;
    float random;
    float2 duv = FG_Distortion(uv, aspect, seed, random);

    float3 src = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, duv).rgb;
    return Luminance(LinearToSRGB(src)) > threshold + random;
}

half4 Frag(Varyings input) : SV_Target
{
    float2 uv1 = input.texcoord;
    float2 uv2 = FG_Displace(uv1, _Seed);

    float threshold1 = 1 - max(_Effect1, _Effect2);
    float threshold2 = 1 - _Effect2;

    float sample1 = FG_Sample(uv1, threshold1, _Seed);
    float sample2 = FG_Sample(uv2, threshold2, _Seed);

    float alpha = max(sample1, sample2);
    float3 color = FG_ApplyHue(float3(sample1, sample2, sample2), _Hue);

    float4 src = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv1);
    return half4(lerp(src.rgb, color, alpha), src.a);
}

ENDHLSL

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        Pass
        {
            Name "FlashGlitch"
            ZTest Always ZWrite Off Cull Off
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
}
