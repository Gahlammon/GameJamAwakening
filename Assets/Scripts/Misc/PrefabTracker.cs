using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTracker : MonoBehaviour
{
    [SerializeField]
    private PrefabHolder prefabHolder = null;

    public GameObject Prefab => prefabHolder.Prefab;
}
