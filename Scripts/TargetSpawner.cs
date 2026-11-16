using System.Collections;
using System.Collections.Generic; //Dinamik veri yapýlarýný kullanabilmek için
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Target Prefabs")]
    [Tooltip("Sahnede çýkacak farklý tatlý prefab'larý")]
    public GameObject[] targetPrefabs; // Bir veya birden fazla nesneyi içinde tutmak için

    [Header("Spawn Points")]
    [Tooltip("Tatlýlarýn çýkacaðý boþ noktalar")]
    public Transform[] spawnPoints; // Transform[]; objelerin position, rotation ve scale bilgilerini tutan bileþendir.

    [Header("Settings")]
    public float spawnInterval = 2.0f;

    private int lastTargetIndex = -1; // Deðer verilmezse deffault olarak 0 gelirdi
    private int consecutiveCount = 0; // Ayný tatlýnýn üst üste kaç kez geldiðini saymak için.

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomTarget();
        }
    }

    void SpawnRandomTarget()
    {
        if (targetPrefabs == null || targetPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomPointIndex]; //Transform geçici olarak spawn noktasýnýn koordinatlarýný tutar.

        int selectedTargetIndex = GetValidTargetIndex();

        Instantiate(targetPrefabs[selectedTargetIndex], selectedPoint.position, Quaternion.identity);
        //Instantiate, bir prefabýn kopyasýný çýkarýp sahneye yerleþtirmek için kullanýlan fonksiyondur.
        //Quanternion, hangi açýyla duracaðýný belirler; .identity objeyi hiç dödürme demektir
    }

    int GetValidTargetIndex()
    {
        if (targetPrefabs.Length == 1) return 0;

        int newIndex = Random.Range(0, targetPrefabs.Length);

        while (newIndex == lastTargetIndex && consecutiveCount >= 2)
        {
            newIndex = Random.Range(0, targetPrefabs.Length);
        }

        if (newIndex == lastTargetIndex)
        {
            consecutiveCount++;
        }
        else
        {
            lastTargetIndex = newIndex;
            consecutiveCount = 1;
        }

        return newIndex;
    }
}
