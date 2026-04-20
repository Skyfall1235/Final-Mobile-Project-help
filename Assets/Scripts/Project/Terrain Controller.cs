using System.Collections.Generic;
using JetBrains.Annotations;
using MatrixUtils.Attributes;
using UnityEngine;
using UnityEngine.Pool;

public class TerrainController : MonoBehaviour
{
    IObjectPool<TerrainSegment> m_terrainSegmentPool;
    [SerializeField, RequiredField] TerrainSegment m_segmentPrefab;
    readonly LinkedList<TerrainSegment> m_activeSegments = new();
    void Start()
    {
        m_terrainSegmentPool = new ObjectPool<TerrainSegment>(CreateTerrainSegment);

        TerrainSegment first = m_terrainSegmentPool.Get();
        first.Initialize();
        GenerateTerrain(first);
        first.gameObject.SetActive(true);
        m_activeSegments.AddLast(first);

        for (int i = 0; i < 10; i++)
        {
            TerrainSegment previousSegment = m_activeSegments.Last.Value;
            TerrainSegment segment = m_terrainSegmentPool.Get();
            segment.Initialize();
            GenerateTerrain(segment, previousSegment);
            segment.gameObject.SetActive(true);
            m_activeSegments.AddLast(segment);
        }
    }
    void GenerateTerrain(TerrainSegment segment, TerrainSegment priorSegment = null)
    {
        if(segment == null) return;
        if (priorSegment?.OverlapPoints.Length > 0)
        {
            Debug.Log($"Generating terrain with {priorSegment.OverlapPoints.Length} overlap points");
            segment.GenerateTerrain(priorSegment.OverlapPoints);
        }
        else
        {
            segment.GenerateTerrain();
        }
        
    }
    TerrainSegment CreateTerrainSegment()
    {
        TerrainSegment segment = Instantiate(m_segmentPrefab);
        segment.gameObject.SetActive(false);
        return segment;
    }
}