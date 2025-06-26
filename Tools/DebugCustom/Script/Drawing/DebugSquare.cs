using System.Linq;
using UnityEngine;
namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner un carré de debug
    /// </summary>
    public class DebugSquare : DebugShape
    {
        Vector3 center;
        Quaternion rotation;
        Vector2 size;

        /// <summary>
        /// Dessine un carré de debug
        /// </summary>
        /// <param name="center">La position du centre du carré</param>
        /// <param name="rotation">La rotation du carré</param>
        /// <param name="size">La taille du carré en x,y</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        public DebugSquare(Vector3 center, Quaternion rotation, Vector2 size, Color color, bool wireframe, float expirationTime)
            : base(color, expirationTime, wireframe)
        {
            this.center = center;
            this.rotation = rotation;
            this.size = size;
            this.wireframe = wireframe;
        }

        public override void Draw()
        {
            GL.Begin(wireframe ? GL.LINE_STRIP : GL.QUADS);
            GL.Color(color);

            Vector3[] corners = new Vector3[4];
            corners[0] = rotation * new Vector2(-size.x / 2, -size.y / 2) + center;
            corners[1] = rotation * new Vector2(size.x / 2, -size.y / 2) + center;
            corners[2] = rotation * new Vector2(size.x / 2, size.y / 2) + center;
            corners[3] = rotation * new Vector2(-size.x / 2, size.y / 2) + center;

            corners.ToList().ForEach(corner => GL.Vertex(corner));
            if (wireframe) GL.Vertex(corners[0]);

            GL.End();
        }
    }
}
