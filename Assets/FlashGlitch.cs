using UnityEngine;
using Random = UnityEngine.Random;
using System;

sealed class FlashGlitch : MonoBehaviour
{
    Lazy<MaterialPropertyBlock> _block = new Lazy<MaterialPropertyBlock>();
    (float value, float speed) [] _params = new (float, float) [2];
    float _seed;

    public void TriggerEffect(int index)
    {
        _params[index] = (1, Mathf.Pow(Random.Range(2.0f, 5.0f), 2));
        _seed = Random.Range(0, 100000);
    }

    void Update()
    {
        for (var i = 0; i < _params.Length; i++)
            _params[i].value -= _params[i].speed * Time.deltaTime;

        var renderer = GetComponent<Renderer>();
        var block = _block.Value;
        renderer.GetPropertyBlock(block);

        block.SetFloat("_Param1", Mathf.Max(0, _params[0].value));
        block.SetFloat("_Param2", Mathf.Max(0, _params[1].value));
        block.SetFloat("_Seed", _seed);
        block.SetFloat("_HueShift", (Time.time * 0.1f) % 1);

        renderer.SetPropertyBlock(block);
    }
}
