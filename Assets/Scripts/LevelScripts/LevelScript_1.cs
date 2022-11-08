using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript_1 : MonoBehaviour
{
    [Header("Doors")]
    [SerializeField] private Door[] room1Doors;
    [SerializeField] private Door[] room2Doors;
    [SerializeField] private Door[] room3Doors;
    [SerializeField] private Door[] room4Doors;

    [Header("Fog")]
    [SerializeField] private GameObject[] room1Fog;
    [SerializeField] private GameObject[] room2Fog;
    [SerializeField] private GameObject[] room3Fog;
    [SerializeField] private GameObject[] room4Fog;

    [Header("Enemies")]
    [SerializeField] private GameObject[] room1Enemies;
    [SerializeField] private GameObject[] room2Enemies;
    [SerializeField] private GameObject[] room3Enemies;
    [SerializeField] private GameObject[] room4Enemies;

    private bool isRoom1Open = false;
    private bool isRoom2Open = false;
    private bool isRoom3Open = false;
    private bool isRoom4Open = false;

    private void Awake()
    {
    }

    private void Update()
    {
        if (!isRoom1Open)
        {
            foreach (Door door in room1Doors)
            {
                if (door.IsOpen())
                {
                    isRoom1Open = true;
                    HideFog(room1Fog);
                    ShowEnemies(room1Enemies);
                    break;
                }
            }
        }

        if (!isRoom2Open)
        {
            foreach (Door door in room2Doors)
            {
                if (door.IsOpen())
                {
                    isRoom2Open = true;
                    HideFog(room2Fog);
                    ShowEnemies(room2Enemies);
                    break;
                }
            }
        }

        if (!isRoom3Open)
        {
            foreach (Door door in room3Doors)
            {
                if (door.IsOpen())
                {
                    isRoom3Open = true;
                    HideFog(room3Fog);
                    ShowEnemies(room3Enemies);
                    break;
                }
            }
        }

        if (!isRoom4Open)
        {
            foreach (Door door in room4Doors)
            {
                if (door.IsOpen())
                {
                    isRoom4Open = true;
                    HideFog(room4Fog);
                    ShowEnemies(room4Enemies);
                    break;
                }
            }
        }
    }

    private void HideFog(GameObject[] fogs)
    {
        foreach (GameObject fog in room1Fog)
        {
            fog.SetActive(false);
        }
    }

    private void ShowEnemies(GameObject[] enemies)
    {
        foreach (GameObject enemy in room1Enemies)
        {
            enemy.SetActive(true);
        }
    }
}
