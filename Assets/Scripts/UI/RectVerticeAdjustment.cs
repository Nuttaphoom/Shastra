using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class RectVerticeAdjustment : Image
    {
        [SerializeField]
        public float dd = 0;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);

            for (int i = 0; i < vh.currentVertCount; i++)
            {
                UIVertex vert = UIVertex.simpleVert;
                vh.PopulateUIVertex(ref vert, i);
                Vector3 position = vert.position;

                switch (i % 4) // Assume vertices are in quads (4 vertices per quad)
                {
                    case 0: // Top-left vertex
                        position.x -=  0f;
                        position.y += 15f;
                        break;
                    case 1: // Top-right vertex
                        position.x += 0f;
                        position.y -= 10f;
                        break;
                    case 2: // Bottom-right vertex
                        position.x += 0f;
                        position.y -= 2f;
                        break;
                        //case 3: // Bottom-left vertex
                        //    position.x -= 5f;
                        //    position.y -= 7f;
                        //    break;
                }

                vert.position = position;
                vh.SetUIVertex(vert, i);
            }
        }
    }
}
