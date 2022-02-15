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
    // TODO: after collision with enemy reduce health
    // implement in GameManager: after 3 health lost, lose game
    public int playerHealth = 1;
    public float speed = 3f;

    public AudioClip hummingSound;
    public AudioClip keyCollectClip;
    public AudioClip enemyCollisionHit;
    private AudioSource audioSource;

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
        // transform.position = meHandle.HandlePosition(transform.position);
        transform.eulerAngles = new Vector3(0, meHandle.GetRotation(), 0);
        float volumeScale = GetRelDistToKey(keyPosition);
        audioSource.PlayOneShot(hummingSound, volumeScale);

    }
/*
    IEnumerator MoveToStart()
    {
        Debug.Log(transform.position);
        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.01f);
            yield return null;
        }
    }
*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            audioSource.PlayOneShot(enemyCollisionHit);
            playerHealth--;
            // meHandle.MoveToPosition(startPosition, speed, true); // async fix...
            // StartCoroutine(MoveToStart());
        }

        if (other.gameObject.tag == "Key")
        {
            if (GameObject.FindObjectsOfType<Door>() != null) {
                audioSource.PlayOneShot(keyCollectClip);
                foreach (var gameObj in GameObject.FindObjectsOfType<Door>())
                {
                    gameObj.Open();
                }
            }
        }
        if (other.gameObject.tag == "SceneSwitcher")
        {
            GameObject.FindObjectOfType<GameManager>().LoadNewScene();
        }
    }
    private float GetRelDistToKey(Vector3 keyPosition)
    {
        float currentDistance = Vector3.Distance(transform.position, keyPosition);
        return 1 - currentDistance / totalDistanceToKey < 0 ? 0 : 1 - currentDistance / totalDistanceToKey;
    }

    public async Task ActivatePlayer()
    {
        await meHandle.SwitchTo(gameObject, speed);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
        rb.useGravity = false;
        meHandle.Free();
    }

}
