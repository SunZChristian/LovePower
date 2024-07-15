using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace LovePower
{
    /// <summary>
    /// 消息提醒界面
    /// </summary>
    public class MessageAlertPanel : PanelBase
    {
        public MessageAlertItem prefab_item;

        private IObjectPool<MessageAlertItemObject> m_itemObjectPool;
        private List<MessageAlertItem> m_activeItems;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_itemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<MessageAlertItemObject>();
            m_activeItems = new List<MessageAlertItem>();

            if (userData != null)
            {
                ShowAlert(userData.ToString());
            }
        }

        //private int count = 0;
        //protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        //{
        //    base.OnUpdate(elapseSeconds, realElapseSeconds);

        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        GameEntry.Event.FireNow(UILoadingStateEventArgs.EventId, UILoadingStateEventArgs.Create(true));
        //    }

        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        GameEntry.Event.FireNow(UILoadingStateEventArgs.EventId, UILoadingStateEventArgs.Create(false));
        //    }
        //}

        public void ShowAlert(string content)
        {
            var item = CreateItem();
            item.SetContent(content);
            item.transform.localPosition = Vector3.zero;
            Sequence quence = DOTween.Sequence();
            quence.Append(item.transform.DOLocalMoveY(300, 0.5f));
            quence.AppendInterval(1);
            quence.OnComplete(() =>
               {
                   RecycleAlert(item);
               });
        }

        private void RecycleAlert(MessageAlertItem item)
        {
            m_itemObjectPool.Unspawn(item);
        }

        private MessageAlertItem CreateItem()
        {
            MessageAlertItem item = null;
            MessageAlertItemObject itemObj = m_itemObjectPool.Spawn();
            if (itemObj != null)
            {
                item = (MessageAlertItem)itemObj.Target;
            }
            else
            {
                item = Instantiate(prefab_item);
                item.transform.SetParent(transform);
                item.transform.localPosition = Vector3.zero;
                m_itemObjectPool.Register(MessageAlertItemObject.Create(item), true);
            }

            return item;
        }
    }
}