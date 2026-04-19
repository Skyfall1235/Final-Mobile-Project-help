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
    [SerializeField] float m_floorDistance = 10;
    [SerializeField] float m_maxHeight = 10;
    [SerializeField] float m_minHeight;
    [SerializeField] float m_tangentScale = 1 / 3f;

    void OnEnable()
    {
        m_shapeController = GetComponent<SpriteShapeController>();
    }

    void OnValidate()
    {
        GenerateTerrain();
    }

    public Vector2[] GenerateTerrain(Vector2[] overlapPoints = null, int? seed = null)
    {
        if (m_shapeController == null) return null;
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
        m_shapeController.spline.InsertPointAt(points.Length,     new Vector2(lastPoint.x, transform.position.y - m_floorDistance));
        m_shapeController.spline.InsertPointAt(points.Length + 1, new Vector2(points[0].x, transform.position.y - m_floorDistance));
        Vector2[] tail = new Vector2[m_overlapCount];
        int sourceIndex = Mathf.Max(0, points.Length - m_overlapCount);
        int copyCount = Mathf.Min(m_overlapCount, points.Length);
        System.Array.Copy(points, sourceIndex, tail, 0, copyCount);
        return tail;
    }

    Vector2[] GenerateTerrainKeyPoints(Vector2[] overlapPoints, int seed)
    {
        int overlapCount = overlapPoints?.Length ?? 0;
        Vector2[] points = new Vector2[overlapCount + m_terrainResolution];
        if (overlapPoints != null) System.Array.Copy(overlapPoints, points, overlapCount);
        float currentYPosition = overlapCount > 0 ? points[overlapCount - 1].y : transform.position.y;
        float directionalSign = -1f;
        points[overlapCount] = overlapCount > 0 ? points[overlapCount - 1] : new(transform.position.x, transform.position.y);
        Random.InitState(seed);
        for (int i = overlapCount + 1; i < points.Length; i++)
        {
            Vector2 steps = new(Random.Range(m_minimums.x, m_maximums.x), Random.Range(m_minimums.y, m_maximums.y));
            points[i].x = points[i - 1].x + steps.x;
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