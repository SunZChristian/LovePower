using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LovePower
{
    public class CreateOrJoinRoomPanel : PanelBase
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
            GameEntry.TcpClient.CreateRoom();
        }

        private void OnJoinRoom()
        {
            
        }
    }
}

