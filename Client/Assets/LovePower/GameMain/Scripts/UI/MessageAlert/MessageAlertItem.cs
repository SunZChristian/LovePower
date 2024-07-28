using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LovePower
{
    public class MessageAlertItem : MonoBehaviour
    {
        public TextMeshProUGUI m_txt_content;

        public void SetContent(string content)
        {
            m_txt_content.text = content;

            var width = m_txt_content.preferredWidth;
            var rect = GetComponent<RectTransform>();
            var sizeDelta = rect.sizeDelta;
            sizeDelta.x = width + 60;
            rect.sizeDelta = sizeDelta;
        }
    }
}