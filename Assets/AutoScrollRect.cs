using UnityEngine;
using UnityEngine.UI;

public class AutoScrollRect : MonoBehaviour
{
    public ScrollRect scroll;              // kéo ScrollRect vào đây (hoặc để trống, script tự lấy)
    public float pixelsPerSecond = 50f;    // tốc độ cuộn (px/giây)
    public bool loop = true;               // lên đỉnh thì quay lại đáy

    RectTransform content;

    void Awake()
    {
        if (!scroll) scroll = GetComponent<ScrollRect>();
        content = scroll ? scroll.content : null;
    }

    void OnEnable()
    {
        Canvas.ForceUpdateCanvases();
        if (scroll) scroll.verticalNormalizedPosition = 0f; // 0 = đáy, 1 = đỉnh
    }

    void Update()
    {
        if (!scroll || !content) return;

        float usable = content.rect.height - scroll.viewport.rect.height;
        if (usable <= 1f) return;

        float deltaNorm = (pixelsPerSecond * Time.unscaledDeltaTime) / usable;
        float v = scroll.verticalNormalizedPosition + deltaNorm; // đi từ đáy lên

        scroll.verticalNormalizedPosition = loop ? (v >= 1f ? 0f : v) : Mathf.Clamp01(v);
    }
}
