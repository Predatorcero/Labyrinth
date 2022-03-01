using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DualPantoFramework;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    PantoHandle meHandle;
    private GameObject enemy;
    private GameObject player;
    Player playerScript;
    Enemy enemyScript;
    PantoCollider[] pantoColliders;

    private int currentLevel = 0;
    public bool gameStarted = false;
    public bool advancedMode = false;
    public bool openedDoors = false;

    async void Start()
    {
        await StartGame();
        currentLevel++;
        FindObjectOfType<Player>().playerDeathEvent += OnPlayerDeath;
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        playerScript = GameObject.FindObjectOfType<Player>();
        enemyScript = GameObject.FindObjectOfType<Enemy>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        pantoColliders = GameObject.FindObjectsOfType<PantoCollider>();
        await Task.Delay(1000);
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.CreateObstacle();
            collider.Enable();
        }

    }

    void Update()
    {
        if (!gameStarted) return;
    }

    async public void OnPlayerDeath(Player player)
    {
        gameStarted = false;
        await Task.Delay(2000);
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.Disable();
        }
        player.transform.position = playerScript.startPosition;
        enemy.transform.position = enemyScript.startPosition;
        await StartGame();
        foreach (PantoCollider collider in pantoColliders)
        {
            if (collider.gameObject.tag == "Door" && openedDoors) {
                continue;
            }
            collider.Enable();
        }
    }

    public async Task StartGame()
    {
        // Level level = GameObject.Find("Panto").GetComponent<Level>();
        // await level.PlayIntroduction(1.0f);
        await GameObject.FindObjectOfType<PantoIntro>().RunIntros();
        if (advancedMode) {
            await GameObject.FindObjectOfType<EnemyAI>().ActivateEnemyAI();
        } else {
            await GameObject.FindObjectOfType<Enemy>().ActivateEnemy();
        }
        await GameObject.FindObjectOfType<Player>().ActivatePlayer();
        gameStarted = true;
    }


    public async Task LoadNewScene()
    {
        gameStarted = false;
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.Remove();
        }
        SceneManager.LoadScene("Level " + (currentLevel + 1));
        await StartGame();
        pantoColliders = GameObject.FindObjectsOfType<PantoCollider>();
        await Task.Delay(1000);
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.CreateObstacle();
            collider.Enable();
        }
    }
    public async void EndGame()
    {
        Debug.Log("YOU WON!");
    }
}
