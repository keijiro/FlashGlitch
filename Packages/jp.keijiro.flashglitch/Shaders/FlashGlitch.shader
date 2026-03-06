Shader "Hidden/URPCameraEffect/FlashGlitch"
{
HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
#include "Packages/jp.keijiro.flashglitch/Shaders/FlashGlitchFunctions.hlsl"

half _Effect1;
half _Effect2;
half _Seed;
half _Hue;

half FlashGlitchSample(half2 uv, half threshold, half seed)
{
    float aspect = _ScreenParams.x / _ScreenParams.y;
    float random;
    float2 duv = FlashGlitchDistortion(uv, aspect, seed, random);

    float3 src = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, duv).rgb;
    return FlashGlitchThreshold(src, threshold + random);
}

half4 Frag(Varyings input) : SV_Target
{
    float2 uv = input.texcoord;
    float2 uvDisplaced = FlashGlitchDisplace(uv, _Seed);

    float threshold1 = 1 - max(_Effect1, _Effect2);
    float threshold2 = 1 - _Effect2;

    float sample1 = FlashGlitchSample(uv, threshold1, _Seed);
    float sample2 = FlashGlitchSample(uvDisplaced, threshold2, _Seed);

    float alpha = max(sample1, sample2);
    float3 color = float3(sample1, sample2, sample2);
    color = FlashGlitchApplyHue(color, _Hue);

    float4 src = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
    float3 rgb = lerp(src.rgb, color, alpha);
    return half4(rgb, src.a);
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
