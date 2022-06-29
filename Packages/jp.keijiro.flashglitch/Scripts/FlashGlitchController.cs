using UnityEngine;

namespace FlashGlitch {

public sealed class FlashGlitchController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] float _releaseTime1 = 0.5f;
    [SerializeField] float _releaseTime2 = 0.5f;
    [SerializeField] uint _seed = 0;
    [SerializeField] float _hue = 0;

    #endregion

    #region Public method

    public void TriggerEffect(int index, float value)
    {
        if (index == 0) _value1 = value; else _value2 = value;
    }

    public void TriggerEffect1(float value)
      => _value1 = value;

    public void TriggerEffect2(float value)
      => _value2 = value;

    public void RandomizeSeed()
      => _seed = (uint)Random.Range(0, 100000);

    public void RandomizeHue()
      => _hue = Random.value;

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
        _value1 = Mathf.Max(0, _value1 - Time.deltaTime / _releaseTime1);
        _value2 = Mathf.Max(0, _value2 - Time.deltaTime / _releaseTime2);
    }

    void LateUpdate()
    {
        var renderer = GetComponent<Renderer>();
        renderer.GetPropertyBlock(_block);

        _block.SetFloat("_Effect1", _value1);
        _block.SetFloat("_Effect2", _value2);
        _block.SetFloat("_Seed", _seed);
        _block.SetFloat("_Hue", _hue);

        renderer.SetPropertyBlock(_block);
    }

    #endregion
}

} // namespace FlashGlitch
