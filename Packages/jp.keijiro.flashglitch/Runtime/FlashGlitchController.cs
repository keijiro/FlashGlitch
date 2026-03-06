using UnityEngine;
using UnityEngine.Rendering;

namespace FlashGlitchURP {

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("URP Camera Effect/Flash Glitch")]
public sealed class FlashGlitchController : MonoBehaviour
{
    [field:SerializeField, Min(0.0001f)] public float ReleaseTime1 { get; set; } = 0.5f;
    [field:SerializeField, Min(0.0001f)] public float ReleaseTime2 { get; set; } = 0.5f;
    [field:SerializeField] public uint Seed { get; set; }
    [field:SerializeField, Range(0, 1)] public float Hue { get; set; }

    [SerializeField, HideInInspector] Shader _shader = null;

    public bool IsActive => _value1 > 0 || _value2 > 0;

    Material _material;
    float _value1;
    float _value2;

    public void TriggerEffect(int index, float value)
    {
        if (index == 0) _value1 = value;
        else _value2 = value;
    }

    public void TriggerEffect1(float value)
      => _value1 = value;

    public void TriggerEffect2(float value)
      => _value2 = value;

    public void RandomizeSeed()
      => Seed = (uint)Random.Range(0, 100000);

    public void RandomizeHue()
      => Hue = Random.value;

    public Material UpdateMaterial()
    {
        if (_material == null) _material = CoreUtils.CreateEngineMaterial(_shader);

        _material.SetFloat("_Effect1", _value1);
        _material.SetFloat("_Effect2", _value2);
        _material.SetInteger("_Seed", (int)Seed);
        _material.SetFloat("_Hue", Hue);

        return _material;
    }

    void Update()
    {
        _value1 = Mathf.Max(0, _value1 - Time.deltaTime / ReleaseTime1);
        _value2 = Mathf.Max(0, _value2 - Time.deltaTime / ReleaseTime2);
    }

    void ReleaseResources()
    {
        CoreUtils.Destroy(_material);
        _material = null;
    }

    void OnDestroy() => ReleaseResources();
    void OnDisable() => ReleaseResources();
}

} // namespace FlashGlitchURP
