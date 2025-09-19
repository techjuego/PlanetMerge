using UnityEngine;
using TechJuego.PlanetMerge.Utils;

namespace TechJuego.PlanetMerge
{
    // This class controls the behavior of the mergeable items (fruits) in the game
    public class MergeItem : MonoBehaviour
    {
        public int gameplayIndex;
        // Index representing the level of the item (e.g., fruit type or rank)
        public int itemIndex;

        // The state of the item (e.g., Ready, Dropping, Collision)
        public ItemState itemState;

        // The X-axis limit within which the item can move (screen boundaries)
        public float limit_x;

        [SerializeField]private GameObject m_DestorySymbol;

        public Rigidbody2D m_Rigidbody2D;
        

        // Called when the object is enabled
        private void OnEnable()
        {
            // Set the limit for X-axis movement based on the camera's size and the item's scale
            transform.localScale = Vector3.one * 0.5f;
            GameEvents.OnSelectBooster += GameEvents_OnSelectBooster;
            GameEvents.OnUseBooster += GameEvents_OnUseBooster;
        }
        private void GameEvents_OnUseBooster(Booster booster)
        {
            m_DestorySymbol.SetActive(false);
        }
        private void OnDisable()
        {
            GameEvents.OnSelectBooster -= GameEvents_OnSelectBooster;
            GameEvents.OnUseBooster -= GameEvents_OnUseBooster;
        }
        private void GameEvents_OnSelectBooster(Booster booster)
        {
            if(itemState != ItemState.Ready)
            {
                m_DestorySymbol.SetActive(true);
            }
        }
        // Called when the script is first initialized
        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            m_DestorySymbol.SetActive(false);
        }
    

        // Offset used to keep track of the mouse position while dragging the item
       public Vector3 offset;

        // Method to handle collision events
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // If the item collides with a wall or another mergeable item, change its state
            if (collision.gameObject.tag.Contains("Wall") || collision.gameObject.tag.Contains("MergeItem"))
            {
                itemState = ItemState.Collision;  // Change state to Collision when contact is made
            }
            // If the item is dropping and collides with another item, attempt to merge
            if ((int)itemState >= (int)ItemState.Dropping && collision.gameObject.tag.Contains("MergeItem"))
            {
                // Check if the items can merge based on itemIndex and if they are not the highest level
                if (itemIndex == collision.gameObject.GetComponent<MergeItem>().itemIndex)
                {
                    int NextIndex = itemIndex + 1;
                    if (NextIndex < GameManager.Instance.mergeItems.Count)
                    {
                        // Ensure only one item merges with another (prevents merging with self)
                        if (gameplayIndex >   collision.gameObject.GetComponent<MergeItem>().gameplayIndex)
                        {
                            // Calculate score for merging items based on itemIndex
                            var score = (NextIndex + 1) * 2;
                            GameManager.Instance.Score += (score * score);  // Update score
                            GameManager.Instance.CombineItem(gameObject.GetComponent<Transform>().position, collision.transform.position, NextIndex);  // Combine items into a new item
                            GameEvents.OnUpdateScore?.Invoke();  // Trigger score update event
                            // Destroy the merged items
                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
}
