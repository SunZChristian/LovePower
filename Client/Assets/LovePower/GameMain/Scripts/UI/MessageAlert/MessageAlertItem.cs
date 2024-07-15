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
        }
    }
}