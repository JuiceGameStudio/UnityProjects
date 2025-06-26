using UnityEngine;
namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner un cone 3D de debug
    /// </summary>
    public class DebugCone3D : DebugShape
    {
        Vector3 center;
        Quaternion rotation;
        float height;
        private float baseLenght;
        int segments;

        /// <summary>
        /// Dessine un cone 3D de debug
        /// </summary>
        /// <param name="center">La position du sommet du cone 3D</param>
        /// <param name="rotation">La rotation du cone 3D</param>
        /// <param name="height">La hauteur du cone</param>
        /// <param name="baseLenght">La longeur de la base du cone cone 3D</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme du cone 3D. Doit être plus grand que 3.</param>
        public DebugCone3D(Vector3 center, Quaternion rotation, float height, float baseLenght, Color color, bool wireframe, float expirationTime, int segments)
            : base(color, expirationTime, wireframe)
        {
            this.center = center;
            this.rotation = rotation;
            this.height = height;
            this.baseLenght = baseLenght;
            this.segments = segments >= 3 ? segments : 3;
        }

        public override void Draw()
        {
            GL.Begin(wireframe ? GL.LINES : GL.TRIANGLES);
            GL.Color(color);

            Vector3 baseCenter = center + rotation * new Vector3(0, -height, 0);

            for (int i = 0; i <= segments; i++)
            {
                float angle = i * 2 * Mathf.PI / segments;
                Vector3 basePoint = baseCenter + rotation * new Vector3(Mathf.Cos(angle) * baseLenght, 0, Mathf.Sin(angle) * baseLenght);

                if (wireframe)
                {
                    GL.Vertex(center);
                    GL.Vertex(basePoint);

                    if (i > 0)
                    {
                        float prevAngle = (i - 1) * 2 * Mathf.PI / segments;
                        Vector3 prevBasePoint = baseCenter + rotation * new Vector3(Mathf.Cos(prevAngle) * baseLenght, 0, Mathf.Sin(prevAngle) * baseLenght);
                        GL.Vertex(prevBasePoint);
                        GL.Vertex(basePoint);
                    }
                }
                else if (i < segments)
                {
                    float nextAngle = (i + 1) * 2 * Mathf.PI / segments;
                    Vector3 nextBasePoint = baseCenter + rotation * new Vector3(Mathf.Cos(nextAngle) * baseLenght, 0, Mathf.Sin(nextAngle) * baseLenght);

                    GL.Vertex(center);
                    GL.Vertex(basePoint);
                    GL.Vertex(nextBasePoint);

                    GL.Vertex(baseCenter);
                    GL.Vertex(basePoint);
                    GL.Vertex(nextBasePoint);
                }
            }

            GL.End();
        }
    }
}
