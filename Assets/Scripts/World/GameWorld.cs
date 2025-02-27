using ReputationContent;
using UnityEngine;
using Unity.AI.Navigation;
using Zenject;
using TheSTAR.Data;
using TheSTAR.GUI;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private FastFood fastFood;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private NavMeshSurface navMeshSurface;

    public Transform PlayerSpawnPoint => playerSpawnPoint;
    public FastFood FastFood => fastFood;
    
    [ContextMenu("BakeNavigationSurface")]
    public void BakeNavigationSurface()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void Init(Reputation reputation)
    {
        fastFood.Init(reputation);
    }

    public void Load()
    {
        fastFood.LoadItems();
    }

    public void ResetOrderTray(int index)
    {
        Debug.Log("ResetOrderTray");
        fastFood.ResetOrderTray(index);
    }
}