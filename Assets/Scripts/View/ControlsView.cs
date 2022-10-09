using System;
using System.Reactive;
using UnityEngine;

namespace Pavantares.CCG.View
{
    public class ControlsView : ViewBase
    {
        [SerializeField]
        private SimpleButton simpleButtonPrefab;

        [SerializeField]
        private RectTransform container;

        private SimpleButton randomHealthButton;
        private SimpleButton randomManaButton;
        private SimpleButton randomAttackButton;
        private SimpleButton restartButton;

        public IObservable<Unit> OnRandomHealth => randomHealthButton.OnClick;
        public IObservable<Unit> OnRandomMana => randomManaButton.OnClick;
        public IObservable<Unit> OnRandomAttack => randomAttackButton.OnClick;
        public IObservable<Unit> OnRestart => restartButton.OnClick;

        private void Awake()
        {
            randomHealthButton = CreateSimpleButton("Random Health");
            randomManaButton = CreateSimpleButton("Random Mana");
            randomAttackButton = CreateSimpleButton("Random Attack");
            restartButton = CreateSimpleButton("Restart");
        }

        private SimpleButton CreateSimpleButton(string title)
        {
            var button = Instantiate(simpleButtonPrefab, container);
            button.Initialize(title);

            return button;
        }
    }
}
