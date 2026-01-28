using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    
    public int spawnCountMin, spawnCountMax;
    public SpaceTrash[] rockPrefabs;

    public Vector3 spawnBounds;

    public bool drawGizmos;
    public Color GizmosColor = Color.grey;

    List<SpaceTrash> Rocks; 
    void Awake()
    {
        if (SpawnManager.instance == null) instance = this;
        NewWave();
    }

    void OnDrawGizmos()
    {
        if (drawGizmos) { Gizmos.color = GizmosColor; Gizmos.DrawCube(Vector3.zero, spawnBounds); }
    }

    public void NewWave()
    {
        if (Rocks != null) foreach (var r in Rocks) r.Refresh();
        else
        {
            Rocks = new List<SpaceTrash>();
            int n = MainHost.f_Random(spawnCountMin, spawnCountMax);
            for (int i = 0; i < n; i++)
            {
                var rot = Quaternion.Euler(MainHost.f_RandomVector(MainHost.f_BuildVector(-360), MainHost.f_BuildVector(360)));
                var pos = MainHost.f_RandomVector(-spawnBounds, spawnBounds, 2);
                SpaceTrash obj = Instantiate(rockPrefabs[MainHost.f_Random(0, rockPrefabs.Length)], pos, rot);
                Rocks.Add(obj);
            }
        }
    }
}
