using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TechJuego.PlanetMerge.Utils
{
    public class TweenEvent
    {
        public delegate void OnAction(GameObject gameObject);
        public static OnAction CancleTween;
    }
    public static class CoroutineExtensions
    {
        public static Coroutine RunCoroutine(this MonoBehaviour monoBehaviour, IEnumerator routine)
        {
            return monoBehaviour.StartCoroutine(routine);
        }
    }
    public enum OperationType
    {
        None,
        Tween,
        CallInSec,
    }
    public enum TweenType
    {
        Delay,
        Value,
        Move,
        MoveLocal,
        Scale,
        Rotate,
        RotateLocal,
        KeepRotate,
        KeepRotateLocal,
        TrigRotate,
        TrigScale,
        TrigMove,
        TrigSinValue,
        CanvasGroupAlpha,
        SpriteRendererAlpha
    }
    public enum EaseTween
    {
        Linear,
        EaseInQuad, EaseOutQuad, EaseInOutQuad, EaseOutInQuad,
        EaseInCubic, EaseOutCubic, EaseInOutCubic, EaseOutInCubic,
        EaseInQuart, EaseOutQuart, EaseInOutQuart, EaseOutInQuart,
        EaseInQuint, EaseOutQuint, EaseInOutQuint, EaseOutInQuint,
        EaseInSine, EaseOutSine, EaseInOutSine, EaseOutInSine,
        EaseInExpo, EaseOutExpo, EaseInOutExpo, EaseOutInExpo,
        EaseInCirc, EaseOutCirc, EaseInOutCirc, EaseOutInCirc,
        EaseInBounce, EaseOutBounce, EaseInOutBounce, EaseOutInBounce,
        EaseInBack, EaseOutBack, EaseInOutBack, EaseOutInBack,
        EaseInElastic, EaseOutElastic, EaseInOutElastic, EaseOutInElastic,
        EaseSpring
    }
    public class TweenUpdate
    {
        public Action OnTweenStart;
        public Action OnTweenComplete;
        public Action<float> onUpdateValue;
        public Action<int> onUpdateIntValue;
        public Action<Vector2> onUpdateVector2;
        public Action<Vector3> onUpdateVector3;
        public void Reset()
        {
            OnTweenStart = null;
            onUpdateValue = null;
            onUpdateVector2 = null;
            onUpdateVector3 = null;
            OnTweenComplete = null;
            onUpdateIntValue = null;
        }
    }

    public class TechTween : MonoBehaviour
    {
        private void OnEnable()
        {
            TweenEvent.CancleTween += TweenEvent_CancleTween;
        }
        private void TweenEvent_CancleTween(GameObject gameObject)
        {
            TechTween[] techTweens = gameObject.transform.GetComponentsInChildren<TechTween>();
            for (int i = 0; i < techTweens.Length; i++)
            {
                Destroy(techTweens[i]);
            }
        }
        private void OnDisable()
        {
            TweenEvent.CancleTween -= TweenEvent_CancleTween;
        }
        class UpdateLayout
        {
            public UpdateLayout() { }
            public HorizontalLayoutGroup horizontalLayout;
            public VerticalLayoutGroup verticalLayout;
            public IEnumerator RunTween()
            {
                yield return new WaitForEndOfFrame();
                if (horizontalLayout != null)
                {
                    horizontalLayout.spacing += 0.1f;
                    horizontalLayout.spacing -= 0.1f;
                }
                if (verticalLayout != null)
                {
                    verticalLayout.spacing += 0.1f;
                    verticalLayout.spacing -= 0.1f;
                }
            }
        }
        public static void UpdateLayputGroup(MonoBehaviour mono, HorizontalLayoutGroup hgroup)
        {
            UpdateLayout tween = new UpdateLayout();
            tween.horizontalLayout = hgroup;
            mono.RunCoroutine(tween.RunTween());
        }
        public static void UpdateLayputGroup(MonoBehaviour mono, VerticalLayoutGroup hgroup)
        {
            UpdateLayout tween = new UpdateLayout();
            tween.verticalLayout = hgroup;
            mono.RunCoroutine(tween.RunTween());
        }
        class FrameEnd
        {
            public Action OnComplete;
            public FrameEnd() { }
            public IEnumerator RunTween()
            {
                yield return new WaitForEndOfFrame();
                OnComplete?.Invoke();
            }
        }
        public static void CallAfterFrameEnd(MonoBehaviour mono, Action OnComplete)
        {
            FrameEnd tween = new FrameEnd();
            tween.OnComplete = OnComplete;
            mono.RunCoroutine(tween.RunTween());
        }
        public class DelayDetail
        {
            public float time;
            public Action OnComplete;
            public DelayDetail() { }
            public IEnumerator RunTween()
            {
                yield return new WaitForSeconds(time);
                OnComplete?.Invoke();
            }
        }
        [HideInInspector]
        public TweenDetail tweenDetail;
        private void Update()
        {
            switch (tweenDetail.tweenType)
            {
                case TweenType.CanvasGroupAlpha:
                case TweenType.SpriteRendererAlpha:
                case TweenType.Value:
                case TweenType.Move:
                case TweenType.MoveLocal:
                case TweenType.Scale:
                case TweenType.Rotate:
                case TweenType.Delay:
                    tweenDetail.UpdateTween();
                    break;
                case TweenType.KeepRotate:
                case TweenType.KeepRotateLocal:
                    tweenDetail.ContinueUpdateTween();
                    break;
                case TweenType.TrigRotate:
                case TweenType.TrigScale:
                case TweenType.TrigMove:
                case TweenType.TrigSinValue:
                    tweenDetail.UpdateTrignometric();
                    break;
            }
        }
        public static void SetAngleY(GameObject gameObject, float angle)
        {
            gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.eulerAngles.x, angle, gameObject.transform.eulerAngles.z);
        }
        public static void SetAngleZ(GameObject gameObject, float angle)
        {
            gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle);
        }
        public static void CancleTween(GameObject gameObject)
        {
            TechTween[] techTweens = gameObject.GetComponentsInChildren<TechTween>();
            for (int i = 0; i < techTweens.Length; i++)
            {
                Destroy(techTweens[i]);
            }
        }
        public static TweenDetail CallInSec(GameObject gameObject, int count, Action onComplete)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.repeat = count;
            tween.action = onComplete;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartAction();
            return tween;
        }
        public static void DelayCall(GameObject gameObject, float time, Action OnComplete)
        {
            TweenDetail tween = new TweenDetail();
            tween.time = time;
            tween.action = OnComplete;
            tween.tweenType = TweenType.Delay;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
        }
        public static TweenDetail MoveToArc(GameObject gameObject, Vector3 startPoint, Vector3 endPoint, Vector3 arcHeight, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = startPoint;
            tween.to = endPoint;
            tween.arcHeight = arcHeight;
            tween.time = time;
            tween.delay = delay;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartJump();
            return tween;
        }
        public static TweenDetail MoveToArc(RectTransform rect, Vector3 endPoint, Vector3 arcHeight, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.rectTrans = rect;
            tween.from = rect.anchoredPosition;
            tween.to = endPoint;
            tween.arcHeight = arcHeight;
            tween.time = time;
            tween.delay = delay;
            tween.techTween = rect.gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartJump();
            return tween;
        }
        public static TweenDetail MoveTo(GameObject gameObject, Vector3 to, float time, AnimationCurve animationCurve, bool isLocal = false, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = isLocal ? gameObject.transform.localPosition : gameObject.transform.position;
            tween.to = to;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = isLocal ? TweenType.MoveLocal : TweenType.Move;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail MoveTo(GameObject gameObject, Vector3 to, float time, bool isLocal = false, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = isLocal ? gameObject.transform.localPosition : gameObject.transform.position;
            tween.to = to;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = isLocal ? TweenType.MoveLocal : TweenType.Move;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail MoveTo(RectTransform rect, Vector3 to, float time, bool isLocal = false, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.rectTrans = rect;
            tween.from = isLocal ? rect.localPosition : rect.position;
            tween.to = to;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = isLocal ? TweenType.MoveLocal : TweenType.Move;
            tween.techTween = rect.gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail Rotate(GameObject gameObject, Vector3 axis, float speed, bool isLocal = false, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.axis = axis;
            tween.speed = speed;
            tween.delay = delay;
            tween.tweenType = isLocal ? TweenType.KeepRotateLocal : TweenType.KeepRotate;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }

        public static TweenDetail TrignometricRotate(GameObject gameObject, Vector3 angleLimit, Vector3 frequency)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = gameObject.transform.eulerAngles;
            tween.to = angleLimit;

            tween.frequency = frequency;
            tween.tweenType = TweenType.TrigRotate;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTrignometroc();
            return tween;
        }
        public static TweenDetail TrignometricScale(GameObject gameObject, Vector3 scaleLimit, Vector3 frequency)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = gameObject.transform.eulerAngles;
            tween.to = scaleLimit;
            tween.frequency = frequency;
            tween.tweenType = TweenType.TrigScale;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTrignometroc();
            return tween;
        }
        public static TweenDetail TrignometricMove(GameObject gameObject, Vector3 moveLimit, Vector3 frequency)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = gameObject.transform.eulerAngles;
            tween.to = moveLimit;
            tween.frequency = frequency;
            tween.tweenType = TweenType.TrigMove;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTrignometroc();
            return tween;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="frequency"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static TweenDetail TrignometricValue(GameObject gameObject, float start, float end, float frequency, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = new Vector3(start, 0, 0);
            tween.to = new Vector3(end, 0, 0);
            tween.frequency = new Vector3(frequency, 0, 0);
            tween.tweenType = TweenType.TrigSinValue;
            tween.delay = delay;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTrignometroc();
            return tween;
        }
        public static TweenDetail RotateTo(GameObject gameObject, Vector3 to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = gameObject.transform.eulerAngles;
            tween.to = to;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Rotate;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail ScaleTo(GameObject gameObject, Vector3 to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = gameObject.transform.localScale;
            tween.to = to;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Scale;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail ScaleFrom(GameObject gameObject, Vector3 from, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = from;
            tween.to = gameObject.transform.localScale;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Scale;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail ValueTo(GameObject gameObject, float start, float to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = new Vector3(start, 0, 0);
            tween.to = new Vector3(to, 0, 0);
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Value;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail ValueTo(GameObject gameObject, int start, int to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = new Vector3(start, 0, 0);
            tween.to = new Vector3(to, 0, 0);
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Value;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }

        public static TweenDetail ValueTo(MonoBehaviour mono, Vector2 start, Vector2 to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = mono.transform;
            tween.from = start;
            tween.to = to;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Value;
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail ValueTo(MonoBehaviour mono, Vector3 start, Vector3 to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = mono.transform;
            tween.from = start;
            tween.to = to;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Value;
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static void SetPosition(RectTransform rect, Vector2 position)
        {
            rect.anchoredPosition = position;
        }
        public static TweenDetail MoveFrom(RectTransform rect, Vector3 from, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.rectTrans = rect;
            tween.from = from;
            tween.to = rect.position;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Move;
            tween.techTween = rect.gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail MoveFrom(GameObject gameObject, Vector3 from, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.trans = gameObject.transform;
            tween.from = from;
            tween.to = gameObject.transform.position;
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.Move;
            tween.techTween = gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail CanvasAlpha(CanvasGroup group, float to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.canvasGroup = group;
            tween.from = new Vector3(group.alpha, 0, 0);
            tween.to = new Vector3(to, 0, 0);
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.CanvasGroupAlpha;
            tween.techTween = group.gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }
        public static TweenDetail SpriteRendererAlpha(SpriteRenderer sprite, float to, float time, float delay = 0)
        {
            TweenDetail tween = new TweenDetail();
            tween.reset();
            tween.spriteRenderer = sprite;
            tween.from = new Vector3(sprite.color.a, 0, 0);
            tween.to = new Vector3(to, 0, 0);
            tween.time = time;
            tween.delay = delay;
            tween.tweenType = TweenType.SpriteRendererAlpha;
            tween.techTween = sprite.gameObject.AddComponent<TechTween>();
            tween.techTween.tweenDetail = tween;
            tween.StartTween();
            return tween;
        }


    }
    [Serializable]
    public class TweenDetail
    {
        public bool isLocal = false;
        public bool isJumping = false;
        public bool isLooping = false;
        public bool isRunning = false;
        public bool isPingPong = false;
        public bool isReversing = false;

        public float time;
        public float delay;
        public float speed;
        public float startTime;

        public int repeat = 1;
        public int loopCount;

        public Vector3 to;
        public Vector3 from;
        public Vector3 axis;
        public Vector3 newVect;
        public Vector3 frequency;
        public Vector3 arcHeight;

        public Transform trans;

        public TweenType tweenType;
        public EaseTween easeTween = EaseTween.Linear;
        public OperationType operationType = OperationType.None;

        public Action action;
        public TechTween techTween;
        public SpriteRenderer spriteRenderer;
        public CanvasGroup canvasGroup;
        public RectTransform rectTrans;
        public AnimationCurve animationCurve;
        public TweenUpdate tweenUpdates = new TweenUpdate();

        public TweenDetail() { }
        public void CancleTween()
        {
            GameObject.Destroy(techTween);
        }
        public void reset()
        {
            isLooping = false;
            repeat = 1;
            trans = null;
            delay = 0.0f;
            from = to = Vector3.zero;
            isPingPong = false;
            isReversing = false;
            tweenUpdates.Reset();
        }
        public void StartAction()
        {
            time = 0f;
            operationType = OperationType.CallInSec;
        }
        public void StartJump()
        {
            startTime = Time.time + delay;
            loopCount = 0;
            tweenUpdates.OnTweenStart?.Invoke();
            isJumping = true;
        }
        public void StartTween()
        {
            startTime = Time.time + delay;
            loopCount = 0;
            tweenUpdates.OnTweenStart?.Invoke();
            isRunning = true;
        }
        public void ContinueUpdateTween()
        {
            if (!isRunning) return;
            switch (tweenType)
            {
                case TweenType.KeepRotate:
                    if (trans != null)
                    {
                        trans.Rotate(axis, speed * Time.deltaTime, Space.World);
                    }
                    break;
                case TweenType.KeepRotateLocal:
                    if (trans != null)
                    {
                        trans.Rotate(axis, speed * Time.deltaTime, Space.Self);
                    }
                    break;
            }
        }
        public float progress;
        public void UpdateTween()
        {
            switch (operationType)
            {
                case OperationType.CallInSec:
                    time += Time.deltaTime;
                    if (time >= (1f / (float)repeat))
                    {
                        time -= (1f / (float)repeat);
                        action?.Invoke();
                    }
                    break;
            }
            if (isJumping)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime < 0) return;
                if (elapsedTime < time)
                {
                    float progress = elapsedTime / time;
                    float parabola = 1.0f - 4.0f * (progress - 0.5f) * (progress - 0.5f);
                    Vector3 nextPos = Vector3.Lerp(from, to, progress);
                    nextPos.x += parabola * arcHeight.x;
                    nextPos.y += parabola * arcHeight.y;
                    nextPos.z += parabola * arcHeight.z;
                    if (trans != null)
                    {
                        trans.position = nextPos;
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.anchoredPosition = nextPos;
                    }
                }
                else
                {
                    if (trans != null)
                    {
                        trans.position = to;
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.anchoredPosition = to;
                    }
                    tweenUpdates.OnTweenComplete?.Invoke();
                    isJumping = false;
                    GameObject.Destroy(techTween);
                }
            }
            if (isRunning)
            {
                float elapsedTime = Time.time - startTime;
                if (elapsedTime < 0) return; // Waiting for delay
                if (elapsedTime < time)
                {
                    float progress = elapsedTime / time;
                    SetValues(GetValue(progress));
                }
                else
                {
                    SetValues(1);
                    action?.Invoke();
                    tweenUpdates.OnTweenComplete?.Invoke();
                    loopCount++;
                    if (isPingPong)
                    {
                        isReversing = !isReversing;
                    }
                    if (isLooping || loopCount < repeat)
                    {
                        startTime = Time.time; // Restart loop
                    }
                    else
                    {
                        isRunning = false;
                        GameObject.Destroy(techTween);
                    }
                }
            }
        }
        private Vector3 startValue;
        public void StartTrignometroc()
        {
            startTime = Time.time + delay;
            switch (tweenType)
            {
                case TweenType.TrigRotate:
                    if (trans != null)
                    {
                        startValue = trans.localEulerAngles;
                    }
                    if (rectTrans != null)
                    {
                        startValue = rectTrans.localEulerAngles;
                    }
                    break;
                case TweenType.TrigScale:
                    if (trans != null)
                    {
                        startValue = trans.localScale;
                    }
                    if (rectTrans != null)
                    {
                        startValue = rectTrans.localScale;
                    }
                    break;
                case TweenType.TrigMove:
                    if (trans != null)
                    {
                        startValue = trans.localPosition;
                    }
                    if (rectTrans != null)
                    {
                        startValue = rectTrans.localScale;
                    }
                    break;
            }
            isRunning = true;
        }
        public void UpdateTrignometric()
        {

            if (!isRunning) return;
            float elapsedTime = Time.time - startTime;
            if (elapsedTime < 0) return; // Waiting for delay
            if (to == Vector3.zero || frequency == Vector3.zero) return;
            // Compute oscillating values
            float x = startValue.x + Mathf.Sin(Time.timeSinceLevelLoad * frequency.x) * to.x;
            float y = startValue.y + Mathf.Sin(Time.timeSinceLevelLoad * frequency.y) * to.y;
            float z = startValue.z + Mathf.Sin(Time.timeSinceLevelLoad * frequency.z) * to.z;
            tweenUpdates.onUpdateValue?.Invoke(x);
            // Apply transformation based on type
            switch (tweenType)
            {
                case TweenType.TrigRotate:
                    if (trans != null)
                    {
                        trans.transform.eulerAngles = new Vector3(x, y, z);
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.transform.eulerAngles = new Vector3(x, y, z);
                    }
                    break;
                case TweenType.TrigScale:
                    if (trans != null)
                    {
                        trans.transform.localScale = new Vector3(x, y, z);
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.transform.localScale = new Vector3(x, y, z);
                    }
                    break;
                case TweenType.TrigMove:
                    if (trans != null)
                    {
                        trans.transform.localPosition = new Vector3(x, y, z);
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.transform.localPosition = new Vector3(x, y, z);
                    }
                    break;

            }
        }
        public IEnumerator RunTween()
        {
            yield return new WaitForSeconds(delay);
            int loops = isLooping ? int.MaxValue : repeat;
            for (int i = 0; i < loops; i++)
            {
                tweenUpdates.OnTweenStart?.Invoke();
                float elapsedTime = 0;
                while (elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    SetValues(GetValue(elapsedTime / time));
                    yield return null;
                }
                SetValues(1);
                tweenUpdates.OnTweenComplete?.Invoke();
                if (isPingPong)
                {
                    isReversing = !isReversing; // Toggle direction after each cycle
                }
            }
            if (techTween != null)
            {
                GameObject.Destroy(techTween);
            }
        }
        public IEnumerator TrignometricObject()
        {
            Vector3 startValue = Vector3.zero;
            switch (tweenType)
            {
                case TweenType.TrigRotate:
                    if (trans != null)
                    {
                        startValue = trans.localEulerAngles;
                    }
                    if (rectTrans != null)
                    {
                        startValue = rectTrans.localEulerAngles;
                    }
                    break;
                case TweenType.TrigScale:
                    if (trans != null)
                    {
                        startValue = trans.localScale;
                    }
                    if (rectTrans != null)
                    {
                        startValue = rectTrans.localScale;
                    }
                    break;
                case TweenType.TrigMove:
                    if (trans != null)
                    {
                        startValue = trans.localPosition;
                    }
                    if (rectTrans != null)
                    {
                        startValue = rectTrans.localScale;
                    }
                    break;

            }
            while (true) // Keep rotating indefinitely
            {
                if (to != Vector3.zero && frequency != Vector3.zero)
                {
                    // Calculate the rotation for each axis based on sine wave and time
                    float x = startValue.x + Mathf.Sin(Time.timeSinceLevelLoad * frequency.x) * to.x;
                    float y = startValue.y + Mathf.Sin(Time.timeSinceLevelLoad * frequency.y) * to.y;
                    float z = startValue.z + Mathf.Sin(Time.timeSinceLevelLoad * frequency.z) * to.z;
                    // Apply the final calculated rotation
                    switch (tweenType)
                    {
                        case TweenType.TrigRotate:
                            if (trans != null)
                            {
                                trans.transform.eulerAngles = new Vector3(x, y, z);
                            }
                            if (rectTrans != null)
                            {
                                rectTrans.transform.eulerAngles = new Vector3(x, y, z);
                            }
                            break;
                        case TweenType.TrigScale:
                            if (trans != null)
                            {
                                trans.transform.localScale = new Vector3(x, y, z);
                            }
                            if (rectTrans != null)
                            {
                                rectTrans.transform.localScale = new Vector3(x, y, z);
                            }
                            break;
                        case TweenType.TrigMove:
                            if (trans != null)
                            {
                                trans.transform.localPosition = new Vector3(x, y, z);
                            }
                            if (rectTrans != null)
                            {
                                rectTrans.transform.localPosition = new Vector3(x, y, z);
                            }
                            break;

                    }
                }
                // Wait for the next frame before continuing
                yield return null;
            }
        }

        public void SetValues(float valu)
        {
            // Determine the direction of the tween based on ping-pong
            Vector3 start = isReversing ? to : from;
            Vector3 end = isReversing ? from : to;

            newVect = Vector3.Lerp(start, end, valu);
            tweenUpdates.onUpdateIntValue?.Invoke(Mathf.RoundToInt(newVect.x));
            tweenUpdates.onUpdateValue?.Invoke(newVect.x);
            tweenUpdates.onUpdateVector2?.Invoke(new Vector2(newVect.x, newVect.y));
            tweenUpdates.onUpdateVector3?.Invoke(newVect);
            switch (tweenType)
            {
                case TweenType.Value:
                    break;
                case TweenType.MoveLocal:
                    if (trans != null)
                    {
                        trans.transform.localPosition = Vector3.Lerp(start, end, valu);
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.transform.localPosition = Vector3.Lerp(start, end, valu);
                    }
                    break;
                case TweenType.Move:
                    if (trans != null)
                    {
                        trans.transform.position = Vector3.Lerp(start, end, valu);
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.transform.position = Vector3.Lerp(start, end, valu);
                    }
                    break;
                case TweenType.Scale:
                    if (trans != null)
                    {
                        trans.transform.localScale = Vector3.Lerp(start, end, valu);
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.transform.localScale = Vector3.Lerp(start, end, valu);
                    }
                    break;
                case TweenType.Rotate:
                    float xNextRot = closestRot(start.x, end.x);
                    float yNextRot = closestRot(start.y, end.y);
                    float zNextRot = closestRot(start.z, end.z);
                    if (trans != null)
                    {
                        trans.transform.eulerAngles = Vector3.Lerp(start, new Vector3(xNextRot, yNextRot, zNextRot), valu);
                    }
                    if (rectTrans != null)
                    {
                        rectTrans.transform.localEulerAngles = Vector3.Lerp(start, new Vector3(xNextRot, yNextRot, zNextRot), valu);
                    }
                    break;
                case TweenType.CanvasGroupAlpha:
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = newVect.x;
                    }
                    break;
                case TweenType.SpriteRendererAlpha:
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newVect.x);
                    }
                    break;
            }
        }
        //Generated from Ai Tools
        public float GetValue(float t)
        {
            float value = t;
            switch (easeTween)
            {
                case EaseTween.Linear:
                    value = t;
                    break;
                case EaseTween.EaseInQuad:
                    value = t * t;
                    break;
                case EaseTween.EaseOutQuad:
                    value = -t * (t - 2);
                    break;
                case EaseTween.EaseInOutQuad:
                    value = t < 0.5f ? 2 * t * t : -2 * t * (t - 2) - 1;
                    break;
                case EaseTween.EaseOutInQuad:
                    value = t < 0.5f ? 0.5f * (-2 * t * (t - 1)) : 0.5f * ((2 * t - 1) * (2 * t - 1)) + 0.25f;
                    break;

                case EaseTween.EaseInCubic:
                    value = t * t * t;
                    break;
                case EaseTween.EaseOutCubic:
                    float f = t - 1;
                    value = f * f * f + 1;
                    break;
                case EaseTween.EaseInOutCubic:
                    value = t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
                    break;
                case EaseTween.EaseOutInCubic:
                    value = t < 0.5f
                        ? 0.5f * ((2 * t - 1) * (2 * t - 1) * (2 * t - 1) + 1)
                        : 0.5f * (2 * t - 1) * (2 * t - 1) * (2 * t - 1) + 0.5f;
                    break;

                case EaseTween.EaseInQuart:
                    value = t * t * t * t;
                    break;
                case EaseTween.EaseOutQuart:
                    f = t - 1;
                    value = 1 - f * f * f * f;
                    break;
                case EaseTween.EaseInOutQuart:
                    value = t < 0.5f ? 8 * t * t * t * t : 1 - 8 * (t - 1) * (t - 1) * (t - 1) * (t - 1);
                    break;
                case EaseTween.EaseOutInQuart:
                    value = t < 0.5f
                        ? 0.5f * (1 - Mathf.Pow(1 - 2 * t, 4))
                        : 0.5f * Mathf.Pow(2 * t - 1, 4) + 0.5f;
                    break;

                case EaseTween.EaseInQuint:
                    value = t * t * t * t * t;
                    break;
                case EaseTween.EaseOutQuint:
                    f = t - 1;
                    value = f * f * f * f * f + 1;
                    break;
                case EaseTween.EaseInOutQuint:
                    value = t < 0.5f ? 16 * t * t * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) * (2 * t - 2) * (2 * t - 2) + 1;
                    break;
                case EaseTween.EaseOutInQuint:
                    value = t < 0.5f
                        ? 0.5f * (Mathf.Pow(2 * t - 1, 5) + 1)
                        : 0.5f * Mathf.Pow(2 * t - 1, 5) + 0.5f;
                    break;

                case EaseTween.EaseInSine:
                    value = 1 - Mathf.Cos((t * Mathf.PI) / 2);
                    break;
                case EaseTween.EaseOutSine:
                    value = Mathf.Sin((t * Mathf.PI) / 2);
                    break;
                case EaseTween.EaseInOutSine:
                    value = 0.5f * (1 - Mathf.Cos(Mathf.PI * t));
                    break;
                case EaseTween.EaseOutInSine:
                    value = t < 0.5f
                        ? 0.5f * Mathf.Sin(t * 2 * Mathf.PI / 2)
                        : 0.5f * (1 - Mathf.Cos((2 * t - 1) * Mathf.PI / 2)) + 0.5f;
                    break;

                case EaseTween.EaseInExpo:
                    value = (t == 0) ? 0 : Mathf.Pow(2, 10 * (t - 1));
                    break;
                case EaseTween.EaseOutExpo:
                    value = (t == 1) ? 1 : 1 - Mathf.Pow(2, -10 * t);
                    break;
                case EaseTween.EaseInOutExpo:
                    if (t == 0 || t == 1) value = t;
                    else value = t < 0.5f ? Mathf.Pow(2, 10 * (2 * t - 1)) / 2 : (2 - Mathf.Pow(2, -10 * (2 * t - 1))) / 2;
                    break;
                case EaseTween.EaseOutInExpo:
                    value = t < 0.5f
                        ? 0.5f * (1 - Mathf.Pow(2, -20 * t))
                        : 0.5f * Mathf.Pow(2, 20 * (t - 1)) + 0.5f;
                    break;

                case EaseTween.EaseInCirc:
                    value = 1 - Mathf.Sqrt(1 - t * t);
                    break;
                case EaseTween.EaseOutCirc:
                    value = Mathf.Sqrt(1 - (t - 1) * (t - 1));
                    break;
                case EaseTween.EaseInOutCirc:
                    value = t < 0.5f ? (1 - Mathf.Sqrt(1 - 4 * t * t)) / 2 : (Mathf.Sqrt(1 - (2 - 2 * t) * (2 - 2 * t)) + 1) / 2;
                    break;
                case EaseTween.EaseOutInCirc:
                    value = t < 0.5f
                        ? 0.5f * Mathf.Sqrt(1 - (2 * t - 1) * (2 * t - 1))
                        : 0.5f * (1 - Mathf.Sqrt(1 - (2 * t - 1) * (2 * t - 1))) + 0.5f;
                    break;

                case EaseTween.EaseInElastic:
                    value = Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t - 1.1f) * 5 * Mathf.PI);
                    break;
                case EaseTween.EaseOutElastic:
                    value = Mathf.Pow(2, -10 * t) * Mathf.Sin((t - 0.1f) * 5 * Mathf.PI) + 1;
                    break;
                case EaseTween.EaseInOutElastic:
                    value = t < 0.5f
                        ? 0.5f * Mathf.Pow(2, 10 * (2 * t - 1)) * Mathf.Sin((2 * t - 1.1f) * 5 * Mathf.PI)
                        : 0.5f * Mathf.Pow(2, -10 * (2 * t - 1)) * Mathf.Sin((2 * t - 1.1f) * 5 * Mathf.PI) + 1;
                    break;
                case EaseTween.EaseOutInElastic:
                    value = t < 0.5f
                        ? 0.5f * (Mathf.Pow(2, -10 * (2 * t)) * Mathf.Sin((2 * t - 0.1f) * 5 * Mathf.PI)) + 0.5f
                        : 0.5f * Mathf.Pow(2, 10 * (2 * t - 2)) * Mathf.Sin((2 * t - 1.1f) * 5 * Mathf.PI);
                    break;

                case EaseTween.EaseInBack:
                    float s = 1.70158f;
                    value = t * t * ((s + 1) * t - s);
                    break;
                case EaseTween.EaseOutBack:
                    s = 1.70158f;
                    f = t - 1;
                    value = 1 + f * f * ((s + 1) * f + s);
                    break;
                case EaseTween.EaseInOutBack:
                    s = 1.70158f;
                    value = t < 0.5f
                        ? 0.5f * (t * t * ((s * 1.525f + 1) * t - s * 1.525f))
                        : 0.5f * ((t - 1) * (t - 1) * ((s * 1.525f + 1) * (t - 1) + s * 1.525f) + 2);
                    break;
                case EaseTween.EaseOutInBack:
                    s = 1.70158f;
                    if (t < 0.5f)
                    {
                        f = 2 * t - 1;
                        value = 0.5f * (1 + f * f * ((s + 1) * f + s));
                    }
                    else
                    {
                        f = 2 * t - 1;
                        value = 0.5f * (f * f * ((s + 1) * f - s)) + 0.5f;
                    }
                    break;

                case EaseTween.EaseOutBounce:
                    value = EaseOutBounce(t);
                    break;
                case EaseTween.EaseInBounce:
                    value = 1 - EaseOutBounce(1 - t);
                    break;
                case EaseTween.EaseInOutBounce:
                    value = t < 0.5f ? (1 - EaseOutBounce(1 - 2 * t)) * 0.5f : (1 + EaseOutBounce(2 * t - 1)) * 0.5f;
                    break;
                case EaseTween.EaseOutInBounce:
                    value = t < 0.5f
                        ? 0.5f * EaseOutBounce(2 * t)
                        : 0.5f * (1 - EaseOutBounce(2 - 2 * t)) + 0.5f;
                    break;

                case EaseTween.EaseSpring:
                    value = Mathf.Sin(t * Mathf.PI * (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t;
                    break;
            }
            return value;
        }

        private float EaseOutBounce(float t)
        {
            if (t < (1 / 2.75f))
                return 7.5625f * t * t;
            else if (t < (2 / 2.75f))
                return 7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f;
            else if (t < (2.5 / 2.75f))
                return 7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f;
            else
                return 7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f;
        }
        public TweenDetail SetEaseType(EaseTween _easeTween)
        {
            easeTween = _easeTween;
            return this;
        }
        public TweenDetail SetOnTweenStart(Action onComplete)
        {
            tweenUpdates.OnTweenStart = onComplete;
            return this;
        }
        public TweenDetail GetCompleteCallback(Action onComplete)
        {
            tweenUpdates.OnTweenComplete = onComplete;
            return this;
        }
        public TweenDetail GetVector3Update(Action<Vector3> onUpdate)
        {
            tweenUpdates.onUpdateVector3 = onUpdate;
            return this;
        }
        public TweenDetail GetValueUpdate(Action<float> onUpdate)
        {
            tweenUpdates.onUpdateValue = onUpdate;
            return this;
        }
        public TweenDetail GetValueIntUpdate(Action<int> onUpdate)
        {
            tweenUpdates.onUpdateIntValue = onUpdate;
            return this;
        }
        public TweenDetail GetVector2Update(Action<Vector2> onUpdate)
        {
            tweenUpdates.onUpdateVector2 = onUpdate;
            return this;
        }
        public TweenDetail SetLoopCount(int count)
        {
            repeat = count;
            return this;
        }
        public TweenDetail SetInfiniteLoop(bool loop)
        {
            isLooping = loop;
            return this;
        }
        public TweenDetail SetPingPong(bool pingPong)
        {
            isPingPong = pingPong;
            return this;
        }
        public float closestRot(float from, float to)
        {
            float minusWhole = 0 - (360 - to);
            float plusWhole = 360 + to;
            float toDiffAbs = Mathf.Abs(to - from);
            float minusDiff = Mathf.Abs(minusWhole - from);
            float plusDiff = Mathf.Abs(plusWhole - from);
            if (toDiffAbs < minusDiff && toDiffAbs < plusDiff)
            {
                return to;
            }
            else
            {
                if (minusDiff < plusDiff)
                {
                    return minusWhole;
                }
                else
                {
                    return plusWhole;
                }
            }
        }
    }
}