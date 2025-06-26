using UnityEngine;

namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner une capsule 3D de debug
    /// </summary>
    public class DebugCapsule3D : DebugShape
    {
        private Vector3 center;
        private float height;
        private float size;
        private int segments;
        private Quaternion rotation;


        /// <summary>
        /// Dessine ue capsule 3D de debug
        /// </summary>
        /// <param name="center">La position du centre de la cpasule 3D</param>
        /// <param name="rotation">La rotation de la cpasule 3D</param>
        /// <param name="size">La taille de la capsule 3D. Doit être plus grand que 0.</param>
        /// <param name="height">La hauteur de la capsule 3D. Doit être plus grand que 0.</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme de la capsule 3D. Doit être plus grand que 4 et divisible par 4 avec un reste de 0 ou de 1.</param>
        public DebugCapsule3D(Vector3 center, Quaternion rotation, float size, float height, Color color, bool wireframe, float expirationTime, int segments)
        : base(color, expirationTime, wireframe)
        {
            float heightConst = height >= 0 ? height : 0;
            float sizeConst = size >= 0 ? size : 0;

            this.center = center;
            this.height = heightConst + sizeConst * 2;
            this.size = sizeConst;
            this.segments = segments >= 4 ? (segments % 4 == 0 || segments % 4 == 1 ? segments : segments + (4 - segments % 4)) : 4;
            this.rotation = rotation;
        }

        public override void Draw()
        {
            if (wireframe)
            {
                DrawWireframeCapsule();
            }
            else
            {
                DrawSolidCapsule();
            };
        }

        private Vector3 RotatePoint(Vector3 point, Quaternion rotation)
        {
            return rotation * point;
        }

        private void DrawWireframeCapsule()
        {
            GL.Begin(GL.LINES);
            GL.Color(color);
        
            DrawHemisphere(center + rotation * Vector3.up * (height / 2 - size), size, segments, rotation, true);
            DrawHemisphere(center - rotation * Vector3.up * (height / 2 - size), size, segments, rotation, false);

            Vector3 topCenter = center + rotation * Vector3.up * (height / 2 - size);
            Vector3 bottomCenter = center - rotation * Vector3.up * (height / 2 - size);
            for (int i = 0; i < segments; i++)
            {
                float angle1 = 2 * Mathf.PI * i / segments;
                float angle2 = 2 * Mathf.PI * (i + 1) / segments;

                Vector3 p1 = new Vector3(Mathf.Cos(angle1) * size, 0, Mathf.Sin(angle1) * size);
                Vector3 p2 = new Vector3(Mathf.Cos(angle2) * size, 0, Mathf.Sin(angle2) * size);

                GL.Vertex(RotatePoint(p1, rotation) + topCenter);
                GL.Vertex(RotatePoint(p1, rotation) + bottomCenter);

                GL.Vertex(RotatePoint(p1, rotation) + topCenter);
                GL.Vertex(RotatePoint(p2, rotation) + topCenter);

                GL.Vertex(RotatePoint(p1, rotation) + bottomCenter);
                GL.Vertex(RotatePoint(p2, rotation) + bottomCenter);
            }

            GL.End();
        }

        private void DrawSolidCapsule()
        {
            GL.Begin(GL.TRIANGLES);
            GL.Color(color);

            DrawSolidHemisphere(center + rotation * Vector3.up * (height / 2 - size), size, segments, rotation, true);
            DrawSolidHemisphere(center - rotation * Vector3.up * (height / 2 - size), size, segments, rotation, false);

            Vector3 topCenter = center + rotation * Vector3.up * (height / 2 - size);
            Vector3 bottomCenter = center - rotation * Vector3.up * (height / 2 - size);
            for (int i = 0; i < segments; i++)
            {
                float angle1 = 2 * Mathf.PI * i / segments;
                float angle2 = 2 * Mathf.PI * (i + 1) / segments;

                Vector3 p1 = new Vector3(Mathf.Cos(angle1) * size, 0, Mathf.Sin(angle1) * size);
                Vector3 p2 = new Vector3(Mathf.Cos(angle2) * size, 0, Mathf.Sin(angle2) * size);

                GL.Vertex(RotatePoint(p1, rotation) + topCenter);
                GL.Vertex(RotatePoint(p2, rotation) + topCenter);
                GL.Vertex(RotatePoint(p1, rotation) + bottomCenter);

                GL.Vertex(RotatePoint(p2, rotation) + topCenter);
                GL.Vertex(RotatePoint(p2, rotation) + bottomCenter);
                GL.Vertex(RotatePoint(p1, rotation) + bottomCenter);
            }

            GL.End();
        }

        private void DrawHemisphere(Vector3 center, float radius, int segments, Quaternion rotation, bool isTop)
        {
            int rings = segments / 2;
            for (int i = 0; i < rings / 2; i++)
            {
                float theta1 = Mathf.PI * i / rings;
                float theta2 = Mathf.PI * (i + 1) / rings;

                for (int j = 0; j < segments; j++)
                {
                    float phi1 = 2 * Mathf.PI * j / segments;
                    float phi2 = 2 * Mathf.PI * (j + 1) / segments;

                    Vector3 p1 = new Vector3(Mathf.Sin(theta1) * Mathf.Cos(phi1), Mathf.Cos(theta1), Mathf.Sin(theta1) * Mathf.Sin(phi1)) * radius;
                    Vector3 p2 = new Vector3(Mathf.Sin(theta2) * Mathf.Cos(phi1), Mathf.Cos(theta2), Mathf.Sin(theta2) * Mathf.Sin(phi1)) * radius;
                    Vector3 p3 = new Vector3(Mathf.Sin(theta2) * Mathf.Cos(phi2), Mathf.Cos(theta2), Mathf.Sin(theta2) * Mathf.Sin(phi2)) * radius;
                    Vector3 p4 = new Vector3(Mathf.Sin(theta1) * Mathf.Cos(phi2), Mathf.Cos(theta1), Mathf.Sin(theta1) * Mathf.Sin(phi2)) * radius;

                    if (!isTop)
                    {
                        p1.y = -p1.y;
                        p2.y = -p2.y;
                        p3.y = -p3.y;
                        p4.y = -p4.y;
                    }

                    GL.Vertex(RotatePoint(p1, rotation) + center);
                    GL.Vertex(RotatePoint(p2, rotation) + center);
                    GL.Vertex(RotatePoint(p2, rotation) + center);
                    GL.Vertex(RotatePoint(p3, rotation) + center);
                    GL.Vertex(RotatePoint(p3, rotation) + center);
                    GL.Vertex(RotatePoint(p4, rotation) + center);
                    GL.Vertex(RotatePoint(p4, rotation) + center);
                    GL.Vertex(RotatePoint(p1, rotation) + center);
                }
            }
        }

        private void DrawSolidHemisphere(Vector3 center, float radius, int segments, Quaternion rotation, bool isTop)
        {
            int rings = segments / 2;
            for (int i = 0; i < rings; i++)
            {
                float theta1 = Mathf.PI * i / rings;
                float theta2 = Mathf.PI * (i + 1) / rings;

                for (int j = 0; j < segments; j++)
                {
                    float phi1 = 2 * Mathf.PI * j / segments;
                    float phi2 = 2 * Mathf.PI * (j + 1) / segments;

                    Vector3 p1 = new Vector3(Mathf.Sin(theta1) * Mathf.Cos(phi1), Mathf.Cos(theta1), Mathf.Sin(theta1) * Mathf.Sin(phi1)) * radius;
                    Vector3 p2 = new Vector3(Mathf.Sin(theta2) * Mathf.Cos(phi1), Mathf.Cos(theta2), Mathf.Sin(theta2) * Mathf.Sin(phi1)) * radius;
                    Vector3 p3 = new Vector3(Mathf.Sin(theta2) * Mathf.Cos(phi2), Mathf.Cos(theta2), Mathf.Sin(theta2) * Mathf.Sin(phi2)) * radius;
                    Vector3 p4 = new Vector3(Mathf.Sin(theta1) * Mathf.Cos(phi2), Mathf.Cos(theta1), Mathf.Sin(theta1) * Mathf.Sin(phi2)) * radius;

                    if (!isTop)
                    {
                        p1.y = -p1.y;
                        p2.y = -p2.y;
                        p3.y = -p3.y;
                        p4.y = -p4.y;
                    }

                    GL.Vertex(RotatePoint(p1, rotation) + center);
                    GL.Vertex(RotatePoint(p2, rotation) + center);
                    GL.Vertex(RotatePoint(p3, rotation) + center);

                    GL.Vertex(RotatePoint(p1, rotation) + center);
                    GL.Vertex(RotatePoint(p3, rotation) + center);
                    GL.Vertex(RotatePoint(p4, rotation) + center);
                }
            }
        }
    }
}
