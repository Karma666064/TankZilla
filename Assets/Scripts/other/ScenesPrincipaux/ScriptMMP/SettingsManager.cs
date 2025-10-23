using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("UI Settings Panel")]
    public GameObject settingsPanel;
    public Button closeButton;

    [Header("Volume")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValueText;

    [Header("Résolution")]
    public TMP_Dropdown resolutionDropdown;

    [Header("Plein Écran")]
    public Toggle fullscreenToggle;

    [Header("Reset Data")]
    public Button resetDataButton;
    public GameObject confirmResetPanel;
    public Button confirmButton;
    public Button cancelButton;
    public TextMeshProUGUI confirmMessageText;

    private Resolution[] resolutions;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Configuration des boutons
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSettings);

        if (resetDataButton != null)
            resetDataButton.onClick.AddListener(ShowResetConfirmation);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(ConfirmReset);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(CancelReset);

        // Configuration des contrôles
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);

        // Initialisation
        SetupResolutions();
        LoadSettings();

        // Cacher les panels au démarrage
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (confirmResetPanel != null)
            confirmResetPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // === VOLUME ===
    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        
        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(value * 100) + "%";

        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    // === RÉSOLUTION ===
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
        if (resolutions == null || index >= resolutions.Length) return;

        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    // === PLEIN ÉCRAN ===
    void OnFullscreenChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // === CHARGEMENT DES PARAMÈTRES ===
    void LoadSettings()
    {
        // Volume
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
            AudioListener.volume = volume;
        }
        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";

        // Résolution
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length - 1);
        if (resolutionDropdown != null)
            resolutionDropdown.value = resIndex;

        // Plein écran
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = fullscreen;
        Screen.fullScreen = fullscreen;
    }

    // === RESET DONNÉES ===
    void ShowResetConfirmation()
    {
        if (confirmResetPanel != null)
        {
            confirmResetPanel.SetActive(true);
        }

        if (confirmMessageText != null)
        {
            confirmMessageText.text = "⚠️ Réinitialiser toutes les données ?\n\nCette action est irréversible !";
        }
    }

    void ConfirmReset()
    {
        // Réinitialiser GameDataManager
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.ResetGame();
        }

        // Supprimer toutes les données sauvegardées
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Recharger les paramètres par défaut
        LoadSettings();

        if (confirmMessageText != null)
        {
            confirmMessageText.text = "✅ Données réinitialisées !";
            confirmMessageText.color = Color.green;
        }

        Debug.Log("Toutes les données ont été réinitialisées !");

        // Fermer après 1.5 secondes
        Invoke(nameof(CancelReset), 1.5f);
    }

    void CancelReset()
    {
        if (confirmResetPanel != null)
        {
            confirmResetPanel.SetActive(false);
        }

        if (confirmMessageText != null)
        {
            confirmMessageText.text = "";
            confirmMessageText.color = Color.white;
        }
    }
}