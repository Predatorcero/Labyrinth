using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    PantoHandle meHandle;
    private GameObject key;

    public Vector3 startPosition;
    private Vector3 keyPosition;
    private float totalDistanceToKey;
    public int playerHealth = 3;
    public float speed = 1f;

    public AudioClip hummingSound;
    public AudioClip keyCollectClip;
    public AudioClip enemyCollisionHit;
    private AudioSource audioSource;

    public delegate void playerDeathDelegate(Player p);
    public event playerDeathDelegate playerDeathEvent;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
        key = GameObject.FindGameObjectWithTag("Key");
        startPosition = transform.position;
        keyPosition = key.transform.position;
        totalDistanceToKey = Vector3.Distance(startPosition, keyPosition);
    }

    void FixedUpdate()
    {
        if (!GameObject.FindObjectOfType<GameManager>().gameStarted) return;
        transform.position = meHandle.HandlePosition(transform.position);
        transform.eulerAngles = new Vector3(0, meHandle.GetRotation(), 0);
        float volumeScale = GetRelDistToKey(keyPosition);
        audioSource.PlayOneShot(hummingSound, volumeScale);

    }

    // IEnumerator MoveToStart()
    // {
    //     Debug.Log(transform.position);
    //     while (transform.position != startPosition)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.2f);
    //         yield return null;
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            audioSource.PlayOneShot(enemyCollisionHit);
            playerHealth--;
            if (playerHealth < 1) {
                Die();
                playerHealth = 3;
            }
        }

        if (other.gameObject.tag == "Key")
        {
            if (GameObject.FindObjectOfType<GameManager>().openedDoors == false) {
                audioSource.PlayOneShot(keyCollectClip);
                foreach (var collider in GameObject.FindObjectsOfType<PantoCollider>())
                {
                    if (collider.tag == "Door") {
                        collider.Disable();
                    }
                }
                foreach (var door in GameObject.FindObjectsOfType<Door>())
                {
                    door.Open();
                }
                
            }
        }
        if (other.gameObject.tag == "SceneSwitcher")
        {
            GameObject.FindObjectOfType<GameManager>().LoadNewScene();
        }

        if (other.gameObject.tag == "Finish")
        {
            GameObject.FindObjectOfType<GameManager>().EndGame();
        }

    }
    private void Die()
    {
        playerDeathEvent(this);
    }

    private float GetRelDistToKey(Vector3 keyPosition)
    {
        float currentDistance = Vector3.Distance(transform.position, keyPosition);
        return 1 - currentDistance / totalDistanceToKey < 0 ? 0 : 1 - currentDistance / totalDistanceToKey;
    }

    public async Task ActivatePlayer()
    {
        await meHandle.SwitchTo(gameObject, speed);
        meHandle.Free();
    }
}
