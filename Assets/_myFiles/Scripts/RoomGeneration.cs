using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    bool hasBeenActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenActivated)   
        {
            GameManager.m_Instance.GetRoomManager().GenerateNewRoom(transform.parent);
            hasBeenActivated = true;
        }
    }
}
