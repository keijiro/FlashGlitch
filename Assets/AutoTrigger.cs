using UnityEngine;
using FlashGlitch;

public sealed class AutoTrigger : MonoBehaviour
{
    [SerializeField] FlashGlitchController _target = null;

    void Start()
    {
        RunAutoTrigger1Async();
        RunAutoTrigger2Async();
    }

    async void RunAutoTrigger1Async()
    {
        while (true)
        {
            var wait = Mathf.Pow(0.5f, Random.Range(1, 8));
            await Awaitable.WaitForSecondsAsync(wait);
            _target.TriggerEffect1(Random.Range(0.5f, 1.0f));
        }
    }

    async void RunAutoTrigger2Async()
    {
        while (true)
        {
            var wait = Mathf.Pow(0.5f, Random.Range(1, 8));
            await Awaitable.WaitForSecondsAsync(wait);
            _target.TriggerEffect2(Random.Range(0.5f, 1.0f));
            if (Random.value < 0.1f) _target.RandomizeHue();
        }
    }
}
