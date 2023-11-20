using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;
    private RoomManager roomManager;
    private MusicManager musicManager;

    [SerializeField] private GameObject PlayerObj;
    
    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Debug.LogError("Multiple GameManagers found. Deleting Copy...");
            Destroy(this);
        }
        else
        {
            m_Instance = this;
        }

        PlayerObj = GameObject.FindGameObjectWithTag("Player");
        musicManager = this.AddComponent<MusicManager>();
    }

    private void Start()
    {
        roomManager = GetComponent<RoomManager>();
    }

    public RoomManager GetRoomManager() { return roomManager; }
    public GameObject GetPlayerObject() { return PlayerObj; }

    public MusicManager GetMusicManager() { return musicManager; }
}