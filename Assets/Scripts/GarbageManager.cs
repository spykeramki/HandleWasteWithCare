using System;
using UnityEngine;

public class GarbageManager : MonoBehaviour
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

    private void Start()
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
            Instantiate(garbageToBeGenerated.garbageCtrl, GetRandomPositionOfWastage(garbageToBeGenerated), Quaternion.identity);
        }
    }


    private Vector3 GetRandomPositionOfWastage(GarbagePrefabWithRangeOfGeneration garbageToBeGenerated)
    {
        float randomAngleInRadians = UnityEngine.Random.Range(0f, 2 * Mathf.PI);

        float xPos = Mathf.Sin(randomAngleInRadians);
        float yPos = Mathf.Cos(randomAngleInRadians);

        float randomRangeBetweenMinAndMax = UnityEngine.Random.Range(garbageToBeGenerated.minDistanceFromBase, garbageToBeGenerated.maxDistanceFromBase);

        Vector3 randomPositionOfWastage = new Vector3(xPos* randomRangeBetweenMinAndMax, 1f, yPos* randomRangeBetweenMinAndMax);

        return randomPositionOfWastage;
    }
}
