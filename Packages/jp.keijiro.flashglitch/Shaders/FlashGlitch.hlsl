#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

void FlashGlitch_Distortion_float
  (float2 uv, float aspect, float time, out float2 output, out float rand)
{
    const float cells = 16;
    const float2 grid = floor(uv * float2(aspect, 1) * cells) / cells;

    const float2 n_pos = float2(grid * float2(0.3, 8)) + time * 3.1545;
    const uint seed = (uint)((SimplexNoise(n_pos) + 2) * 0.8 + time) * 4;

    const float2 offs1 = float2(Hash(seed), Hash(seed + 1));

    const float skew = (Hash(seed + 2) - 0.5) * 2;
    const float skew2 = skew * skew * skew * skew * skew * skew * skew;
    const float2 offs2 = float2(uv.y * skew2 * 8, 0);

    output = frac(uv + offs1 + offs2);
    rand = Hash(seed + 3);
}

void FlashGlitch_Threshold_float
  (float3 color, float threshold, out float output)
{
#ifndef UNITY_COLORSPACE_GAMMA
    color = LinearToSRGB(color);
#endif
    output = Luminance(color) > threshold;
}
