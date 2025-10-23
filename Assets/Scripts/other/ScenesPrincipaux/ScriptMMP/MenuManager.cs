using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Boutons Menu")]
    public Button playButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Panneaux")]
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public Button creditsBackButton;

    void Start()
    {
        // S'assurer que le GameDataManager existe
        if (GameDataManager.Instance == null)
        {
            GameObject gdm = new GameObject("GameDataManager");
            gdm.AddComponent<GameDataManager>();
        }

        // S'assurer que le SettingsManager existe
        if (SettingsManager.Instance == null)
        {
            GameObject sm = new GameObject("SettingsManager");
            sm.AddComponent<SettingsManager>();
        }

        // Configuration des boutons
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        if (creditsBackButton != null)
            creditsBackButton.onClick.AddListener(OnCreditsBack);

        // Afficher le menu principal
        ShowMainPanel();
    }

    void ShowMainPanel()
    {
        if (mainPanel != null)
            mainPanel.SetActive(true);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    void OnPlayClicked()
    {
        // Réinitialiser et commencer le jeu
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.ResetGame();
            string firstGame = GameDataManager.Instance.GetNextMiniGameScene();
            SceneManager.LoadScene(firstGame);
        }
    }

    void OnSettingsClicked()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.ShowSettings();
        }
    }

    void OnCreditsClicked()
    {
        if (mainPanel != null)
            mainPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    void OnCreditsBack()
    {
        ShowMainPanel();
    }

    void OnQuitClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}