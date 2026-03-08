using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlashGlitch {

[CustomEditor(typeof(FlashGlitchController)), CanEditMultipleObjects]
sealed class FlashGlitchControllerEditor : Editor
{
    [SerializeField] VisualTreeAsset _uxml = null;

    const float DefaultStrength = 1;

    public override VisualElement CreateInspectorGUI()
    {
        var ui = _uxml.CloneTree();
        ui.Q<Button>("effect1-button").clicked += OnEffect1Button;
        ui.Q<Button>("effect2-button").clicked += OnEffect2Button;
        return ui;
    }

    void OnEffect1Button()
    {
        foreach (FlashGlitchController controller in targets)
            controller.TriggerEffect1(DefaultStrength);
    }

    void OnEffect2Button()
    {
        foreach (FlashGlitchController controller in targets)
            controller.TriggerEffect2(DefaultStrength);
    }
}

} // namespace FlashGlitch
