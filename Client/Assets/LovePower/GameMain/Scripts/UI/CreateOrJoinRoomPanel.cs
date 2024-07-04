using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace LovePower
{
    /// <summary>
    /// UI - 创建或加入房间
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
            Log.Info("请求创建房间");
            GameEntry.TcpClient.CreateRoom();
        }

        private void OnJoinRoom()
        {
            
        }
    }
}

