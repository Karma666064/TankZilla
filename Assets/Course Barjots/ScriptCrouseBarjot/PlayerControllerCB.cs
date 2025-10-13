using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerControllerCB : MonoBehaviour
{
    [Header("Configuration")]
    public int playerNumber = 1;
    public float moveDistancePerQTE = 5f;
    public float moveSpeed = 8f;
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
            string actionMapName = playerNumber == 1 ? "Player1" : "Player2";
            playerActionMap = inputActions.FindActionMap(actionMapName);
            
            if (playerActionMap != null)
            {
                playerActionMap.FindAction("ButtonA").performed += OnButtonA;
                playerActionMap.FindAction("ButtonB").performed += OnButtonB;
                playerActionMap.FindAction("ButtonX").performed += OnButtonX;
                playerActionMap.FindAction("ButtonY").performed += OnButtonY;
                
                playerActionMap.Enable();
            }
            else
            {
                Debug.LogError($"Action Map '{actionMapName}' non trouvé !");
            }
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

        if (isMovingToNextQTE)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMovingToNextQTE = false;
                
                if (currentQTEIndex >= qteButtons.Length)
                {
                    hasFinished = true;
                    GameManagerCB.Instance.PlayerFinished(playerNumber);
                }
                else
                {
                    ShowCurrentQTE();
                }
            }
        }
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
                currentQTE.HideQTE();
                
                currentQTEIndex++;
                
                if (currentQTEIndex < qteButtons.Length)
                {
                    targetPosition = qteButtons[currentQTEIndex].transform.position;
                }
                else
                {
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
        
        foreach (QTEButton qte in qteButtons)
        {
            qte.HideQTE();
        }
        ShowCurrentQTE();
    }

    void OnDestroy()
    {
        if (playerActionMap != null)
        {
            playerActionMap.FindAction("ButtonA").performed -= OnButtonA;
            playerActionMap.FindAction("ButtonB").performed -= OnButtonB;
            playerActionMap.FindAction("ButtonX").performed -= OnButtonX;
            playerActionMap.FindAction("ButtonY").performed -= OnButtonY;
            
            playerActionMap.Disable();
        }
    }
}