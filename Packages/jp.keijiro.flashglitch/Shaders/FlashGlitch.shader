Shader "Hidden/URPCameraEffect/FlashGlitch"
{
HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

half2 _Params;
half _Hue;
uint _Seed;

// Hash x4
float4 Hash4(uint seed)
{
    seed *= 4;
    return float4(Hash(seed), Hash(seed + 1), Hash(seed + 2), Hash(seed + 3));
}

// Per-grid random number generator
float4 FG_GridRandom(float2 uv, uint seed)
{
    // Quantize UV into a coarse grid.
    const float cells = 32;
    float aspect = _ScreenParams.x / _ScreenParams.y;
    float2 grid = floor(uv * float2(aspect, 1) * cells) / cells;

    // Use quantized 2D noise to derive a per-grid seed offset with spatial correlation.
    float2 npos = grid * float2(0.075, 2) + float(seed & 0xffu);
    uint offs = (SimplexNoise(npos) + 2) * 4;

    return Hash4(seed + offs);
}

// UV skew function
float2 FG_SkewUV(float2 uv, float rand)
{
    float skew = (rand - 0.5) * 2;
    float skew7 = skew * skew * skew * skew * skew * skew * skew;
    return float2(uv.y * skew7 * 8, 0);
}

// UV base displacement function
float2 FG_Displace(float2 uv, uint seed)
{
    float d = Hash((uint)(uv.y * 6) + seed) * 2 - 1;
    float d5 = d * d * d * d * d;
    return float2(uv.x + d5 * 0.5, uv.y);
}

// Hue shifter
half3 FG_ApplyHue(half3 color, half shift)
{
    half3 hsv = RgbToHsv(saturate(color));
    hsv.x = frac(hsv.x + shift);
    return SRGBToLinear(HsvToRgb(hsv));
}

// Glitched sampler
half FG_SampleGlitch(half2 uv, half threshold, float4 rand)
{
    float2 skew = FG_SkewUV(uv, rand.z);
    half2 uv2 = frac(uv + rand.xy + skew);
    half3 src = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv2).rgb;
    return Luminance(LinearToSRGB(src)) > threshold + rand.w;
}

half4 Frag(Varyings input) : SV_Target
{
    half2 uv1 = input.texcoord;
    float2 uv2 = FG_Displace(uv1, _Seed ^ 0x38f7cu);

    half threshold1 = 1 - max(_Params.x, _Params.y);
    half threshold2 = 1 - _Params.y;

    float4 rand1 = FG_GridRandom(uv1, _Seed ^ 0x1beefu);
    float4 rand2 = FG_GridRandom(uv2, _Seed ^ 0x1beefu);
    half sample1 = FG_SampleGlitch(uv1, threshold1, rand1);
    half sample2 = FG_SampleGlitch(uv2, threshold2, rand2);

    half alpha = max(sample1, sample2);
    half3 color = FG_ApplyHue(half3(sample1, sample2, sample2), _Hue);

    half4 src = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv1);
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
