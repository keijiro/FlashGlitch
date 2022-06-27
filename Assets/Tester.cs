using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections;

sealed class Tester : MonoBehaviour
{
    [SerializeField] string _propertyName = "_Param";

    Lazy<MaterialPropertyBlock> _block = new Lazy<MaterialPropertyBlock>();
    float _param;

    float NextWaitTime()
      => Mathf.Pow(0.5f, Random.Range(1, 4));

    IEnumerator Start()
    {
        while (true)
        {
            _param = 1;
            yield return new WaitForSeconds(NextWaitTime());
        }
    }

    void Update()
    {
        _param = Mathf.Max(0, _param - Time.deltaTime * 10);

        var r = GetComponent<Renderer>();
        r.GetPropertyBlock(_block.Value);
        _block.Value.SetFloat(_propertyName, _param);
        r.SetPropertyBlock(_block.Value);
    }
}
