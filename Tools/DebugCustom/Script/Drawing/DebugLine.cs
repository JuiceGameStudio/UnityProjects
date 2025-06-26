using UnityEngine;
namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner une ligne de debug
    /// </summary>
    public class DebugLine : DebugShape
    {
        Vector3 start;
        Vector3 end;

        /// <summary>
        /// Dessine une ligne de debug
        /// </summary>
        /// <param name="start">La position du début de la ligne</param>
        /// <param name="end">La position de fin de la ligne</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        public DebugLine(Vector3 start, Vector3 end, Color color, bool wireframe, float expirationTime)
            : base(color, expirationTime, wireframe)
        {
            this.start = start;
            this.end = end;
        }

        public override void Draw()
        {
            GL.Begin(GL.LINES);
            GL.Color(color);

            GL.Vertex(start);
            GL.Vertex(end);

            GL.End();
        }
    }
}
