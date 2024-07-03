using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool gameRunning;
    private GameObject player1;
    private GameObject player2;

    [HideInInspector] public float player1Points;
    [HideInInspector] public float player2Points;
    private float currentTime;

    [Header("Score Texts")]
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public TextMeshProUGUI timerText;
    [Header("Players")]
    public PlayerController player1prefab;
    public PlayerController player2prefab;
    [Header("Player Spawns")]
    public GameObject player1Spawn;
    public GameObject player2Spawn;
    [Header("Fruits and Snek")]
    public List<GameObject> treeSpawnables;
    [Header("Bush Spawns")]
    public List<GameObject> bushSpawnables;
    [Header("Tree Spawn Points")]
    public Transform highLeft;
    public Transform highRight;
    public Transform lowLeft;
    public Transform lowLeftR;
    public Transform lowRight;
    public Transform lowRightL;
    [Header("Bush Spawn Points")]
    public Transform bushLeft;
    public Transform bushRight;
    [Header("Varibles")]
    public float treeSpawnIntervalMin;
    public float treeSpawnIntervalMax;
    public float bushSpawnIntervalMin;
    public float bushSpawnIntervalMax;
    public float playTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = playTime;
        UpdateTimerDisplay();

        player1 = Instantiate(player1prefab.gameObject, player1Spawn.transform.position, player1Spawn.transform.rotation);
        player2 = Instantiate(player2prefab.gameObject, player2Spawn.transform.position, player2Spawn.transform.rotation);

        StartCoroutine(SpawnTreeObjects());
        StartCoroutine(SpawnBushObjects());
    }

    public void UpdateScore(GameObject player, int points)
    {
        if (player == player1)
        {
            player1Points += points;
            player1Score.text = player1Points.ToString();
        }
        else if (player == player2)
        {
            player2Points += points;
            player2Score.text = player2Points.ToString();
        }
        else
        {
            Debug.Log("Player could not be identified");
        }
    }

    private IEnumerator SpawnTreeObjects()
    {
        gameRunning = true;
        while (gameRunning)
        {
            // Randomly determine how long to wait before changing direction
            float waitTime = Random.Range(treeSpawnIntervalMin, treeSpawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            GameObject choosenSpawn = treeSpawnables[Random.Range(0, treeSpawnables.Count)];
            if (Random.Range(0,5) != 4)
            {
                Instantiate(choosenSpawn, new Vector3 (Random.Range(highLeft.position.x, highRight.position.x), highLeft.position.y), choosenSpawn.transform.rotation);
            }
            else
            {
                int randomNum = (Random.Range(0, 2));
                Debug.Log("Random Spawn lower = " + randomNum);
                bool leftOrRight = randomNum == 0 ? true : false;
                if (leftOrRight)
                {
                    Instantiate(choosenSpawn, new Vector3(Random.Range(lowLeft.position.x, lowLeftR.position.x), Random.Range(lowLeft.position.y, lowLeftR.position.y)), choosenSpawn.transform.rotation);
                }
                else
                {
                    Instantiate(choosenSpawn, new Vector3(Random.Range(lowRight.position.x, lowRightL.position.x), Random.Range(lowRight.position.y, lowRightL.position.y)), choosenSpawn.transform.rotation);
                }
            }
        }
    }

    private IEnumerator SpawnBushObjects()
    {
        gameRunning = true;
        while (gameRunning)
        {
            // Randomly determine how long to wait before changing direction
            float waitTime = Random.Range(bushSpawnIntervalMin, bushSpawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            GameObject choosenSpawn = bushSpawnables[Random.Range(0, bushSpawnables.Count)];
            int randomNum = (Random.Range(0, 2));
            float spawnLocation = randomNum == 0 ? bushLeft.position.x : bushRight.position.x;
            Instantiate(choosenSpawn, new Vector3(spawnLocation, bushLeft.position.y), choosenSpawn.transform.rotation);
        }
    }

    void UpdateTimerDisplay()
    {
        // Display only seconds
        int seconds = Mathf.FloorToInt(currentTime);
        string timerString = seconds.ToString();

        // Update the timer text
        timerText.text = timerString;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
            // Handle timer expiration (e.g., game over)
        }
    }
}
