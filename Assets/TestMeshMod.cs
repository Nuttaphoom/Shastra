using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class TestMeshMod : BaseMeshEffect
    {

        private List<Image> slotList = new List<Image>();

        private float mintop = -5.0f;
        private float maxtop = 10.0f;

        [HideInInspector]
        public int SlotNO ;

        public void SetSLOTNO(int i)
        {
            SlotNO = i; 
        }
        public override void ModifyMesh(VertexHelper vh)
        {
            
            if (!IsActive()) return;

            float splitRange = (maxtop - mintop) / slotList.Count;
            int vertCount = vh.currentVertCount;

            var vert = new UIVertex();

            //foreach (Image slot in slotList)
            //{
            //    slot.GetComponent<Graphic>();
            //    slot.SetVerticesDirty();
            //}

            if(SlotNO == 0)
            {
                vh.PopulateUIVertex(ref vert, 0);
                vert.position.x += 10;
                vh.SetUIVertex(vert, 0);

                vh.PopulateUIVertex(ref vert, 3);
                vert.position.x -= 5 * ((SlotNO + 1) / 2);
                vh.SetUIVertex(vert, 3);
            }
            if (SlotNO == 1)
            {
                vh.PopulateUIVertex(ref vert, 0);
                //vert.position.x += 10;
                vh.SetUIVertex(vert, 0);

                vh.PopulateUIVertex(ref vert, 3);
                vert.position.x -= 5 * ((SlotNO + 1) / 2);
                vh.SetUIVertex(vert, 3);
            }
            if (SlotNO == 2)
            {
                vh.PopulateUIVertex(ref vert, 0);
                vert.position.x -= 5;
                vh.SetUIVertex(vert, 0);
                vh.PopulateUIVertex(ref vert, 3);
                vert.position.x -= 5 * ((SlotNO + 1) / 2);
                vh.SetUIVertex(vert, 3);
            }





            //for (int v = 0; v < vertCount; v++)
            //{
            //    vh.PopulateUIVertex(ref vert, v);

            //    vert.position.x += 50;// (Random.value - 0.5f) * 10;
            //    //vert.position.y += (Random.value - 0.5f) * 10;

            //    vh.SetUIVertex(vert, v);
            //}
        }

        private void Update()
        {
            var graphic = GetComponent<Graphic>();
            graphic.SetVerticesDirty();
        }
    }
}
