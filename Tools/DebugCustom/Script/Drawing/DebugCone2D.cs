using UnityEngine;
namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner un cone 2D de debug
    /// </summary>
    public class DebugCone2D : DebugShape
    {
        Vector3 basePosition;
        Quaternion rotation;
        float height;
        float baseLenght;

        /// <summary>
        /// Dessine un cone en 2D
        /// </summary>
        /// <param name="basePosition">La position du départ du cone 2D</param>
        /// <param name="rotation">La rotation du cone 2D</param>
        /// <param name="height">La hauteur du cone 2D</param>
        /// <param name="baseLenght">La longeur de la base du cone 2D</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        public DebugCone2D(Vector3 basePosition, Quaternion rotation, float height, float baseLenght, Color color, bool wireframe, float expirationTime)
            : base(color, expirationTime, wireframe)
        {
            this.basePosition = basePosition;
            this.rotation = rotation;
            this.height = height;
            this.baseLenght = baseLenght;
        }

        public override void Draw()
        {
            GL.Begin(wireframe ? GL.LINES : GL.TRIANGLES);
            GL.Color(color);

            Vector3 baseCenter = basePosition + rotation * new Vector3(0, -height, 0);
            Vector3 leftBase = baseCenter + rotation * new Vector3(-baseLenght, 0, 0);
            Vector3 rightBase = baseCenter + rotation * new Vector3(baseLenght, 0, 0);

            GL.Vertex(basePosition);
            GL.Vertex(leftBase);

            if (wireframe)
            {
                GL.Vertex(basePosition);
                GL.Vertex(rightBase);

                GL.Vertex(leftBase);
            }

            GL.Vertex(rightBase);
            GL.End();
        }
    }
}
