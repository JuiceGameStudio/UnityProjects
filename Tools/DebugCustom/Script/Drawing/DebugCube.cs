using UnityEngine;
namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script pour dessiner un cube de debug
    /// </summary>
    public class DebugCube : DebugShape
    {
        Vector3 center;
        Vector3 size;
        Quaternion rotation;

        /// <summary>
        /// Dessine un cube de debug
        /// </summary>
        /// <param name="center">La position du centre du cube</param>
        /// <param name="rotation">La rotation du cube</param>
        /// <param name="size">La taille du cube en x,y,z</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="wireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="expirationTime">Le temps avant l'expiration du debug</param>
        public DebugCube(Vector3 center, Quaternion rotation, Vector3 size, Color color, bool wireframe, float expirationTime)
            : base(color, expirationTime, wireframe)
        {
            this.center = center;
            this.rotation = rotation;
            this.size = size;
        }

        public override void Draw()
        {
            Vector3 halfExtents = new Vector3(size.x, size.y, size.z) / 2f;

            Vector3[] vertices = {
                center + rotation * new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z),
                center + rotation * new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z),
                center + rotation * new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z),
                center + rotation * new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z),

                center + rotation * new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z),
                center + rotation * new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z),
                center + rotation * new Vector3(halfExtents.x, halfExtents.y, halfExtents.z),
                center + rotation * new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z),
            };

            int[][] faceIndices = {
                new int[] { 0, 1, 2, 3 }, 
                new int[] { 4, 5, 6, 7 }, 
                new int[] { 0, 4, 5, 1 }, 
                new int[] { 1, 5, 6, 2 }, 
                new int[] { 2, 6, 7, 3 }, 
                new int[] { 3, 7, 4, 0 }  
            };

            GL.Begin(wireframe ? GL.LINES : GL.QUADS);
            GL.Color(color);

            foreach (var indices in faceIndices)
            {
                for (int i = 0; i < indices.Length; i++)
                {
                    GL.Vertex(vertices[indices[i]]);
                    if (wireframe) GL.Vertex(vertices[indices[(i + 1) % indices.Length]]);
                }
            }

            GL.End();
        }
    }
}
