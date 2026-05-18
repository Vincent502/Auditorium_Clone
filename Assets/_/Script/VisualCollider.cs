using System.Linq;
using UnityEngine;

public class VisualCollider : MonoBehaviour
{
    [Header("Réglages de la ligne")]
    [Tooltip("Plus ce nombre est élevé, plus le cercle sera lisse.")]
    [SerializeField] private int vertexCount = 40;

    private CircleCollider2D circleCollider;
    private LineRenderer lineRenderer;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        // Configuration essentielle du Line Renderer
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;

        DrawCircleOutline();
    }

    void DrawCircleOutline()
    {
        // On configure le nombre de points du Line Renderer (+1 pour fermer proprement le cercle)
        lineRenderer.positionCount = vertexCount;

        Vector3[] points = new Vector3[vertexCount];

        // Récupération des données du Circle Collider
        float radius = circleCollider.radius;
        Vector2 offset = circleCollider.offset;

        // Calcul de chaque point autour du cercle
        for (int i = 0; i < vertexCount; i++)
        {
            // Calcul de l'angle pour ce point précis (en radians)
            float angle = i * 2 * Mathf.PI / vertexCount;

            // Placement du point en fonction du rayon et de l'offset du collider
            float x = Mathf.Cos(angle) * radius + offset.x;
            float y = Mathf.Sin(angle) * radius + offset.y;

            points[i] = new Vector3(x, y, 0f);
        }

        // Application des points calculés au Line Renderer
        lineRenderer.SetPositions(points);
    }
}
