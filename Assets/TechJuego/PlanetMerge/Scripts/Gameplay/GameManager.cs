using UnityEngine;
using TechJuego.PlanetMerge.Sound;
using System.Collections.Generic;
//using TechJuego.PlanetMerge.HapticFeedback;
using TechJuego.PlanetMerge.Monetization;
namespace TechJuego.PlanetMerge
{
    // The GameManager class handles game mechanics such as spawning items, combining them, and tracking game state.
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }
        private static GameManager _instance;
        public GameManager()
        {
            _instance = this;
        }

        public List<MergeItem> mergeItems = new List<MergeItem>();

        // Position where the new items are spawned
        public GameObject BornPos;

        public bool m_BombSelected;

        // Effect to be played when items merge
        public GameObject m_MergeEffect;

        // Flag to check if the game is over
        public bool isGameOver;
        // Particle system for merging effects
        public ParticleSystem m_MergeParticle;

        // Reference to the MergeItem component to track item state
        public MergeItem m_MergItem;

        [SerializeField] private GameObject m_BombImage;

        [SerializeField] private GameObject m_BombEffect;

        public MergeItem currentMergeItem;

        // Current score of the game
        private int m_Score = 0;
        private int lastTriggerScore = 0;
        public int Score
        {
            get { return m_Score; }
            set
            {
                m_Score = value;
                GameEvents.OnUpdateScore?.Invoke();

                if (m_Score - lastTriggerScore >= 500)
                {
                    lastTriggerScore = m_Score - (m_Score % 500); // store the last multiple of 3000
                    GameDistribution.Instance.ShowAd();
                    GameDistribution.Instance.SendEvent("Ads");
                    //AdsHandler.Instance.ShowInterstitial();
                }
            }
        }
        private void Awake()
        {
            GameplayDataHolder gameplayDataHolder = Resources.Load("Gameplay/GameplayDataHolder") as GameplayDataHolder;
            mergeItems = gameplayDataHolder.DeepCopy().mergeItems;
            for (int i = 0; i < mergeItems.Count; i++)
            {
                mergeItems[i].itemIndex = i;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            rangeSize = 1;
            // Play background music loop when the game starts
            SoundEvents.OnPlayLoopSound?.Invoke("BGMUSIC");

            // Set the game state to in progress at the start
            GameStateHandler.Instance.m_GameState = GameState.InProgress;

            // Spawn the first item when the game starts
            CreateNewItem();
        }

        public int rangeSize;
        public int MaxRangeSize;
        public int spawnedItemIndex;
        private int GetIndex()
        {
            spawnedItemIndex++;
            return spawnedItemIndex;
        }
        int GetWeightedRandomIndex(int size)
        {
            if (size <= 0) return 0;

            // Linear probability: index 0 has highest, last index lowest
            float totalWeight = 0f;
            float[] weights = new float[size];

            for (int i = 0; i < size; i++)
            {
                weights[i] = size - i;  // Example: size=5 -> [5,4,3,2,1]
                totalWeight += weights[i];
            }

            float rand = Random.value * totalWeight;
            float cumulative = 0f;

            for (int i = 0; i < size; i++)
            {
                cumulative += weights[i];
                if (rand < cumulative)
                    return i;
            }

            return size - 1; // fallback
        }
        // Method to create a new item with a 1-second delay, used for spawning items repeatedly
        public void CreateItem()
        {
            Invoke(nameof(CreateNewItem), 1f);  // Invoke CreateNewItem after 1 second
        }
        // Method to create a new item
        public void CreateNewItem()
        {
            // Randomly choose an item from the mergeItems array
            if (rangeSize >= MaxRangeSize)
            {
                rangeSize = MaxRangeSize;
            }
            int num = GetWeightedRandomIndex(rangeSize);
            if (mergeItems != null)
            {
                MergeItem item = mergeItems[num];
                if (item != null)
                {
                    // Instantiate the chosen item at the spawn position
                    var curItem = Instantiate(item, BornPos.transform.position, item.transform.rotation);
                    curItem.gameplayIndex = GetIndex();
                    currentMergeItem = curItem;
                    // Set the item state to 'Ready' for interaction
                    curItem.itemState = ItemState.Ready;
                }
            }
        }
        // Method to combine two items and create a higher-level one at the midpoint of their positions
        public void CombineItem(Vector3 currentPos, Vector3 collisionPos, int itemIndex)
        {
            int newrange = itemIndex +1;
            if(newrange > rangeSize)
            {
                rangeSize = newrange;
            }

            // Calculate the midpoint of the two item positions
            Vector3 newPos = (currentPos + collisionPos) / 2;
            // Get the new item to be created based on the itemIndex
            MergeItem newItem = mergeItems[itemIndex];

            // Instantiate the new item at the calculated position
            var combineItem = Instantiate(newItem, newPos, newItem.transform.rotation);
            combineItem.gameplayIndex = GetIndex();

            // Set the new item state to 'Collision' indicating it was just merged
            combineItem.itemState = ItemState.Collision;

            // Enable gravity on the newly created item
            combineItem.GetComponent<Rigidbody2D>().gravityScale = 1f;

            combineItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            // Play the merging sound effect
            SoundEvents.OnPlaySingleShotSound?.Invoke("Merge");

            // Set the particle system color to match the merged item
            var particl = m_MergeParticle.main;

            // Instantiate the merge effect at the new position
            var effect = Instantiate(m_MergeEffect);
            effect.transform.position = newPos;
            //if (HapticSetting.GetVibrate())
            //{
            //    HapticCall.Instance.MediumHaptic();
            //}
        }
    
        private MergeItem destroyItem;
        private void Update()
        {
            if (GameStateHandler.Instance.m_GameState == GameState.InProgress)
            {
                if (m_BombSelected)
                {
                    Vector2 mousePos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    m_BombImage.transform.position = new Vector3(mousePos1.x, mousePos1.y, 0);

                    if (Input.GetMouseButtonDown(0)) // Left mouse click
                    {
                        // Convert mouse position (screen space) to world position
                        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        // Perform raycast at that position
                        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                        if (hit.collider != null)
                        {
                            Debug.Log("Clicked on: " + hit.collider.name);

                            if (hit.transform.TryGetComponent(out MergeItem mergeItem))
                            {
                                if (mergeItem.itemState != ItemState.Ready)
                                {
                                    destroyItem = mergeItem;
                                }
                            }
                        }
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (destroyItem != null)
                        {
                            DataHandler.Instance.BombCount--;
                            if (DataHandler.Instance.BombCount <= 0)
                            {
                                DataHandler.Instance.BombCount = 0;
                            }
                            m_BombSelected = false;
                            GameEvents.OnUseBooster?.Invoke(Booster.Bomb);
                            ShowBombEffect(destroyItem.transform.position);
                            Destroy(destroyItem.gameObject);
                            m_BombImage.SetActive(false);

                            //if (HapticSetting.GetVibrate())
                            //{
                            //    HapticCall.Instance.HeavyHaptic();
                            //}
                        }
                    }
                }
            }
        }
        private void OnEnable()
        {
            GameEvents.OnSelectBooster += GameEvents_OnSelectBooster;
        }
        private void OnDisable()
        {
            GameEvents.OnSelectBooster -= GameEvents_OnSelectBooster;
        }
        private void GameEvents_OnSelectBooster(Booster booster)
        {
            m_BombImage.SetActive(true);
        }
        private GameObject DestroyEffect;
        public void ShowBombEffect(Vector3 pos) 
        {
            if (DestroyEffect == null)
            {
                DestroyEffect = Instantiate(m_BombEffect, pos, Quaternion.identity);
            }
            else
            {
                DestroyEffect.transform.position = pos;
                DestroyEffect.SetActive(true);
            }
        }
    }
}