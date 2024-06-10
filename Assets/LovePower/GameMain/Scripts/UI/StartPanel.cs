using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LovePower
{
    public class StartPanel : PanelBase
    {
        public Button m_btnCreateRoom;
        public Button m_btnJoinRoom;

        private void Awake()
        {
            m_btnCreateRoom.onClick.AddListener(OnCreateRoom);
            m_btnJoinRoom.onClick.AddListener(OnJoinRoom);
        }

        private void OnCreateRoom()
        {
            NetManager.Instance.CreateRoom();
        }

        private void OnJoinRoom()
        {
            NetManager.Instance.JoinRoom();
        }
    }
}

