using UnityEngine;

namespace FlashGlitchURP {

public sealed class FlashGlitchRandomTrigger : MonoBehaviour
{
    [SerializeField] FlashGlitchController _target = null;
    [SerializeField] int _index = 0;
    [SerializeField] float _minValue = 0;
    [SerializeField] float _maxValue = 1;

    async void Start()
    {
        while (true)
        {
            var wait = Mathf.Pow(0.5f, Random.Range(1, 8));
            await Awaitable.WaitForSecondsAsync(wait);

            var value = Random.Range(_minValue, _maxValue);
            _target.TriggerEffect(_index, value);

            _target.RandomizeSeed();
            if (Random.value < 0.01f) _target.RandomizeHue();
        }
    }
}

} // namespace FlashGlitchURP
