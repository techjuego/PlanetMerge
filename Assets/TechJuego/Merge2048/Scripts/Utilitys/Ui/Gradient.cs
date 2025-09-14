using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace TechJuego.FruitSliceMerge
{
    public class Gradient : BaseMeshEffect
    {
        [SerializeField] public Color TopLeft = Color.white;
        [SerializeField] public Color TopRight = Color.white;
        [SerializeField] public Color BottomLeft = Color.black;
        [SerializeField] public Color BottomRight = Color.black;
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!this.IsActive()) return;
            List<UIVertex> vertexList = new List<UIVertex>();
            vh.GetUIVertexStream(vertexList);
            ModifyVertices(vertexList);
            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexList);
        }
        public void ModifyVertices(List<UIVertex> vertexList)
        {
            if (!IsActive() || vertexList.Count == 0)
                return;
            float minX = vertexList[0].position.x;
            float maxX = vertexList[0].position.x;
            float minY = vertexList[0].position.y;
            float maxY = vertexList[0].position.y;
            foreach (var v in vertexList)
            {
                var pos = v.position;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.x < minX) minX = pos.x;
                if (pos.y > maxY) maxY = pos.y;
                if (pos.y < minY) minY = pos.y;
            }
            float width = maxX - minX;
            float height = maxY - minY;
            for (int i = 0; i < vertexList.Count; i++)
            {
                UIVertex v = vertexList[i];
                float normalizedX = (v.position.x - minX) / width;
                float normalizedY = (v.position.y - minY) / height;
                Color leftColor = Color.Lerp(BottomLeft, TopLeft, normalizedY);
                Color rightColor = Color.Lerp(BottomRight, TopRight, normalizedY);
                Color finalColor = Color.Lerp(leftColor, rightColor, normalizedX);
                v.color = finalColor;
                vertexList[i] = v;
            }
        }
    }
}