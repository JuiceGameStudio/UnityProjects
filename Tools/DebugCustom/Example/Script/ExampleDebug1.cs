using UnityEngine;
using CustomSystem.Debug.Draw;
using CustomSystem.Debug;

public class ExampleDebug1 : MonoBehaviour, IDebugSystem
{
    public bool localDebugEnabled { get; set; } = false;

    public Transform start;
    public Transform end;

    public float size;
    public Vector3 size3;
    public float width;
    public int segments;
    public float height;
    public bool isWireframe;
    public Color color;

    public bool Line = false;

    public bool Square = false;
    public bool Cube = false;

    public bool Circle = false;
    public bool Sphere = false;

    public bool Cone2D = false;
    public bool Cone3D = false;

    public bool Capsule2D = false;
    public bool Capsule3D = false;

    void Update()
    {
        if (Line) DebugDraw.Line(start.position, end.position, color);

        if (Square) DebugDraw.Square(start.position, start.rotation, size3, color, isWireframe);
        if (Cube) DebugDraw.Cube(start.position, start.rotation, size3, color, isWireframe);

        if (Circle) DebugDraw.Circle(start.position, start.rotation, size, color, isWireframe, segments);
        if (Sphere) DebugDraw.Sphere(start.position, start.rotation, size, color, isWireframe, segments);

        if (Cone2D) DebugDraw.Cone2D(start.position, start.rotation, height, size, color, isWireframe);
        if (Cone3D) DebugDraw.Cone3D(start.position, start.rotation, height, size, color, isWireframe, segments);

        if (Capsule2D) DebugDraw.Capsule2D(start.position, start.rotation, size, height, color, isWireframe, segments);
        if (Capsule3D) DebugDraw.Capsule3D(start.position, start.rotation, height, size, color, isWireframe, segments);
    }
}
