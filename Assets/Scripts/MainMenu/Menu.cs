using UnityEngine;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    [SerializeField] float openDuration;

    Vector2 initialPosPanel = new Vector2(-637.1f, 26.1f);
    Vector2 targetPosPanel = new Vector2(1162.9f, 26.1f);

    public void OpenPanel(RectTransform panel)
    {
        panel.DOAnchorPos(targetPosPanel, openDuration);
    }
    public void ClosePanel(RectTransform panel)
    {
        panel.DOAnchorPos(initialPosPanel, openDuration);
    }
}
