using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {
    private static GameController instance;
    [SerializeField] private Player player1, player2;
    [SerializeField] private GameObject arrowPlayer1, arrowPlayer2;
    private bool gameStart = false;

    public float delayChangeTile;
    [HideInInspector] public int finishedAims = 0;

    public static GameController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start() {
        
    }

    private void Update() {
        if (!gameStart) {
            if (finishedAims == 0) {
                arrowPlayer1.SetActive(true);
                arrowPlayer2.SetActive(false);
            }
            else if (finishedAims == 1) {
                arrowPlayer1.SetActive(false);
                arrowPlayer2.SetActive(true);
            }
            else {
                arrowPlayer1.SetActive(false);
                arrowPlayer2.SetActive(false);
                gameStart = true;
                Debug.Log("Jogo vai começar!!!");
                StartCoroutine(StartGameDelay());
            }
        }
    }

    private IEnumerator StartGameDelay() {
        yield return new WaitForSeconds(1f);
        player1.StartMovement();
        player2.StartMovement();
    }
}
