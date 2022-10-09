using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Cysharp.Threading.Tasks;
using Pavantares.CCG.Models;
using UnityEngine;

namespace Pavantares.CCG.View
{
    public class CardsHolderView : ViewBase
    {
        [SerializeField]
        private CardView cardViewPrefab;

        [SerializeField]
        private RectTransform container;

        [Space]
        [SerializeField]
        private float cardAngleStep;

        [SerializeField]
        private float cardShift;

        [SerializeField]
        private float radius;

        private readonly ISubject<CardView> onReleaseCardView = new Subject<CardView>();
        private readonly List<CardView> cardViews = new();

        public IObservable<CardView> OnReleaseCardView => onReleaseCardView;

        public void AddCardView(Card card)
        {
            var cardView = CreateCardView(card);

            cardViews.Add(cardView);

            RefreshLayout().Forget();
        }

        public async UniTask RemoveFromHandCardView(string cardId)
        {
            var cardView = cardViews.Find(x => x.Card.Id == cardId);

            if (cardView == null || cardView.Card.IsOnBoard)
            {
                return;
            }

            cardViews.Remove(cardView);
            await cardView.AnimateRemoving();
            Destroy(cardView.gameObject);
        }

        public async UniTask RefreshValues()
        {
            for (var i = 0; i < cardViews.Count; i++)
            {
                var cardView = cardViews[i];
                await cardView.RefreshValues();
            }
        }

        public async UniTask RefreshLayout()
        {
            var validCardsView = cardViews.FindAll(x => !x.Card.IsOnBoard);
            var middlePoint = new Vector2(0, cardShift);
            var centerPoint = middlePoint + radius * Vector2.down;
            var distance = radius + cardShift;
            var startAngle = 0.5f * cardAngleStep * (validCardsView.Count - 1);
            var tasks = new List<UniTask>();

            for (var i = 0; i < validCardsView.Count; i++)
            {
                var angle = startAngle - i * cardAngleStep;
                var localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                var localDirection = localRotation * Vector3.up;
                var localPosition = centerPoint + (Vector2)localDirection * distance;

                var cardView = validCardsView[i];
                var task = cardView.SetLocalPositionAndLocalRotation(localPosition, localRotation);
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        private CardView CreateCardView(Card card)
        {
            var cardView = Instantiate(cardViewPrefab, container);
            cardView.Initialize(card);
            cardView.OnRelease.Subscribe(onReleaseCardView.OnNext).AddTo(cardView.GetCancellationTokenOnDestroy());

            return cardView;
        }
    }
}
