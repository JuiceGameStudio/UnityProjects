using UnityEngine;
namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner un cercle de debug
    /// </summary>
    public class DebugCircle : DebugShape
    {
        Vector3 center;
        float radius;
        int segments;
        Quaternion rotation;

        /// <summary>
        /// Dessine un cercle de debug
        /// </summary>
        /// <param name="center">Le position du centre du cercle</param>
        /// <param name="rotation">>La rotation du cercle</param>
        /// <param name="radius">Le rayon du cercle</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        /// <param name="segments">Le nombre de segments pour dessiner le cercle. Doit être au moins de trois</param>
        public DebugCircle(Vector3 center, Quaternion rotation, float radius, Color color, bool wireframe, float expirationTime, int segments)
            : base(color, expirationTime, wireframe)
        {
            this.center = center;
            this.rotation = rotation;
            this.radius = radius;
            this.segments = segments >= 3 ? segments : 3;
        }

        public override void Draw()
        {
            GL.Begin(wireframe ? GL.LINE_STRIP : GL.TRIANGLES);
            GL.Color(color);

            float angleIncrement = 2 * Mathf.PI / segments;

            if (wireframe)
            {
                for (int i = 0; i <= segments; i++)
                {
                    Vector3 vertex = CalculateVertex(i * angleIncrement, rotation);
                    GL.Vertex(vertex);
                }
            }
            else
            {
                for (int i = 0; i < segments; i++)
                {
                    Vector3 vertex1 = CalculateVertex(i * angleIncrement, rotation);
                    Vector3 vertex2 = CalculateVertex((i + 1) * angleIncrement, rotation);

                    GL.Vertex(center);
                    GL.Vertex(vertex1);
                    GL.Vertex(vertex2);
                }
            }

            GL.End();
        }

        Vector3 CalculateVertex(float angle, Quaternion rotationQuaternion)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            return rotationQuaternion * new Vector3(x, y, 0) + center;
        }
    }
}
