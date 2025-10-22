using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panneaux UI")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Boutons Menu Principal")]
    public Button playButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Paramètres Audio")]
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeValueText;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeValueText;
    public Toggle muteToggle;
    public TextMeshProUGUI muteToggleLabel;

    [Header("Paramètres Graphiques")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle vsyncToggle;
    public TMP_Dropdown targetFramerateDropdown;

    [Header("Paramètres de Jeu")]
    public TMP_Dropdown difficultyDropdown;
    public Toggle tutorialToggle;

    [Header("Données")]
    public Button resetLeaderboardButton;
    public GameObject resetConfirmationPanel;
    public Button confirmResetButton;
    public Button cancelResetButton;
    public TextMeshProUGUI resetStatusText;

    [Header("Boutons Navigation")]
    public Button backButton;
    public Button creditsBackButton;
    public Button applyButton;
    public Button defaultsButton;

    private Resolution[] resolutions;
    private bool hasUnsavedChanges = false;

    void Start()
    {
        // S'assurer que le GameDataManager existe
        if (GameDataManager.Instance == null)
        {
            GameObject gdm = new GameObject("GameDataManager");
            gdm.AddComponent<GameDataManager>();
        }

        // Afficher le panneau principal
        ShowMainPanel();

        // Configuration des boutons principaux
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);
        
        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsClicked);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
        
        if (backButton != null)
            backButton.onClick.AddListener(OnBackFromSettings);
        
        if (creditsBackButton != null)
            creditsBackButton.onClick.AddListener(ShowMainPanel);

        // Boutons settings
        if (applyButton != null)
            applyButton.onClick.AddListener(OnApplySettings);
        
        if (defaultsButton != null)
            defaultsButton.onClick.AddListener(OnRestoreDefaults);

        // Boutons reset leaderboard
        if (resetLeaderboardButton != null)
            resetLeaderboardButton.onClick.AddListener(OnResetLeaderboardClicked);
        
        if (confirmResetButton != null)
            confirmResetButton.onClick.AddListener(OnConfirmReset);
        
        if (cancelResetButton != null)
            cancelResetButton.onClick.AddListener(OnCancelReset);

        if (resetConfirmationPanel != null)
            resetConfirmationPanel.SetActive(false);

        // Charger les paramètres
        LoadSettings();
        SetupResolutions();
        SetupQualityLevels();
        SetupFramerates();

        // Listeners pour détecter les changements
        SetupChangeListeners();
    }

    void SetupChangeListeners()
    {
        // Audio
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener((value) => { UpdateVolumeText(masterVolumeValueText, value); hasUnsavedChanges = true; });
        
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener((value) => { UpdateVolumeText(musicVolumeValueText, value); hasUnsavedChanges = true; });
        
        if (muteToggle != null)
            muteToggle.onValueChanged.AddListener((value) => { UpdateMuteLabel(); hasUnsavedChanges = true; });

        // Graphiques
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener((value) => hasUnsavedChanges = true);
        
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener((value) => hasUnsavedChanges = true);
        
        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener((value) => hasUnsavedChanges = true);
        
        if (vsyncToggle != null)
            vsyncToggle.onValueChanged.AddListener((value) => hasUnsavedChanges = true);
        
        if (targetFramerateDropdown != null)
            targetFramerateDropdown.onValueChanged.AddListener((value) => hasUnsavedChanges = true);

        // Jeu
        if (difficultyDropdown != null)
            difficultyDropdown.onValueChanged.AddListener((value) => hasUnsavedChanges = true);
        
        if (tutorialToggle != null)
            tutorialToggle.onValueChanged.AddListener((value) => hasUnsavedChanges = true);
    }

    void ShowMainPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    void OnPlayClicked()
    {
        GameDataManager.Instance.ResetGame();
        string firstGame = GameDataManager.Instance.GetNextMiniGameScene();
        SceneManager.LoadScene(firstGame);
    }

    void OnSettingsClicked()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        hasUnsavedChanges = false;
        LoadSettings(); // Recharger pour annuler les changements non sauvegardés
    }

    void OnCreditsClicked()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    void OnBackFromSettings()
    {
        if (hasUnsavedChanges)
        {
            // Optionnel : Afficher une confirmation
            Debug.Log("Changements non sauvegardés annulés");
        }
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

    // === PARAMÈTRES AUDIO ===
    void UpdateVolumeText(TextMeshProUGUI textElement, float value)
    {
        if (textElement != null)
        {
            textElement.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }

    void UpdateMuteLabel()
    {
        if (muteToggleLabel != null && muteToggle != null)
        {
            muteToggleLabel.text = muteToggle.isOn ? "Activé" : "Désactivé";
        }
    }

    // === PARAMÈTRES GRAPHIQUES ===
    void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void SetupQualityLevels()
    {
        if (qualityDropdown == null) return;

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    void SetupFramerates()
    {
        if (targetFramerateDropdown == null) return;

        targetFramerateDropdown.ClearOptions();
        System.Collections.Generic.List<string> framerates = new System.Collections.Generic.List<string>
        {
            "30 FPS",
            "60 FPS",
            "120 FPS",
            "144 FPS",
            "Illimité"
        };
        targetFramerateDropdown.AddOptions(framerates);
    }

    // === APPLIQUER LES PARAMÈTRES ===
    void OnApplySettings()
    {
        // Audio
        if (masterVolumeSlider != null)
        {
            float masterVolume = masterVolumeSlider.value;
            AudioListener.volume = masterVolume;
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        }

        if (musicVolumeSlider != null)
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        }

        if (muteToggle != null)
        {
            bool isMuted = muteToggle.isOn;
            AudioListener.pause = isMuted;
            PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        }

        // Graphiques
        if (fullscreenToggle != null)
        {
            Screen.fullScreen = fullscreenToggle.isOn;
            PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        }

        if (resolutionDropdown != null && resolutions != null)
        {
            Resolution resolution = resolutions[resolutionDropdown.value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        }

        if (qualityDropdown != null)
        {
            QualitySettings.SetQualityLevel(qualityDropdown.value);
            PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);
        }

        if (vsyncToggle != null)
        {
            QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
            PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        }

        if (targetFramerateDropdown != null)
        {
            int[] fpsValues = { 30, 60, 120, 144, -1 };
            Application.targetFrameRate = fpsValues[targetFramerateDropdown.value];
            PlayerPrefs.SetInt("TargetFramerate", targetFramerateDropdown.value);
        }

        // Jeu
        if (difficultyDropdown != null)
        {
            PlayerPrefs.SetInt("Difficulty", difficultyDropdown.value);
        }

        if (tutorialToggle != null)
        {
            PlayerPrefs.SetInt("ShowTutorial", tutorialToggle.isOn ? 1 : 0);
        }

        PlayerPrefs.Save();
        hasUnsavedChanges = false;

        Debug.Log("Paramètres sauvegardés !");
    }

    // === RESTAURER PAR DÉFAUT ===
    void OnRestoreDefaults()
    {
        // Audio
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = 1f;
            UpdateVolumeText(masterVolumeValueText, 1f);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = 0.8f;
            UpdateVolumeText(musicVolumeValueText, 0.8f);
        }

        if (muteToggle != null)
        {
            muteToggle.isOn = false;
            UpdateMuteLabel();
        }

        // Graphiques
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = true;

        if (qualityDropdown != null)
            qualityDropdown.value = 2; // Medium/High

        if (vsyncToggle != null)
            vsyncToggle.isOn = true;

        if (targetFramerateDropdown != null)
            targetFramerateDropdown.value = 1; // 60 FPS

        // Jeu
        if (difficultyDropdown != null)
            difficultyDropdown.value = 1; // Normal

        if (tutorialToggle != null)
            tutorialToggle.isOn = true;

        hasUnsavedChanges = true;
        Debug.Log("Paramètres par défaut restaurés (non sauvegardés)");
    }

    // === CHARGEMENT DES PARAMÈTRES ===
    void LoadSettings()
    {
        // Audio
        if (masterVolumeSlider != null)
        {
            float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            masterVolumeSlider.value = masterVolume;
            AudioListener.volume = masterVolume;
            UpdateVolumeText(masterVolumeValueText, masterVolume);
        }

        if (musicVolumeSlider != null)
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            musicVolumeSlider.value = musicVolume;
            UpdateVolumeText(musicVolumeValueText, musicVolume);
        }

        if (muteToggle != null)
        {
            bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
            muteToggle.isOn = isMuted;
            AudioListener.pause = isMuted;
            UpdateMuteLabel();
        }

        // Graphiques
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        if (qualityDropdown != null)
            qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());

        if (vsyncToggle != null)
            vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;

        if (targetFramerateDropdown != null)
            targetFramerateDropdown.value = PlayerPrefs.GetInt("TargetFramerate", 1);

        if (resolutionDropdown != null)
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length - 1);

        // Jeu
        if (difficultyDropdown != null)
            difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty", 1);

        if (tutorialToggle != null)
            tutorialToggle.isOn = PlayerPrefs.GetInt("ShowTutorial", 1) == 1;
    }

    // === RESET LEADERBOARD ===
    void OnResetLeaderboardClicked()
    {
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(true);
        }
    }

    void OnConfirmReset()
    {
        // Réinitialiser toutes les données de jeu
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.ResetGame();
        }

        // Supprimer tous les scores sauvegardés
        PlayerPrefs.DeleteKey("HighScores");
        PlayerPrefs.DeleteKey("BestTimes");
        PlayerPrefs.Save();

        // Afficher message de confirmation
        if (resetStatusText != null)
        {
            resetStatusText.text = "✓ Classement réinitialisé !";
            resetStatusText.color = Color.green;
        }

        Debug.Log("Leaderboard réinitialisé !");

        // Fermer le panneau après 1.5 secondes
        if (resetConfirmationPanel != null)
        {
            Invoke(nameof(CloseResetPanel), 1.5f);
        }
    }

    void OnCancelReset()
    {
        CloseResetPanel();
    }

    void CloseResetPanel()
    {
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(false);
        }

        if (resetStatusText != null)
        {
            resetStatusText.text = "";
        }
    }
}