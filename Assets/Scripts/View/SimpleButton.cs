using System;
using System.Reactive;
using System.Reactive.Subjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pavantares.CCG.View
{
    public class SimpleButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private TMP_Text text;

        private readonly ISubject<Unit> onClick = new Subject<Unit>();

        public IObservable<Unit> OnClick => onClick;

        private void OnEnable()
        {
            button.onClick.AddListener(() => onClick.OnNext(Unit.Default));
        }

        private void OnDisable()
        {
            button.onClick.RemoveAllListeners();
        }

        public void Initialize(string title)
        {
            text.text = title;
        }
    }
}
