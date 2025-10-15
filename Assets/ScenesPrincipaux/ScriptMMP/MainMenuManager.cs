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
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle muteToggle;

    [Header("Paramètres Graphiques")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    [Header("Autres")]
    public Button backButton;
    public Button creditsBackButton;

    private Resolution[] resolutions;

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

        // Configuration des boutons
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);
        
        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsClicked);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
        
        if (backButton != null)
            backButton.onClick.AddListener(ShowMainPanel);
        
        if (creditsBackButton != null)
            creditsBackButton.onClick.AddListener(ShowMainPanel);

        // Charger les paramètres
        LoadSettings();
        SetupResolutions();
        SetupQualityLevels();

        // Listeners pour les paramètres
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        
        if (muteToggle != null)
            muteToggle.onValueChanged.AddListener(OnMuteToggled);
        
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        
        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    void ShowMainPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    void OnPlayClicked()
    {
        // Réinitialiser le jeu et commencer
        GameDataManager.Instance.ResetGame();
        
        // Charger le premier mini-jeu
        string firstGame = GameDataManager.Instance.GetNextMiniGameScene();
        SceneManager.LoadScene(firstGame);
    }

    void OnSettingsClicked()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    void OnCreditsClicked()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
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
    void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void OnMusicVolumeChanged(float value)
    {
        // À implémenter avec votre système audio
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    void OnSFXVolumeChanged(float value)
    {
        // À implémenter avec votre système audio
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void OnMuteToggled(bool isMuted)
    {
        AudioListener.pause = isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
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

    void OnResolutionChanged(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

    void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    void SetupQualityLevels()
    {
        if (qualityDropdown == null) return;

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityLevel", index);
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
        }

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (muteToggle != null)
        {
            bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
            muteToggle.isOn = isMuted;
            AudioListener.pause = isMuted;
        }

        // Graphiques
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        if (qualityDropdown != null)
            qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
    }
}