using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace TechJuego.FruitSliceMerge
{
    public class LinerGradient : BaseMeshEffect
    {
        public enum GradientType
        {
            Horizontal,
            Vertical
        }
        public GradientType gradtype;
        [SerializeField]
        public Color
            FirstColor = Color.white;
        [SerializeField]
        public Color
            SecondColor = Color.black;
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!this.IsActive())
                return;
            List<UIVertex> vertexList = new List<UIVertex>();
            vh.GetUIVertexStream(vertexList);

            ModifyVertices(vertexList);

            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexList);
        }
        float bottomY;
        float topY;
        float y;
        public void ModifyVertices(List<UIVertex> vertexList)
        {
            if (!IsActive())
            {
                return;
            }

            int count = vertexList.Count;
            if (count > 0)
            {
                switch (gradtype)
                {
                    case GradientType.Horizontal:
                        bottomY = vertexList[0].position.x;
                        topY = vertexList[0].position.x;
                        break;
                    case GradientType.Vertical:
                        bottomY = vertexList[0].position.y;
                        topY = vertexList[0].position.y;
                        break;
                }

            }

            for (int i = 1; i < count; i++)
            {
                switch (gradtype)
                {
                    case GradientType.Horizontal:
                        y = vertexList[i].position.x;
                        break;
                    case GradientType.Vertical:
                        y = vertexList[i].position.x;
                        break;
                }
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }
            }
            float uiElementHeight = topY - bottomY;
            for (int i = 0; i < count; i++)
            {
                UIVertex uiVertex = vertexList[i];
                UIVertex vivertex2 = vertexList[i];
                switch (gradtype)
                {
                    case GradientType.Horizontal:
                        uiVertex.color = Color32.Lerp(SecondColor, FirstColor, (uiVertex.position.x - (bottomY - (uiElementHeight * dividevalue))) / uiElementHeight);
                        //uiVertex.color = Color32.Lerp (SecondColor, FirstColor, (vivertex2.position.y - (bottomY - (uiElementHeight * dividevalue)) ) / uiElementHeight);
                        //	if (i < count / 2) {
                        vertexList[i] = uiVertex;
                        //	} else {
                        //		vertexList [i] = vivertex2;
                        //	}
                        break;
                    case GradientType.Vertical:
                        uiVertex.color = Color32.Lerp(SecondColor, FirstColor, (uiVertex.position.y - (bottomY - (uiElementHeight * dividevalue))) / uiElementHeight);

                        vertexList[i] = uiVertex;

                        break;
                }
            }
        }
        [Range(-1, 1)]
        public float dividevalue;
    }
}