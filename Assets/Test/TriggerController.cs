using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.Properties;
using FlashGlitch;

public sealed class TriggerController : MonoBehaviour
{
    [SerializeField] FlashGlitchController _target = null;
    [SerializeField] InputAction _trigger1Action = null;
    [SerializeField] InputAction _trigger2Action = null;

    [field:SerializeField]
    public bool AutoTrigger { get; set; } = true;

    [field:SerializeField, Range(0, 1)]
    public float Trigger1Strength { get; set; } = 1;

    [field:SerializeField, Range(0, 1)]
    public float Trigger2Strength { get; set; } = 1;

    void FireTrigger1()
      => _target.TriggerEffect1(Trigger1Strength);

    void FireTrigger2()
      => _target.TriggerEffect2(Trigger2Strength);

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.dataSource = this;

        root.Q<Button>("trigger1-button").clicked += FireTrigger1;
        root.Q<Button>("trigger2-button").clicked += FireTrigger2;

        _trigger1Action.performed += _ => FireTrigger1();
        _trigger2Action.performed += _ => FireTrigger2();
        _trigger1Action.Enable();
        _trigger2Action.Enable();

        RunAutoTrigger1Async();
        RunAutoTrigger2Async();
    }

    async void RunAutoTrigger1Async()
    {
        while (true)
        {
            while (!AutoTrigger) await Awaitable.NextFrameAsync();

            var wait = Mathf.Pow(0.5f, Random.Range(1, 8));
            await Awaitable.WaitForSecondsAsync(wait);

            if (!AutoTrigger) continue;

            _target.TriggerEffect1(Random.value * Trigger1Strength);
            if (Random.value < 0.03f) _target.RandomizeHue();
        }
    }

    async void RunAutoTrigger2Async()
    {
        while (true)
        {
            while (!AutoTrigger) await Awaitable.NextFrameAsync();

            var wait = Mathf.Pow(0.5f, Random.Range(1, 8));
            await Awaitable.WaitForSecondsAsync(wait);

            if (!AutoTrigger) continue;

            _target.TriggerEffect2(Random.value * Trigger2Strength);
            if (Random.value < 0.03f) _target.RandomizeHue();
        }
    }
}
