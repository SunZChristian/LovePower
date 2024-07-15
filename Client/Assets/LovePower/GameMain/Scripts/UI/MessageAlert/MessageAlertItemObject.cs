using GameFramework;
using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class MessageAlertItemObject : ObjectBase
    {       
        public static MessageAlertItemObject Create(object target)
        {
            MessageAlertItemObject item = ReferencePool.Acquire<MessageAlertItemObject>();
            item.Initialize(target);
            return item;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            MessageAlertItem item = (MessageAlertItem)Target;
            if (item != null)
            {
                item.gameObject.SetActive(true);
                item.transform.SetAsLastSibling();
            }
                
        }

        protected override void OnUnspawn()
        {
            MessageAlertItem item = (MessageAlertItem)Target;
            if (item != null)
                item.gameObject.SetActive(false);

            base.OnUnspawn();
        }

        protected override void Release(bool isShutdown)
        {
            MessageAlertItem item = (MessageAlertItem)Target;
            if (Target == null)
                return;
            Object.Destroy(item.gameObject);
        }
    }
}

