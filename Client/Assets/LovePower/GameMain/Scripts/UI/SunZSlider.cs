using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace LovePower
{
    public class SunZSlider : Slider
    {
        public UnityEvent OnPointDownEvent;

        public UnityEvent OnPointUpEvent;


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            OnPointDownEvent?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            OnPointUpEvent?.Invoke();
        }
    }
}

