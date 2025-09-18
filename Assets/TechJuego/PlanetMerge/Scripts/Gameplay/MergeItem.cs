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

        private Rigidbody2D m_Rigidbody2D;
        

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
        // Get the world position of the mouse pointer
        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        // Offset used to keep track of the mouse position while dragging the item
        Vector3 offset;
        private bool isDraging = false;

        // Called every frame to update item behavior
        void Update()
        {
                // Show highlight when the item is ready for interaction
                if (itemState == ItemState.Ready)
                {
                    GameEvents.OnMosueDown?.Invoke(transform.position);
                }
                else
                {
                    // m_Highlight.gameObject.SetActive(false);
                }

                // Prevent interaction if the pointer is over a UI element
                if (UiUtility.IsPointerOverUIObject())
                {
                    return;
                }

                // Ensure game logic runs only when the game state is 'InProgress'
                if (GameStateHandler.Instance.m_GameState != GameState.InProgress)
                {
                    return;
                }

                if (GameManager.Instance.m_BombSelected)
                    return;


                // Handle item dragging behavior when it is in the 'Ready' state
                if (GameStateHandler.Instance.m_GameState == GameState.InProgress && itemState == ItemState.Ready)
                {

                    if (Input.GetMouseButtonDown(0)) // On mouse down, start dragging
                    {
                        isDraging = true;
                        offset = transform.position - GetMouseWorldPosition();  // Store the offset from the mouse
                    }

                    if (Input.GetMouseButtonUp(0) && isDraging) // On mouse up, stop dragging and drop the item
                    {
                        isDraging = false;
                        m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        m_Rigidbody2D.gravityScale = 1f;  // Enable gravity
                        itemState = ItemState.Dropping;  // Change state to Dropping
                        GameManager.Instance.currentMergeItem = null;
                        GameManager.Instance.CreateItem();  // Create new fruit item
                        GameEvents.OnMosueUp?.Invoke();
                    }

                    // Update the item position if it is being dragged
                    if (isDraging)
                    {
                        if (itemState == ItemState.Ready)
                        {
                            // Update the position of the item to follow the mouse
                            Vector3 mousePos = GetMouseWorldPosition() + offset;
                            gameObject.transform.position = new Vector3(Mathf.Clamp(mousePos.x, -limit_x, limit_x),
                            gameObject.transform.position.y, gameObject.transform.position.z);
                        }
                    }
                }
        }

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
