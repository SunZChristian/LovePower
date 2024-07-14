using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LovePower
{
    /// <summary>
    /// Loadinging 界面
    /// </summary>
    public class LoadingPanel : PanelBase
    {
        public GameObject m_content;

        public Image m_img_loading;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            StopLoading();

            //注册事件
            GameEntry.Event.Subscribe(UILoadingStateEventArgs.EventId, OnLoadingStateChanage);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            //移除事件
            GameEntry.Event.Unsubscribe(UILoadingStateEventArgs.EventId, OnLoadingStateChanage);

            base.OnClose(isShutdown, userData);
        }

        private void OnLoadingStateChanage(object sender, GameEventArgs e)
        {
            UILoadingStateEventArgs args = (UILoadingStateEventArgs)e;
            if (args.Show)
                ShowLoading();
            else
                StopLoading();
        }

        private void ShowLoading()
        {            
            DOTween.Kill(m_img_loading,false);
            m_img_loading.rectTransform.Rotate(Vector3.zero);
            m_img_loading.rectTransform.DORotate(new Vector3(0, 0, -360), 5, RotateMode.FastBeyond360);
            m_content.SetActive(true);
        }

        private void StopLoading()
        {
            DOTween.Kill(m_img_loading, false);
            m_content.SetActive(false);
        }
    }
}

