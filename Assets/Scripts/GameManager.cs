using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DualPantoFramework;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    private GameObject enemy;
    private GameObject gameObjPlayer;
    Player player;
    PantoCollider[] pantoColliders;
    public bool gameStarted = false;

    async void Start()
    {

        await StartGame();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindObjectOfType<Player>();
        gameObjPlayer = GameObject.FindGameObjectWithTag("Player");
        pantoColliders = GameObject.FindObjectsOfType<PantoCollider>();
        await Task.Delay(1000);
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.CreateObstacle();
            collider.Enable();
        }

    }

    async void Update() // async update...
    {
        if (!gameStarted) return;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            foreach (PantoCollider collider in pantoColliders)
            {
                Destroy(collider);
            }
        }
        if (player.playerHealth < 0) 
        {
            gameStarted = false;
            await Task.Delay(2000);
            gameObjPlayer.transform.position = player.startPosition;
            await StartGame();

        }
    }

    public async Task StartGame()
    {
        // Level level = GameObject.Find("Panto").GetComponent<Level>();
        // await level.PlayIntroduction(1.0f);
        // await GameObject.FindObjectOfType<PantoIntro>().RunIntros();
        await GameObject.FindObjectOfType<Enemy>().ActivateEnemy();
        await GameObject.FindObjectOfType<Player>().ActivatePlayer();
        GameObject.FindGameObjectWithTag("Key").GetComponent<CapsuleCollider>().isTrigger = true;
        gameStarted = true;
    }

    public async void LoadNewScene()
    {
        gameStarted = false;
        SceneManager.LoadScene("Level 2");
        await StartGame();
    }
}
