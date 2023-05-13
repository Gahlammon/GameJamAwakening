using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerSpawnPoints : Singleton<ServerPlayerSpawnPoints>
{
    [SerializeField]
    private List<Transform> spawnPoints;

    private IEnumerator<Transform> spawnPointsEnumerator;

    private void Start()
    {
        spawnPointsEnumerator = spawnPoints.GetEnumerator();
    }

    public Transform ConsumeNextSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            return null;
        }

        if(!spawnPointsEnumerator.MoveNext())
        {
            spawnPointsEnumerator.Reset();
            spawnPointsEnumerator.MoveNext();
        }

        Transform point = spawnPointsEnumerator.Current;
        return point;
    }
}
