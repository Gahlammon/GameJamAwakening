using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Prefab Holder")]
public class PrefabHolder : ScriptableObject
{
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab => prefab;
}
