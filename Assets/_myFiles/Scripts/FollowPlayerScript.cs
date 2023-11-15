using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPrevPosition;

    private void Start()
    {
        player = GameManager.m_Instance.GetPlayerObject();
        playerPrevPosition = player.transform.position;
    }

    void Update()
    {
        float distance = player.transform.position.z - playerPrevPosition.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
        playerPrevPosition = player.transform.position;
    }
}
