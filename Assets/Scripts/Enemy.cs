using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using System.Threading.Tasks;

public class Enemy : MonoBehaviour
{
    PantoHandle itHandle;
    public Vector3 startPosition;
    private Vector3 direction = Vector3.right;
    private AudioSource audioSource;
    public AudioClip weakHitClip;
    public float speed = 1.0f;

    void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        itHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    void FixedUpdate()
    {
        if (!GameObject.FindObjectOfType<GameManager>().gameStarted) return;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            audioSource.PlayOneShot(weakHitClip);
            direction *= -1;
        }
    }


    public async Task ActivateEnemy()
    {
        await itHandle.SwitchTo(gameObject, speed);
    }
}
