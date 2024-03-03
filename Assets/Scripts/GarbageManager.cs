using System;
using Unity.Netcode;
using UnityEngine;

public class GarbageManager : NetworkBehaviour
{
    public enum GarbageType
    {
        PLASTIC,
        GLASS,
        OIL,
        ORGANIC,
        RADIOACTIVE,
        NONE
    }

    [SerializeField]
    private Transform baseTransform;

    [Serializable]
    public struct GarbagePrefabWithRangeOfGeneration
    {
        public GarbageCtrl garbageCtrl;
        public float minDistanceFromBase;
        public float maxDistanceFromBase;
        public int noOfWasteToBeGenerated;
    }

    [SerializeField]
    private GarbagePrefabWithRangeOfGeneration[] garbagePrefabsWithRangeOfGeneration;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            SpawnWastage();
        }
    }
    private void SpawnWastage()
    {
        for (int i = 0; i < garbagePrefabsWithRangeOfGeneration.Length; i++)
        {
            GenerateRandomWastage(garbagePrefabsWithRangeOfGeneration[i]);
        }
    }


    private void GenerateRandomWastage(GarbagePrefabWithRangeOfGeneration garbageToBeGenerated)
    {

        for (int i = 0; i < garbageToBeGenerated.noOfWasteToBeGenerated; i++)
        {
            GarbageCtrl garbageCtrl = Instantiate(garbageToBeGenerated.garbageCtrl, GetRandomPositionOfWastage(garbageToBeGenerated), Quaternion.identity);
            garbageCtrl.GetComponent<NetworkObject>().Spawn(true);
        }
    }


    private Vector3 GetRandomPositionOfWastage(GarbagePrefabWithRangeOfGeneration garbageToBeGenerated)
    {
        float randomAngleInRadians = UnityEngine.Random.Range(0f, 2 * Mathf.PI);

        float xPos = Mathf.Sin(randomAngleInRadians);
        float yPos = Mathf.Cos(randomAngleInRadians);

        float randomRangeBetweenMinAndMax = UnityEngine.Random.Range(garbageToBeGenerated.minDistanceFromBase, garbageToBeGenerated.maxDistanceFromBase);

        Vector3 randomPositionOfWastage = new Vector3(xPos* randomRangeBetweenMinAndMax, 46f, yPos* randomRangeBetweenMinAndMax);

        return randomPositionOfWastage;
    }
}
