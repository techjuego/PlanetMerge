using UnityEngine;

namespace TechJuego.PlanetMerge
{
    // This class controls the behavior of the top boundary (likely a game over zone) in the game
    public class Top : MonoBehaviour
    {
        // The cross line that appears when a collision happens (to indicate game over)
        public SpriteRenderer m_CrossLine;

        // A flag to check if the collision has occurred
        private bool isColliding = false;

        // A flag to check if an overflow situation is happening
        private bool isOverFlow = false;

        // Called when the object is enabled
        private void OnEnable()
        {
            // Reset the cross line color to white when enabled (indicating no collision)
            m_CrossLine.color = Color.white;
        }

        // Called when another collider enters this object's trigger zone
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (GameStateHandler.Instance.m_GameState == GameState.InProgress)
            {
                // Check if the colliding object is a "MergeItem"
                if (collision.gameObject.tag.Contains("MergeItem"))
                {
                        // If the "MergeItem" has collided (item state = Collision), trigger the game over process
                        if (collision.gameObject.GetComponent<MergeItem>().itemState == ItemState.Collision)
                        {
                            isColliding = true;
                            // Change the color of the cross line to red to indicate a collision
                            m_CrossLine.color = Color.red;
                            // Invoke the ShowGameOverView method after a short delay (1 second)
                            Invoke(nameof(ShowGameOverView), 1f);
                        }
                }
            }
        }

        // Called when the collider exits the trigger zone
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (GameStateHandler.Instance.m_GameState == GameState.InProgress)
            {
                // Check if the colliding object is a "MergeItem"
                if (collision.gameObject.tag.Contains("MergeItem"))
                {
                        // If the "MergeItem" has left the trigger zone and is in a collision state, reset the cross line
                        if (collision.gameObject.GetComponent<MergeItem>().itemState == ItemState.Collision)
                        {
                            // Reset the color of the cross line to white
                            m_CrossLine.color = Color.white;
                            isColliding = false;
                        }
                }
            }
        }

        // Method to show the game over view when a collision has occurred
        private void ShowGameOverView()
        {
            if (GameStateHandler.Instance.m_GameState == GameState.InProgress)
            {
                // If the collision is confirmed, change the game state to Game Over
                if (isColliding)
                {
                    GameStateHandler.Instance.m_GameState = GameState.GameOver;
                    // Disable the BoxCollider2D to prevent further triggering
                    GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
