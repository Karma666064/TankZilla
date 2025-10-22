using UnityEngine;
using TMPro;
using System.Collections;

public class GameManagerCB : MonoBehaviour
{
    public static GameManagerCB Instance;

    [Header("Joueurs")]
    public PlayerControllerCB player1;
    public PlayerControllerCB player2;

    [Header("UI")]
    public TextMeshProUGUI countdownText;
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;

    [Header("Configuration")]
    public int numberOfQTEsPerTrack = 10;
    public GameObject qtePrefab;

    private bool raceStarted = false;
    private int finishCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (winnerPanel != null)
        {
            winnerPanel.SetActive(false);
        }
        
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        SpawnQTEsForPlayer(player1);
        SpawnQTEsForPlayer(player2);

        StartCoroutine(StartCountdown());
    }

    void SpawnQTEsForPlayer(PlayerControllerCB player)
    {
        if (player == null || player.startPoint == null || player.endPoint == null)
        {
            Debug.LogError("Player ou points de départ/arrivée non assignés !");
            return;
        }

        Debug.Log($"Génération des QTE pour le joueur {player.playerNumber}");

        QTEButton[] spawnedQTEs = new QTEButton[numberOfQTEsPerTrack];
        
        float totalDistance = Vector3.Distance(player.startPoint.position, player.endPoint.position);
        float spacing = totalDistance / (numberOfQTEsPerTrack + 1);

        for (int i = 0; i < numberOfQTEsPerTrack; i++)
        {
            float t = (i + 1) / (float)(numberOfQTEsPerTrack + 1);
            Vector3 spawnPos = Vector3.Lerp(player.startPoint.position, player.endPoint.position, t);
            spawnPos.z = 0; 

            GameObject qteObj = null;
            
            if (qtePrefab != null)
            {
                qteObj = Instantiate(qtePrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                qteObj = Instantiate(Resources.Load<GameObject>("QTEButtonPrefab"), spawnPos, Quaternion.identity);
            }

            if (qteObj != null)
            {
                spawnedQTEs[i] = qteObj.GetComponent<QTEButton>();
                
                if (spawnedQTEs[i] == null)
                {
                    Debug.LogError("Le prefab QTE n'a pas de composant QTEButton !");
                }
                else
                {
                    spawnedQTEs[i].playerNumber = player.playerNumber;
                    spawnedQTEs[i].SetPlayerNumber(player.playerNumber);
                }
            }
            else
            {
                Debug.LogError("Impossible de créer le QTE ! Vérifiez que le prefab est assigné.");
            }
        }

        player.qteButtons = spawnedQTEs;
        Debug.Log($"Joueur {player.playerNumber} : {spawnedQTEs.Length} QTE générés");
    }

    IEnumerator StartCountdown()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                countdownText.fontSize = 120;
                countdownText.color = Color.yellow;
                yield return new WaitForSeconds(1f);
            }

            countdownText.text = "GO !";
            countdownText.fontSize = 150;
            countdownText.color = Color.green;
            yield return new WaitForSeconds(0.8f);

            countdownText.gameObject.SetActive(false);
        }

        if (player1 != null) player1.StartRace();
        if (player2 != null) player2.StartRace();
        
        raceStarted = true;
        finishCount = 0;
    }

    public void PlayerFinished(int playerNumber)
    {
        if (!raceStarted) return;

        finishCount++;

        if (finishCount == 1)
        {
            raceStarted = false;

            if (winnerPanel != null)
            {
                winnerPanel.SetActive(true);
            }

            if (winnerText != null)
            {
                winnerText.text = $"Joueur {playerNumber} a gagné !";
                winnerText.fontSize = 60;
                winnerText.color = Color.green;
            }

            if (player1 != null) player1.enabled = false;
            if (player2 != null) player2.enabled = false;

            Debug.Log($"VICTOIRE - Joueur {playerNumber} !");
        }
    }

    public void RestartRace()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}