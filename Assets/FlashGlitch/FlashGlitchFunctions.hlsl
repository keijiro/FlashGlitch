#ifndef FLASH_GLITCH_FUNCTIONS_INCLUDED
#define FLASH_GLITCH_FUNCTIONS_INCLUDED

#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

float2 FlashGlitchDistortion(float2 uv, float aspect, float seed, out float random)
{
    const float cells = 16;
    float2 grid = floor(uv * float2(aspect, 1) * cells) / cells;

    float2 nPos = float2(grid * float2(0.3, 8)) + seed * 3.1545;
    uint hashSeed = (uint)((SimplexNoise(nPos) + 2) * 0.8 + seed) * 4;

    float2 offs1 = float2(Hash(hashSeed), Hash(hashSeed + 1));

    float skew = (Hash(hashSeed + 2) - 0.5) * 2;
    float skew2 = skew * skew * skew * skew * skew * skew * skew;
    float2 offs2 = float2(uv.y * skew2 * 8, 0);

    random = Hash(hashSeed + 3);
    return frac(uv + offs1 + offs2);
}

float FlashGlitchThreshold(float3 color, float threshold)
{
#ifndef UNITY_COLORSPACE_GAMMA
    color = LinearToSRGB(color);
#endif
    return Luminance(color) > threshold;
}

float2 FlashGlitchDisplace(float2 uv, float seed)
{
    float d = Hash(uv.y * 6 + seed) * 2 - 1;
    float dPow = d * d * d * d * d;
    return float2(uv.x + dPow * 0.03, uv.y);
}

float3 FlashGlitchRgbToHsv(float3 c)
{
    float4 k = float4(0, -1.0 / 3.0, 2.0 / 3.0, -1);
    float4 p = lerp(float4(c.bg, k.wz), float4(c.gb, k.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
    float d = q.x - min(q.w, q.y);
    float e = 1e-10;
    return float3(abs(q.z + (q.w - q.y) / (6 * d + e)), d / (q.x + e), q.x);
}

float3 FlashGlitchHsvToRgb(float3 c)
{
    float3 p = abs(frac(c.xxx + float3(0, 2.0 / 3.0, 1.0 / 3.0)) * 6 - 3);
    return c.z * lerp(float3(1, 1, 1), saturate(p - 1), c.y);
}

float3 FlashGlitchApplyHue(float3 color, float offset)
{
    float3 hsv = FlashGlitchRgbToHsv(saturate(color));
    hsv.x = frac(hsv.x + offset);
    return FlashGlitchHsvToRgb(hsv);
}

#endif
