using UnityEngine;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    [SerializeField] RectTransform leaderboardPanel;

    [SerializeField] float openDuration;

    Vector2 initialPosPanel = new Vector2(-637.1f, 26.1f);
    Vector2 targetPosPanel = new Vector2(1162.9f, 26.1f);

    Vector2 initialPosLeaderboardPanel = new Vector2(322f, 0.375f);
    Vector2 targetPosLeaderboardPanel = new Vector2(-413f, 0.375f);

    bool leaderboardPanelIsOpen;

    public void OpenPanel(RectTransform panel)
    {
        panel.DOAnchorPos(targetPosPanel, openDuration);
        leaderboardPanel.DOAnchorPos(initialPosLeaderboardPanel, openDuration);

        leaderboardPanelIsOpen = false;
    }
    public void ClosePanel(RectTransform panel)
    {
        panel.DOAnchorPos(initialPosPanel, openDuration);
    }

    public void ToggleLeaderboardPanel()
    {
        if (leaderboardPanelIsOpen) leaderboardPanel.DOAnchorPos(initialPosLeaderboardPanel, openDuration);
        else leaderboardPanel.DOAnchorPos(targetPosLeaderboardPanel, openDuration);

        leaderboardPanelIsOpen = !leaderboardPanelIsOpen;
    }
}
