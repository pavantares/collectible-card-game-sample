using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pavantares.CCG.View
{
    public class BoardView : ViewBase, IPointerEnterHandler, IPointerExitHandler
    {
        private bool isPointerEntered;

        private readonly List<Transform> containers = new();

        private void Awake()
        {
            for (var i = 0; i < 6; i++)
            {
                containers.Add(CreateContainer(i));
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerEntered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerEntered = false;
        }

        public Transform GetNextContainer()
        {
            if (isPointerEntered)
            {
                for (var i = 0; i < containers.Count; i++)
                {
                    var container = containers[i];

                    if (container.childCount == 0)
                    {
                        return container;
                    }
                }
            }

            return null;
        }

        private Transform CreateContainer(int index)
        {
            var container = new GameObject($"Container{index}", typeof(RectTransform)).transform;
            container.SetParent(transform, false);

            return container;
        }
    }
}
