using UnityEngine;
namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner un sphere de debug
    /// </summary>
    public class DebugSphere : DebugShape
    {
        Vector3 center;
        Quaternion rotation;
        float radius;
        int segments;

        /// <summary>
        /// Dessine une sphere de debug
        /// </summary>
        /// <param name="center">Le position du centre de la sphere</param>
        /// <param name="rotation">La rotation de la sphere</param>
        /// <param name="radius">Le rayon de la sphere</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme de la sphere. Doit être plus grand que 4 et pair.</param>
        public DebugSphere(Vector3 center, Quaternion rotation, float radius, Color color, bool wireframe, float expirationTime, int segments)
            : base(color, expirationTime, wireframe)
        {
            this.center = center;
            this.rotation = rotation;
            this.radius = radius;
            this.segments = segments > 4 ? segments % 2 == 0 ? segments : segments + 1 : 4;
        }

        public override void Draw()
        {
            GL.Begin(wireframe ? GL.LINES : GL.TRIANGLES);
            GL.Color(color);

            float step = Mathf.PI * 2 / segments;

            for (int i = 0; i < segments; i++)
            {
                float theta = i * step;
                float nextTheta = (i + 1) * step;

                for (int j = 0; j < segments / 2; j++)
                {
                    float phi = j * step;
                    float nextPhi = (j + 1) * step;

                    Vector3 v1 = new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta));
                    Vector3 v2 = new Vector3(Mathf.Sin(phi) * Mathf.Cos(nextTheta), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(nextTheta));
                    Vector3 v3 = new Vector3(Mathf.Sin(nextPhi) * Mathf.Cos(theta), Mathf.Cos(nextPhi), Mathf.Sin(nextPhi) * Mathf.Sin(theta));
                    Vector3 v4 = new Vector3(Mathf.Sin(nextPhi) * Mathf.Cos(nextTheta), Mathf.Cos(nextPhi), Mathf.Sin(nextPhi) * Mathf.Sin(nextTheta));

                    GL.Vertex(rotation * (v1 * radius) + center);
                    if (!wireframe) GL.Vertex(rotation * (v3 * radius) + center);
                    GL.Vertex(rotation * (v2 * radius) + center);

                    if (!wireframe) GL.Vertex(rotation * (v2 * radius) + center);
                    if (wireframe) GL.Vertex(rotation * (v1 * radius) + center);
                    GL.Vertex(rotation * (v3 * radius) + center);
                    if (!wireframe) GL.Vertex(rotation * (v4 * radius) + center);
                }
            }

            GL.End();
        }
    }
}
