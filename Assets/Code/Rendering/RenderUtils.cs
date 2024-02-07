using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
    public static class RenderUtils
    {
        private static Mesh _fullscreenQuad;

        public static Mesh FullscreenQuad
        {
            get
            {
                if (_fullscreenQuad == null)
                {

                    _fullscreenQuad = new Mesh();
                    _fullscreenQuad.SetVertices(new List<Vector3>
                    {
                        new(-1.0f, -1.0f, 0.0f),
                        new(-1.0f, 1.0f, 0.0f),
                        new(1.0f, 1.0f, 0.0f),
                        new(1.0f, -1.0f, 0.0f)
                    });
                    
                    _fullscreenQuad.SetUVs(0, new List<Vector2>
                    {
                        new(0.0f, 0.0f),
                        new(0.0f, 1.0f),
                        new(1.0f, 1.0f),
                        new(1.0f, 0.0f)
                    });
                    
                    _fullscreenQuad.SetIndices(new List<ushort> { 0, 1, 2, 2, 3, 0 }, MeshTopology.Triangles, 0, false);
                    _fullscreenQuad.UploadMeshData(true);
                }

                return _fullscreenQuad;
            }
        }
    }
}