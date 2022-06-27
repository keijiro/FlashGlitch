#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

void FlashGlitch_Displace_float
  (float2 uv, float time, out float2 output)
{
    float2 n = SimplexNoiseGrad(uv * 3).xy;
    float2 n2 = pow(saturate(n * 0.2), 10) * 0.2;
    float2 offs = lerp(n * 0.001, n2, n2);
    output = uv + offs;
}

void FlashGlitch_Distortion_float
  (float2 uv, float aspect, float time, out float2 output)
{
    const float cells = 16;
    const float2 grid = floor(uv * float2(aspect, 1) * cells) / cells;
    const float2 n_pos = float2(grid * float2(0.3, 8)) + time * 3.1545;
    const uint seed = (uint)((SimplexNoise(n_pos) + 2) * 1.8 + time) * 4;

    const float2 offs1 = float2(Hash(seed), Hash(seed + 1));

    const float rnd_skew = Hash(seed + 2);
    const float skew = rnd_skew > 0.5 ? (rnd_skew - 0.5) * 8 : 0;
    const float2 offs2 = float2(uv.y * skew, 0);

    output = frac(uv + offs1 + offs2);
}

void FlashGlitch_Threshold_float
  (float3 input, float thresh, out float output)
{
    output = Luminance(input) > thresh;
}
