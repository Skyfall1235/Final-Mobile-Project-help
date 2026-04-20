using System;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteShapeController)), ExecuteAlways]
public class TerrainSegment : MonoBehaviour
{
    SpriteShapeController m_shapeController;
    [Range(3, 100), SerializeField] int m_terrainResolution = 50;
    [Range(1, 20), SerializeField] int m_overlapCount = 5;
    [SerializeField] Vector2 m_minimums = new(16, 6);
    [SerializeField] Vector2 m_maximums = new(34, 12);
    [SerializeField] float m_floorDistance = 30;
    [SerializeField] float m_maxHeight = 10;
    [SerializeField] float m_minHeight;
    [SerializeField] float m_tangentScale = 1 / 3f;

    public Vector2[] OverlapPoints { get; private set; } = Array.Empty<Vector2>();
    void OnEnable()
    {
        Initialize();
    }
    public void Initialize()
    {
        m_shapeController = GetComponent<SpriteShapeController>();
    }

    public void GenerateTerrain(Vector2[] overlapPoints = null, int? seed = null)
    {
        if (m_shapeController == null) return;
        m_shapeController.spline.Clear();
        Vector2[] points = GenerateTerrainKeyPoints(overlapPoints, seed ?? 0);
        for (int i = 0; i < points.Length; i++)
        {
            m_shapeController.spline.InsertPointAt(i, points[i]);
            if (i <= 0 || i >= points.Length - 1) continue;
            Vector2 tangent = ComputeTangent(points[i - 1], points[i + 1]);
            m_shapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            m_shapeController.spline.SetLeftTangent(i, -tangent);
            m_shapeController.spline.SetRightTangent(i, tangent);
        }
        Vector2 lastPoint = points[^1];
        m_shapeController.spline.InsertPointAt(points.Length, new Vector2(lastPoint.x, transform.position.y - m_floorDistance));
        m_shapeController.spline.InsertPointAt(points.Length + 1, new Vector2(points[0].x, transform.position.y - m_floorDistance));
        int overlapCount = overlapPoints?.Length ?? 0;
        int newPointsStart = Mathf.Max(overlapCount, points.Length - m_overlapCount);
        int copyCount = points.Length - newPointsStart;
        OverlapPoints = new Vector2[copyCount];
        Array.Copy(points, newPointsStart, OverlapPoints, 0, copyCount);
    }

    Vector2[] GenerateTerrainKeyPoints(Vector2[] overlapPoints, int seed)
    {
        int overlapCount = overlapPoints?.Length ?? 0;
        Vector2[] points = new Vector2[overlapCount + m_terrainResolution];
        if (overlapPoints != null) Array.Copy(overlapPoints, points, overlapCount);
        float currentYPosition = overlapCount > 0 ? points[overlapCount - 1].y : transform.position.y;
        float directionalSign = overlapCount % 2 > 0 ? -1f : 1f;
        Random.InitState(seed);
        for (int i = overlapCount; i < points.Length; i++)
        {
            Vector2 prev = i > 0 ? points[i - 1] : new Vector2(transform.position.x, transform.position.y);
            Vector2 steps = new(Random.Range(m_minimums.x, m_maximums.x), Random.Range(m_minimums.y, m_maximums.y));
            points[i].x = prev.x + steps.x;
            points[i].y = Mathf.Clamp(currentYPosition + steps.y * directionalSign, transform.position.y + m_minHeight, transform.position.y + m_maxHeight);
            currentYPosition = points[i].y;
            directionalSign *= -1f;
        }
        return points;
    }

    Vector2 ComputeTangent(Vector2 previous, Vector2 next)
    {
        float slope = (next.y - previous.y) / (next.x - previous.x);
        float xStep = (next.x - previous.x) / 2f;
        return new(xStep * m_tangentScale, slope * m_tangentScale);
    }
}