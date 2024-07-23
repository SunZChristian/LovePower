using GameFramework.Event;
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

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(CreateRoomSuccessArgs.EventId, OnCreateRoomSuccess);
            GameEntry.Event.Subscribe(JoinRoomSuccessArgs.EventId, OnJoinRoomSuccess);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(CreateRoomSuccessArgs.EventId, OnCreateRoomSuccess);
            GameEntry.Event.Unsubscribe(JoinRoomSuccessArgs.EventId, OnJoinRoomSuccess);
            base.OnClose(isShutdown, userData);
        }

        private void OnCreateRoom()
        {
            Log.Info("请求创建房间");
            GameEntry.TcpClient.CreateRoom();
        }

        private void OnJoinRoom()
        {
            Log.Info("请求加入房间");
            GameEntry.TcpClient.JoinRoom();
        }

        private void OnCreateRoomSuccess(object sender, GameEventArgs e)
        {
            //收到这个消息就判定创建房间成功了
            PlayerRuntimeData.IsOwner = true;
            
            //跳转播放界面
            GameEntry.UI.OpenUIForm(EUIFormID.VideoHallPanel);
            GameEntry.UI.CloseUIForm(this);
        }

        private void OnJoinRoomSuccess(object sender, GameEventArgs e)
        {
            //收到这个消息就判定进入房间成功了
            PlayerRuntimeData.IsOwner = false;
            //跳转播放界面
            GameEntry.UI.OpenUIForm(EUIFormID.VideoHallPanel);
            GameEntry.UI.CloseUIForm(this);
        }
    }
}

