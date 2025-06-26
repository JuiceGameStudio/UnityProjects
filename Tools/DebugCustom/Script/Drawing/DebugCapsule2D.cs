using UnityEngine;

namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner une capsule 2D de debug
    /// </summary>
    public class DebugCapsule2D : DebugShape
    {
        private Vector3 center;
        private float size;
        private float height;
        private float radius;
        private int segments;
        private Quaternion rotation;

        /// <summary>
        /// Dessine ue capsule 2D de debug
        /// </summary>
        /// <param name="center">La position du centre de la cpasule 2D</param>
        /// <param name="rotation">La rotation de la cpasule 2D</param>
        /// <param name="size">La taille de la capsule 2D. Doit être plus grand que 0.</param>
        /// <param name="height">La hauteur de la capsule 2D. Doit être plus grand que 0.</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme de la cpasule 2D. Doit être plus grand que 3.</param>
        public DebugCapsule2D(Vector3 center, Quaternion rotation, float size, float height, Color color, bool wireframe, float expirationTime, int segments)
        : base(color, expirationTime, wireframe)
        {
            float heightConst = height >= 0 ? height : 0;
            float sizeConst = size >= 0 ? size : 0;

            this.center = center;
            this.size = sizeConst * 2;
            this.height = heightConst + sizeConst * 2;
            radius = sizeConst;
            this.segments = segments >= 3 ? segments : 3;
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
            }
        }

        private void DrawWireframeCapsule()
        {
            GL.Begin(GL.LINES);
            GL.Color(color);

            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.PI * i / segments;
                float angle2 = Mathf.PI * (i + 1) / segments;
                Vector3 vertex1 = center + rotation * new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius + height / 2 - radius, 0);
                Vector3 vertex2 = center + rotation * new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius + height / 2 - radius, 0);
                GL.Vertex(vertex1);
                GL.Vertex(vertex2);
            }

            Vector3 rightStart = center + rotation * new Vector3(radius, height / 2 - radius, 0);
            Vector3 rightEnd = center + rotation * new Vector3(radius, -height / 2 + radius, 0);
            GL.Vertex(rightStart);
            GL.Vertex(rightEnd);

            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.PI * i / segments + Mathf.PI;
                float angle2 = Mathf.PI * (i + 1) / segments + Mathf.PI;
                Vector3 vertex1 = center + rotation * new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius - height / 2 + radius, 0);
                Vector3 vertex2 = center + rotation * new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius - height / 2 + radius, 0);
                GL.Vertex(vertex1);
                GL.Vertex(vertex2);
            }

            Vector3 leftStart = center + rotation * new Vector3(-radius, height / 2 - radius, 0);
            Vector3 leftEnd = center + rotation * new Vector3(-radius, -height / 2 + radius, 0);
            GL.Vertex(leftStart);
            GL.Vertex(leftEnd);

            GL.End();
        }

        private void DrawSolidCapsule()
        {
            GL.Begin(GL.TRIANGLES);
            GL.Color(color);

            Vector3 topLeft = center + rotation * new Vector3(-size / 2, height / 2 - radius, 0);
            Vector3 topRight = center + rotation * new Vector3(size / 2, height / 2 - radius, 0);
            Vector3 bottomLeft = center + rotation * new Vector3(-size / 2, -height / 2 + radius, 0);
            Vector3 bottomRight = center + rotation * new Vector3(size / 2, -height / 2 + radius, 0);

            GL.Vertex(topLeft);
            GL.Vertex(topRight);
            GL.Vertex(bottomRight);

            GL.Vertex(topLeft);
            GL.Vertex(bottomRight);
            GL.Vertex(bottomLeft);

            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.PI * i / segments;
                float angle2 = Mathf.PI * (i + 1) / segments;
                Vector3 vertex1 = center + rotation * new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius + height / 2 - radius, 0);
                Vector3 vertex2 = center + rotation * new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius + height / 2 - radius, 0);

                GL.Vertex(center + rotation * new Vector3(0, height / 2 - radius, 0));
                GL.Vertex(vertex1);
                GL.Vertex(vertex2);
            }


            for (int i = 0; i < segments; i++)
            {
                float angle1 = Mathf.PI * i / segments + Mathf.PI;
                float angle2 = Mathf.PI * (i + 1) / segments + Mathf.PI;
                Vector3 vertex1 = center + rotation * new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius - height / 2 + radius, 0);
                Vector3 vertex2 = center + rotation * new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius - height / 2 + radius, 0);

                GL.Vertex(center + rotation * new Vector3(0, -height / 2 + radius, 0));
                GL.Vertex(vertex1);
                GL.Vertex(vertex2);
            }

            GL.End();
        }
    }
}
