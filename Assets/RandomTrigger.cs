using UnityEngine;
using System.Collections;

namespace FlashGlitch {

public sealed class RandomTrigger : MonoBehaviour
{
    [SerializeField] FlashGlitchController _target = null;
    [SerializeField] int _index = 0;
    [SerializeField] float _minValue = 0;
    [SerializeField] float _maxValue = 1;

    IEnumerator Start()
    {
        while (true)
        {
            var time = Mathf.Pow(0.5f, Random.Range(1, 4));
            yield return new WaitForSeconds(time);

            var value = Random.Range(_minValue, _maxValue);
            _target.TriggerEffect(_index, value);

            _target.RandomizeSeed();
            if (Random.value < 0.01f) _target.RandomizeHue();
        }
    }
}

} // namespace FlashGlitch
