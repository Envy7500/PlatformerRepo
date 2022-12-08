using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    

    public void RespawnNow()
    {
        transform.position = respawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("spikes"))
        {
            RespawnNow();
        }
    }
}
