using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlashGlitchURP {

public sealed class FlashGlitchRendererFeature : ScriptableRendererFeature
{
    [SerializeField]
    RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;

    FlashGlitchPass _pass;

    public override void Create()
      => _pass = new FlashGlitchPass { renderPassEvent = _passEvent };

    public override void AddRenderPasses
      (ScriptableRenderer renderer, ref RenderingData renderingData)
      => renderer.EnqueuePass(_pass);
}

} // namespace FlashGlitchURP
