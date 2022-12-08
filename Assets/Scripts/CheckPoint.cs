using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private PlayerRespawn playerRespawn;


    // Start is called before the first frame update
    void Start()
    {
        playerRespawn = GameObject.Find("Player Variant").GetComponent<PlayerRespawn>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player Variant")
        {
            playerRespawn.respawnPoint = transform.position;
        }
    }
}
