using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private float RoomLength = 15.1812f;

    [Serializable]
    private struct Room
    {
        public RoomType Type;
        public GameObject RoomObject;
    }

    [SerializeField] private Room[] rooms;

    private Dictionary<RoomType, GameObject> roomDictionary = new Dictionary<RoomType, GameObject>(); //Dictionaries are easier and faster to search then struct arrays :p

    [SerializeField] private List<GameObject> generatedRooms = new List<GameObject>();

    [SerializeField] private int MaxRooms;

    private void Start()
    {
        foreach (Room room in rooms) // I have a new hatred for the word room
        {
            roomDictionary.Add(room.Type, room.RoomObject);
        }
        generatedRooms.AddRange(GameObject.FindGameObjectsWithTag("Room"));
    }

    public float GetRoomLength() { return RoomLength; }
    public void SetRoomLength(float newRoomLEngth) { RoomLength = newRoomLEngth; }

    public void GenerateNewRoom(Transform PreviousLocation)
    {
        int RandRoomNum = UnityEngine.Random.Range(0, roomDictionary.Keys.Count);
        GameObject RandRoom = roomDictionary[roomDictionary.ElementAt(RandRoomNum).Key]; //There is a better way to do this... but this is COOLER so TAKE THAT MOM

        Vector3 newPos = new Vector3(PreviousLocation.transform.position.x, PreviousLocation.transform.position.y, PreviousLocation.transform.position.z + RoomLength);

        generatedRooms.Add(Instantiate(RandRoom, newPos, PreviousLocation.transform.rotation));


        if (generatedRooms.Count > MaxRooms)
        {
            Destroy(generatedRooms[0]);
            generatedRooms.RemoveAt(0);
        }
    }
}

enum RoomType { Empty, Crouch, Jump, Pit, COUNT }