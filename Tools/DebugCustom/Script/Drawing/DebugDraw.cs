using UnityEngine;
using System.Collections.Generic;

namespace CustomSystem.Debug.Draw
{
    /// <summary>
    /// Script gérant les fonctions de dessin de debugs custom
    /// </summary>
    public static class DebugDraw
    {
        static List<DebugShape> shapes = new List<DebugShape>();
        static Material debugMaterial;

        static DebugDraw()
        {
            Camera.onPostRender += OnPostRender;

            Shader debugShader = Shader.Find("Hidden/DebugDrawSolid");
            if (debugShader != null) debugMaterial = new Material(debugShader);
        }

        /// <summary>
        /// Dessine une ligne de debug
        /// </summary>
        /// <param name="startPoint">La position du début de la ligne</param>
        /// <param name="endPoint">La position de fin de la ligne</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Line(Vector3 startPoint, Vector3 endPoint, Color color, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugLine(startPoint, endPoint, color, true, Time.time + time));
            }
        }

        /// <summary>
        /// Dessine un carré de debug
        /// </summary>
        /// <param name="center">La position du centre du carré</param>
        /// <param name="rotation">La rotation du carré</param>
        /// <param name="size">La taille du carré en x,y</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Square(Vector3 center, Quaternion rotation, Vector2 size, Color color, bool isWireframe = true, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugSquare(center, rotation, size, color, isWireframe, Time.time + time));
            }
        }

        /// <summary>
        /// Dessine un cube de debug
        /// </summary>
        /// <param name="center">La position du centre du cube</param>
        /// <param name="rotation">La rotation du cube</param>
        /// <param name="size">La taille du cube en x,y,z</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Cube(Vector3 center, Quaternion rotation, Vector3 size, Color color, bool isWireframe = true, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugCube(center, rotation, size, color, isWireframe, Time.time + time));
            }
        }

        /// <summary>
        /// Dessine un cercle de debug
        /// </summary>
        /// <param name="center">Le position du centre du cercle</param>
        /// <param name="rotation">>La rotation du cercle</param>
        /// <param name="radius">Le rayon du cercle</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="segments">Le nombre de segments pour dessiner le cercle. Doit être au moins de trois</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Circle(Vector3 center, Quaternion rotation, float radius, Color color, bool isWireframe = true, int segments = 16, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugCircle(center, rotation, radius, color, isWireframe, Time.time + time, segments));
            }
        }

        /// <summary>
        /// Dessine une sphere de debug
        /// </summary>
        /// <param name="center">Le position du centre de la sphere</param>
        /// <param name="rotation">La rotation de la sphere</param>
        /// <param name="radius">Le rayon de la sphere</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme de la sphere. Doit être plus grand que 4 et pair.</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Sphere(Vector3 center, Quaternion rotation, float radius, Color color, bool isWireframe = true, int segments = 16, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugSphere(center, rotation, radius, color, isWireframe, Time.time + time, segments));
            }
        }

        /// <summary>
        /// Dessine un cone en 2D
        /// </summary>
        /// <param name="basePosition">La position du départ du cone 2D</param>
        /// <param name="rotation">La rotation du cone 2D</param>
        /// <param name="height">La hauteur du cone 2D</param>
        /// <param name="baseLenght">La longeur de la base du cone 2D</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Cone2D(Vector3 basePosition, Quaternion rotation, float height, float baseLenght, Color color, bool isWireframe = true, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugCone2D(basePosition, rotation, height, baseLenght, color, isWireframe, Time.time + time));
            }
        }

        /// <summary>
        /// Dessine un cone 3D de debug
        /// </summary>
        /// <param name="center">La position du sommet du cone 3D</param>
        /// <param name="rotation">La rotation du cone 3D</param>
        /// <param name="height">La hauteur du cone</param>
        /// <param name="baseLenght">La longeur de la base du cone cone 3D</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme du cone 3D. Doit être plus grand que 3.</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Cone3D(Vector3 center, Quaternion rotation, float height, float baseLenght, Color color, bool isWireframe = true, int segments = 16, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugCone3D(center, rotation, height, baseLenght, color, isWireframe, Time.time + time, segments));
            }
        }

        /// <summary>
        /// Dessine ue capsule 2D de debug
        /// </summary>
        /// <param name="center">La position du centre de la cpasule 2D</param>
        /// <param name="rotation">La rotation de la cpasule 2D</param>
        /// <param name="size">La taille de la capsule 2D. Doit être plus grand que 0.</param>
        /// <param name="height">La hauteur de la capsule 2D. Doit être plus grand que 0.</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme du cone 3D. Doit être plus grand que 3.</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Capsule2D(Vector3 center, Quaternion rotation, float size, float height, Color color, bool isWireframe = true, int segments = 16, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugCapsule2D(center, rotation, size, height, color, isWireframe, Time.time + time, segments));
            }
        }

        /// <summary>
        /// Dessine ue capsule 3D de debug
        /// </summary>
        /// <param name="center">La position du centre de la cpasule 3D</param>
        /// <param name="rotation">La rotation de la cpasule 3D</param>
        /// <param name="size">La taille de la capsule 3D. Doit être plus grand que 0.</param>
        /// <param name="height">La hauteur de la capsule 3D. Doit être plus grand que 0.</param>
        /// <param name="color">La couleur du debug</param>
        /// <param name="isWireframe">Vrai, si l'on dessine en wireframe ou en plein</param>
        /// <param name="segments">Le nombre de segments (côtés) utilisés pour approximer la forme de la capsule 3D. Doit être plus grand que 4 et divisible par 4 avec un reste de 0 ou de 1.</param>
        /// <param name="time">Le temps avant l'expiration du debug</param>
        public static void Capsule3D(Vector3 center, Quaternion rotation, float size, float height, Color color, bool isWireframe = true, int segments = 16, float time = 0)
        {
            if (DebugSystem.IsGlobalDebugEnabled() && DebugSystem.IsLocalDebugEnabled())
            {
                shapes.Add(new DebugCapsule3D(center, rotation, height, size, color, isWireframe, Time.time + time, segments));
            }
        }

        static void OnPostRender(Camera camera)
        {
            if (camera == null || shapes.Count == 0 || debugMaterial == null) return;

            debugMaterial.SetPass(0);

            GL.PushMatrix();
            GL.LoadProjectionMatrix(camera.projectionMatrix);
            GL.modelview = camera.worldToCameraMatrix;

            shapes.ForEach(shape => shape.Draw());

            GL.PopMatrix();

            shapes.RemoveAll(shape => shape.expirationTime < Time.time);
        }
    }

    /// <summary>
    /// Class commune de debug de forme
    /// </summary>
    public abstract class DebugShape
    {
        public Color color;
        public float expirationTime;
        public bool wireframe;

        public DebugShape(Color color, float expirationTime, bool wireframe)
        {
            this.color = color;
            this.expirationTime = expirationTime;
            this.wireframe = wireframe;
        }

        public abstract void Draw();
    }
}
