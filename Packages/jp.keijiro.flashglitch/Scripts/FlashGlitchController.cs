using UnityEngine;

namespace FlashGlitch {

public sealed class FlashGlitchController : MonoBehaviour
{
    #region Editable attributes

    [field:SerializeField] public float ReleaseTime1 { get; set; } = 0.5f;
    [field:SerializeField] public float ReleaseTime2 { get; set; } = 0.5f;
    [field:SerializeField] public uint Seed { get; set; } = 0;
    [field:SerializeField] public float Hue { get; set; } = 0;

    #endregion

    #region Public methods and properties

    public void TriggerEffect(int index, float value)
    {
        if (index == 0) _value1 = value; else _value2 = value;
    }

    public void TriggerEffect1(float value)
      => _value1 = value;

    public void TriggerEffect2(float value)
      => _value2 = value;

    public void RandomizeSeed()
      => Seed = (uint)Random.Range(0, 100000);

    public void RandomizeHue()
      => Hue = Random.value;

    public Texture Source { get; set; }

    #endregion

    #region Private members

    MaterialPropertyBlock _block;
    float _value1, _value2;

    #endregion

    #region MonoBehaviour implementation

    void Start()
      => _block = new MaterialPropertyBlock();

    void Update()
    {
        _value1 = Mathf.Max(0, _value1 - Time.deltaTime / ReleaseTime1);
        _value2 = Mathf.Max(0, _value2 - Time.deltaTime / ReleaseTime2);
    }

    void LateUpdate()
    {
        var renderer = GetComponent<Renderer>();
        renderer.GetPropertyBlock(_block);

        if (Source != null) _block.SetTexture("_MainTex", Source);
        _block.SetFloat("_Effect1", _value1);
        _block.SetFloat("_Effect2", _value2);
        _block.SetFloat("_Seed", Seed);
        _block.SetFloat("_Hue", Hue);

        renderer.SetPropertyBlock(_block);
    }

    #endregion
}

} // namespace FlashGlitch
