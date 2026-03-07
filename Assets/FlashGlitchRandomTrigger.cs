using UnityEngine;
using FlashGlitch;

public sealed class FlashGlitchRandomTrigger : MonoBehaviour
{
    [SerializeField] FlashGlitchController _target = null;
    [SerializeField] int _index = 0;
    [SerializeField] float _minStrength = 0;
    [SerializeField] float _maxStrength = 1;

    async void Start()
    {
        while (true)
        {
            var wait = Mathf.Pow(0.5f, Random.Range(1, 8));
            await Awaitable.WaitForSecondsAsync(wait);

            var strength = Random.Range(_minStrength, _maxStrength);
            if (_index == 0)
                _target.TriggerEffect1(strength);
            else
                _target.TriggerEffect2(strength);

            if (Random.value < 0.03f) _target.RandomizeHue();
        }
    }
}
