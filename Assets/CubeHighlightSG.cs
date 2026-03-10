using UnityEngine;

public class CubeHighlightSG : MonoBehaviour
{
    public float highlightValue = 1f;

    Material mat;
    float originalValue;
    static readonly int HighlightId = Shader.PropertyToID("_Highlight");

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        mat = renderer.material;
        if (mat.HasProperty(HighlightId))
            originalValue = mat.GetFloat(HighlightId);
    }

    void OnMouseEnter()
    {
        if (mat != null && mat.HasProperty(HighlightId))
            mat.SetFloat(HighlightId, highlightValue);
    }

    void OnMouseExit()
    {
        if (mat != null && mat.HasProperty(HighlightId))
            mat.SetFloat(HighlightId, originalValue);
    }
}
