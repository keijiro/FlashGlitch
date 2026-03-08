using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace FlashGlitch {

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Flash Glitch")]
public sealed class FlashGlitchController : MonoBehaviour
{
    [field:SerializeField, Min(0.0001f)] public float ReleaseTime1 { get; set; } = 0.5f;
    [field:SerializeField, Min(0.0001f)] public float ReleaseTime2 { get; set; } = 0.5f;
    [field:SerializeField, Range(0, 1)] public float Hue { get; set; }
    [field:SerializeField] public uint Seed { get; set; } = 1;

    [SerializeField, HideInInspector] Shader _shader = null;

    public bool IsActive => math.any(_params > 0);

    Material _material;
    float2 _params;
    (Random gen, int val) _random;

    float ApplyStrengthCurve(float strength)
      => 0.15f + (1 - math.pow(1 - strength, 2));

    public void TriggerEffect1(float strength)
    {
        _params.x = ApplyStrengthCurve(strength);
        _random.val = _random.gen.NextInt();
    }

    public void TriggerEffect2(float strength)
    {
        _params.y = ApplyStrengthCurve(strength);
        _random.val = _random.gen.NextInt();
    }

    public void RandomizeHue() => Hue = _random.gen.NextFloat();

    public Material UpdateMaterial()
    {
        if (_material == null) _material = CoreUtils.CreateEngineMaterial(_shader);
        _material.SetVector("_Params", (Vector2)_params);
        _material.SetInteger("_Seed", _random.val);
        _material.SetFloat("_Hue", Hue);
        return _material;
    }

    void ReleaseResources()
    {
        CoreUtils.Destroy(_material);
        _material = null;
    }

    void OnDestroy() => ReleaseResources();

    void OnDisable() => ReleaseResources();

    void OnEnable()
      => (_random.gen = new Random(Seed)).NextUInt4(); // Init with warming up

    void Update()
    {
        var delta = Time.deltaTime / new float2(ReleaseTime1, ReleaseTime2);
        _params = math.max(0, _params - delta);
    }
}

} // namespace FlashGlitch
