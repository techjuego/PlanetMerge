using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class CameraCollider : MonoBehaviour
    {
        public BoxCollider2D m_RightCollider;
        public BoxCollider2D m_LeftCollider;

        public BoxCollider2D m_BottomCollider;
        public Transform m_Bottom;

        public BoxCollider2D m_TopCollider;
        public Transform m_Top;

        public Transform m_EndCollider;
        public Transform m_SpawnPos;
        private void OnEnable()
        {
            SetCollider();
        }
        private void OnValidate()
        {
            SetCollider();
        }
        void SetCollider()
        {
            float width = Camera.main.orthographicSize * Camera.main.aspect;

            m_RightCollider.transform.position = new Vector3(width, 0, 0);
            m_RightCollider.size = new Vector2(2, Camera.main.orthographicSize * 2 + 2);
            m_RightCollider.offset = new Vector2(m_RightCollider.size.x / 2, 0);

            m_LeftCollider.transform.position = new Vector3(-width, 0, 0);
            m_LeftCollider.size = new Vector2(2, Camera.main.orthographicSize * 2 + 2);
            m_LeftCollider.offset = new Vector2(-m_RightCollider.size.x / 2, 0);

            m_BottomCollider.transform.position = new Vector3(0, -Camera.main.orthographicSize + (5 / Camera.main.orthographicSize), 0);
            m_BottomCollider.size = new Vector2(width * 2, 2);
            m_BottomCollider.offset = new Vector2(0, -m_RightCollider.size.x / 2);
            m_Bottom.transform.localScale = new Vector3(width * 2, 0.1f, 1);

            m_TopCollider.transform.position = new Vector3(0, Camera.main.orthographicSize - (11 / Camera.main.orthographicSize), 0);
            m_TopCollider.size = new Vector2(width * 2, 2);
            m_TopCollider.offset = new Vector2(0, m_RightCollider.size.x / 2);
            m_Top.transform.localScale = new Vector3(width * 2, 0.1f, 1);
            m_SpawnPos.transform.position = m_TopCollider.transform.position - new Vector3(0, 1, 0);
            m_EndCollider.transform.position = m_TopCollider.transform.position - new Vector3(0, 2, 0);
            //   m_EndCollider.transform.localScale = new Vector3(width * 2, 0.1f, 1);
        }
    }
}