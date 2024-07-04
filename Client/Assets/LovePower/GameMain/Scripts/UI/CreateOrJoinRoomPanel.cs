using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace LovePower
{
    /// <summary>
    /// UI - ��������뷿��
    /// </summary>
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
            Log.Info("���󴴽�����");
            GameEntry.TcpClient.CreateRoom();
        }

        private void OnJoinRoom()
        {
            
        }
    }
}

