using System;
using System.Reactive.Subjects;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pavantares.CCG.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pavantares.CCG.View
{
    public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Transform root;

        [SerializeField]
        private TMP_Text titleText;

        [SerializeField]
        private TMP_Text descriptionText;

        [SerializeField]
        private TMP_Text healthText;

        [SerializeField]
        private TMP_Text manaText;

        [SerializeField]
        private TMP_Text attackText;

        [SerializeField]
        private RawImage contentImage;

        [SerializeField]
        private Image shineImage;

        [SerializeField]
        private Image rootImage;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private readonly ISubject<CardView> onRelease = new Subject<CardView>();

        private int health;
        private int mana;
        private int attack;
        private Tweener pickCardTweener;
        private Sequence positionAndRotationSequence;
        private Sequence animateRemovingSequence;
        private Sequence completeReleaseSequence;
        private RectTransform parentContainer;
        private Vector2 localPosition;
        private Quaternion localRotation;
        private Vector2 delta;

        public Card Card { get; private set; }

        public IObservable<CardView> OnRelease => onRelease;

        private void Awake()
        {
            parentContainer = transform.parent as RectTransform;
        }

        private void OnDestroy()
        {
            pickCardTweener?.Kill();
            positionAndRotationSequence?.Kill();
            animateRemovingSequence?.Kill();
            completeReleaseSequence?.Kill();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PickCard(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PickCard(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentContainer,
                eventData.position,
                eventData.pressEventCamera,
                out var pointerPosition);
            localPosition = transform.localPosition;
            delta = localPosition - pointerPosition;
            rootImage.raycastTarget = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onRelease.OnNext(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            PickCard(false);
            SetActiveShine(true);
            transform.DORotateQuaternion(Quaternion.identity, 0.2f);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentContainer,
                eventData.position,
                eventData.pressEventCamera,
                out var pointerPosition);
            transform.localPosition = pointerPosition + delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetActiveShine(false);
        }

        public void Initialize(Card card)
        {
            this.Card = card;
            health = card.Health;
            mana = card.Mana;
            attack = card.Attack;

            titleText.text = card.Title;
            descriptionText.text = card.Description;
            contentImage.texture = card.Picture;
            healthText.text = card.Health.ToString();
            manaText.text = card.Mana.ToString();
            attackText.text = card.Attack.ToString();
        }

        public async UniTask SetLocalPositionAndLocalRotation(Vector2 position, Quaternion rotation, float duration = 0.2f)
        {
            if (Card.IsOnBoard)
            {
                return;
            }

            positionAndRotationSequence?.Kill();
            positionAndRotationSequence = DOTween.Sequence();
            positionAndRotationSequence.Join(transform.DOLocalMove(position, duration));
            positionAndRotationSequence.Join(transform.DORotateQuaternion(rotation, duration));

            localPosition = position;
            localRotation = rotation;

            await positionAndRotationSequence;
        }

        public async UniTask RefreshValues()
        {
            if (Card.IsOnBoard)
            {
                return;
            }

            if (health != Card.Health)
            {
                await AnimateCounting(healthText, health, Card.Health);
                health = Card.Health;
            }

            if (mana != Card.Mana)
            {
                manaText.text = Card.Mana.ToString();
                await AnimateCounting(manaText, mana, Card.Mana);
                mana = Card.Mana;
            }

            if (attack != Card.Attack)
            {
                await AnimateCounting(attackText, attack, Card.Attack);
                attack = Card.Attack;
            }
        }

        public async UniTask AnimateRemoving()
        {
            if (Card.IsOnBoard)
            {
                return;
            }

            var alpha = 1;
            var point = transform.localPosition + 300 * (transform.localRotation * Vector3.up);

            animateRemovingSequence?.Kill();
            animateRemovingSequence = DOTween.Sequence();
            animateRemovingSequence.Join(transform.DOLocalMoveY(point.y, 0.4f).SetEase(Ease.OutBack));
            animateRemovingSequence.Join(DOTween.To(() => alpha, x => alpha = x, 0, 0.8f).OnUpdate(() => canvasGroup.alpha = alpha));

            await animateRemovingSequence;
        }

        public void CompleteRelease(Transform boardContainer)
        {
            if (Card.IsOnBoard)
            {
                return;
            }

            completeReleaseSequence?.Kill();
            completeReleaseSequence = DOTween.Sequence();

            if (boardContainer == null)
            {
                completeReleaseSequence.Join(transform.DOLocalMove(localPosition, 0.2f));
                completeReleaseSequence.Join(transform.DORotateQuaternion(localRotation, 0.2f));
                rootImage.raycastTarget = true;
            }
            else
            {
                transform.SetParent(boardContainer);
                completeReleaseSequence.Join(transform.DOLocalMove(Vector3.zero, 0.2f));
                completeReleaseSequence.Join(transform.DORotateQuaternion(Quaternion.identity, 0.2f));
                Card.IsOnBoard = true;
            }
        }

        private void SetActiveShine(bool isActive)
        {
            shineImage.gameObject.SetActive(isActive);
        }

        private void PickCard(bool isPick)
        {
            pickCardTweener?.Kill();
            pickCardTweener = root.DOLocalMoveY(isPick ? 25 : 0, 0.2f);
        }

        private static async UniTask AnimateCounting(TMP_Text text, int current, int next)
        {
            var value = current;

            while (value != next)
            {
                value = value > next ? value - 1 : value + 1;
                text.SetText(value.ToString());
                await UniTask.DelayFrame(10);
            }
        }
    }
}
