using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerControllerCB : MonoBehaviour
{
    [Header("Configuration")]
    public int playerNumber = 1;
    public float moveDistancePerQTE = 5f; // Distance parcourue à chaque bon QTE
    public float moveSpeed = 8f; // Vitesse de déplacement
    public float penaltyDuration = 2f;
    
    [Header("Références")]
    public Transform startPoint;
    public Transform endPoint;
    public QTEButton[] qteButtons;
    
    [Header("Input System")]
    public InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    
    private bool isRaceActive = false;
    private bool isPenalized = false;
    private bool isMovingToNextQTE = false;
    private int currentQTEIndex = 0;
    private bool hasFinished = false;
    private Vector3 targetPosition;
    private QTEButton.ButtonType? lastPressedButton = null;

    void Awake()
    {
        if (inputActions != null)
        {
            string actionMapName = playerNumber == 1 ? "KataPlayer1" : "KataPlayer2";
            playerActionMap = inputActions.FindActionMap(actionMapName);
            
            if (playerActionMap != null)
            {
                // NOUVEAU MAPPING pour Player1:
                // S → A, D → B, Q → X, Z → Y
                
                // Vérifier que chaque action existe avant de s'abonner
                InputAction actionA = playerActionMap.FindAction("ButtonA");
                if (actionA != null)
                {
                    actionA.performed += OnButtonA; // S pour P1, A pour P2
                }
                else
                {
                    Debug.LogError($"Action 'ButtonA' non trouvée dans {actionMapName} !");
                }
                
                InputAction actionB = playerActionMap.FindAction("ButtonB");
                if (actionB != null)
                {
                    actionB.performed += OnButtonB; // D pour P1, B pour P2
                }
                else
                {
                    Debug.LogError($"Action 'ButtonB' non trouvée dans {actionMapName} !");
                }
                
                InputAction actionX = playerActionMap.FindAction("ButtonX");
                if (actionX != null)
                {
                    actionX.performed += OnButtonX; // Q pour P1, X pour P2
                }
                else
                {
                    Debug.LogError($"Action 'ButtonX' non trouvée dans {actionMapName} !");
                }
                
                InputAction actionY = playerActionMap.FindAction("ButtonY");
                if (actionY != null)
                {
                    actionY.performed += OnButtonY; // Z pour P1, Y pour P2
                }
                else
                {
                    Debug.LogError($"Action 'ButtonY' non trouvée dans {actionMapName} !");
                }
                
                playerActionMap.Enable();
            }
            else
            {
                Debug.LogError($"Action Map '{actionMapName}' non trouvé !");
            }
        }
        else
        {
            Debug.LogError("Input Actions Asset non assigné sur PlayerControllerCB !");
        }
    }

    void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            targetPosition = transform.position;
        }
    }

    void Update()
    {
        if (!isRaceActive || hasFinished) return;

        // Déplacement progressif vers la position cible
        if (isMovingToNextQTE)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Vérifier si on a atteint la cible
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMovingToNextQTE = false;
                
                // Vérifier si la course est terminée
                if (currentQTEIndex >= qteButtons.Length)
                {
                    hasFinished = true;
                    GameManagerCB.Instance.PlayerFinished(playerNumber);
                }
                else
                {
                    // Afficher le prochain QTE
                    ShowCurrentQTE();
                }
            }
        }

        // Traiter l'input reçu si on est en attente d'un QTE
        if (!isMovingToNextQTE && !isPenalized && lastPressedButton.HasValue)
        {
            CheckQTEInput(lastPressedButton.Value);
            lastPressedButton = null;
        }
    }

    private void OnButtonA(InputAction.CallbackContext context)
    {
        if (isRaceActive && !hasFinished)
            lastPressedButton = QTEButton.ButtonType.A;
    }

    private void OnButtonB(InputAction.CallbackContext context)
    {
        if (isRaceActive && !hasFinished)
            lastPressedButton = QTEButton.ButtonType.B;
    }

    private void OnButtonX(InputAction.CallbackContext context)
    {
        if (isRaceActive && !hasFinished)
            lastPressedButton = QTEButton.ButtonType.X;
    }

    private void OnButtonY(InputAction.CallbackContext context)
    {
        if (isRaceActive && !hasFinished)
            lastPressedButton = QTEButton.ButtonType.Y;
    }

    void ShowCurrentQTE()
    {
        if (currentQTEIndex < qteButtons.Length)
        {
            qteButtons[currentQTEIndex].ShowQTE();
        }
    }

    void CheckQTEInput(QTEButton.ButtonType pressedButton)
    {
        if (currentQTEIndex < qteButtons.Length)
        {
            QTEButton currentQTE = qteButtons[currentQTEIndex];
            bool isCorrect = currentQTE.CheckInput(pressedButton);
            
            if (isCorrect)
            {
                Debug.Log($"Player {playerNumber} : Bonne touche !");
                
                // Cacher le QTE validé
                currentQTE.HideQTE();
                
                // Calculer la prochaine position
                currentQTEIndex++;
                
                if (currentQTEIndex < qteButtons.Length)
                {
                    targetPosition = qteButtons[currentQTEIndex].transform.position;
                }
                else
                {
                    // Aller à la ligne d'arrivée
                    targetPosition = endPoint.position;
                }
                
                targetPosition.z = 0;
                isMovingToNextQTE = true;
            }
            else
            {
                Debug.Log($"Player {playerNumber} : Mauvaise touche ! Pénalité !");
                StartCoroutine(ApplyPenalty());
            }
        }
    }

    IEnumerator ApplyPenalty()
    {
        isPenalized = true;
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = Color.white;
        
        if (sr != null)
        {
            originalColor = sr.color;
            sr.color = Color.red;
        }
        
        yield return new WaitForSeconds(penaltyDuration);
        
        if (sr != null)
        {
            sr.color = originalColor;
        }
        
        isPenalized = false;
    }

    public void StartRace()
    { 
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            targetPosition = transform.position;
        }
        
        isRaceActive = true;
        hasFinished = false;
        currentQTEIndex = 0;
        isPenalized = false;
        isMovingToNextQTE = false;
        
        // Cacher tous les QTE au début
        foreach (QTEButton qte in qteButtons)
        {
            qte.HideQTE();
        }
        
        // Afficher le premier QTE
        ShowCurrentQTE();
    }

    void OnDestroy()
    {
        if (playerActionMap != null)
        {
            InputAction actionA = playerActionMap.FindAction("ButtonA");
            if (actionA != null)
            {
                actionA.performed -= OnButtonA;
            }
            
            InputAction actionB = playerActionMap.FindAction("ButtonB");
            if (actionB != null)
            {
                actionB.performed -= OnButtonB;
            }
            
            InputAction actionX = playerActionMap.FindAction("ButtonX");
            if (actionX != null)
            {
                actionX.performed -= OnButtonX;
            }
            
            InputAction actionY = playerActionMap.FindAction("ButtonY");
            if (actionY != null)
            {
                actionY.performed -= OnButtonY;
            }
            
            playerActionMap.Disable();
        }
    }
}