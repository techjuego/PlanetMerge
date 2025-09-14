using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TechJuego.FruitSliceMerge
{
    public enum ChangeEffect
    {
        Fade,
        LeftToRightFill,
        RightToLeftFill,
        TopToBottomFill,
        BottomToTopFill,
        LeftFill,
        RightFill,
        TopFill,
        BottomFill,
    }
    public class GameColors
    {
        public static Color Color1()
        {
            return Color.black;
        }
    }
    public class SceneLoader : MonoBehaviour
    {
        public float fadeSpeed;
        public Color fadeColor;
        public string sceneToLoad;
        public ChangeEffect sceneChangeEffect;

        private Image background;
        private CanvasGroup canvasGroup;
        private static bool isFading;
        private float alpha;
        private bool isFadeIn;
        private bool hasStartedLoading;
        private static Canvas canvas;

        void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            isFadeIn = true;
            switch (sceneChangeEffect)
            {
                case ChangeEffect.Fade:
                    StartCoroutine(FadeCoroutine());
                    break;
                case ChangeEffect.LeftToRightFill:
                    background.fillOrigin = 1;
                    StartCoroutine(AfterLoadFillScene());
                    break;
                case ChangeEffect.RightToLeftFill:
                    background.fillOrigin = 0;
                    StartCoroutine(AfterLoadFillScene());
                    break;
                case ChangeEffect.TopToBottomFill:
                    background.fillOrigin = 0;
                    StartCoroutine(AfterLoadFillScene());
                    break;
                case ChangeEffect.BottomToTopFill:
                    background.fillOrigin = 1;
                    StartCoroutine(AfterLoadFillScene());
                    break;
                case ChangeEffect.LeftFill:
                case ChangeEffect.RightFill:
                case ChangeEffect.TopFill:
                case ChangeEffect.BottomFill:
                    StartCoroutine(AfterLoadFillScene());
                    break;
            }

        }

        public static void RestartScene() => LoadScene(SceneManager.GetActiveScene().name);

        public static void RestartScene(Color color, float speed, ChangeEffect effect) =>
            LoadScene(SceneManager.GetActiveScene().name, color, speed, effect);

        public static void LoadScene(string name) => SceneManager.LoadScene(name);

        public static void LoadScene(string scene, Color color, float speed, ChangeEffect effect)
        {
            if (isFading) { Debug.Log("Already Fading"); return; }

            var faderObject = new GameObject("SceneFader");
            var fader = faderObject.AddComponent<SceneLoader>();
            canvas = faderObject.AddComponent<Canvas>();

            faderObject.AddComponent<CanvasGroup>();
            var img = faderObject.AddComponent<Image>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            fader.fadeSpeed = speed;
            fader.sceneToLoad = scene;
            fader.fadeColor = color;
            fader.sceneChangeEffect = effect;
            isFading = true;
            img.type = Image.Type.Filled;
            img.fillAmount = 0f;
            img.color = color;
            img.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            img.sprite = Resources.Load<Sprite>("FillSquare");
            switch (effect)
            {
                case ChangeEffect.Fade:
                    break;
                case ChangeEffect.LeftToRightFill:
                case ChangeEffect.LeftFill:
                    img.fillMethod = Image.FillMethod.Horizontal;
                    img.fillOrigin = 0;
                    break;
                case ChangeEffect.RightToLeftFill:
                case ChangeEffect.RightFill:
                    img.fillMethod = Image.FillMethod.Horizontal;
                    img.fillOrigin = 1;
                    break;
                case ChangeEffect.TopToBottomFill:
                case ChangeEffect.TopFill:
                    img.fillMethod = Image.FillMethod.Vertical;
                    img.fillOrigin = 1;
                    break;
                case ChangeEffect.BottomToTopFill:
                case ChangeEffect.BottomFill:
                    img.fillMethod = Image.FillMethod.Vertical;
                    img.fillOrigin = 0;
                    break;
            }
            fader.InitializeFader();
        }
        private void InitializeFader()
        {
            DontDestroyOnLoad(gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
            background = GetComponent<Image>();
            canvas.sortingOrder = 999;
            if (background) background.color = fadeColor;
            switch (sceneChangeEffect)
            {
                case ChangeEffect.Fade:
                    StartCoroutine(FadeCoroutine());
                    break;
                case ChangeEffect.LeftToRightFill:
                case ChangeEffect.RightToLeftFill:
                case ChangeEffect.TopToBottomFill:
                case ChangeEffect.BottomToTopFill:
                case ChangeEffect.LeftFill:
                case ChangeEffect.RightFill:
                case ChangeEffect.TopFill:
                case ChangeEffect.BottomFill:
                    StartCoroutine(LoadFillScene());
                    break;
            }

        }

        private IEnumerator FadeCoroutine()
        {
            float startTime = Time.time;
            while (true)
            {
                alpha = Mathf.Clamp01((Time.time - startTime) * fadeSpeed * (isFadeIn ? -1 : 1) + (isFadeIn ? 1 : 0));
                canvasGroup.alpha = alpha;

                if (!isFadeIn && alpha == 1 && !hasStartedLoading)
                {
                    hasStartedLoading = true;
                    SceneManager.LoadScene(sceneToLoad);
                }
                else if (isFadeIn && alpha == 0) break;
                yield return null;
            }
            CompleteFade();
        }

        private IEnumerator LoadFillScene()
        {
            AsyncOperation scene = SceneManager.LoadSceneAsync(sceneToLoad);
            scene.allowSceneActivation = false;
            float startTime = Time.time;
            while (!scene.isDone)
            {
                if (scene.progress >= 0.9f)
                {
                    alpha = Mathf.Clamp01((Time.time - startTime) * fadeSpeed);
                    background.fillAmount = alpha;
                    if (alpha >= 1) scene.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        private IEnumerator AfterLoadFillScene()
        {
            float startTime = Time.time;

            while ((alpha = Mathf.Clamp01(1 - (Time.time - startTime) * fadeSpeed)) > 0)
            {
                background.fillAmount = alpha;
                yield return null;
            }
            CompleteFade();
        }

        private static void CompleteFade()
        {
            isFading = false;
            Destroy(GameObject.Find("SceneFader"));
        }
    }
}