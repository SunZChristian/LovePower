using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace LovePower
{
    public static class UIExtensions 
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static int? OpenUIForm(this UIComponent uiComponent, EUIFormID uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, userData);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!drUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, Constant.AssetPriority.UIFormAsset, drUIForm.PauseCoveredUIForm, userData);

        }

        public static void CloseUIForm(this UIComponent uiComponent, PanelBase uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static void ShowAlert(this UIComponent uiComponent, string content)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow((int)EUIFormID.MessageAlertPanel);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", EUIFormID.MessageAlertPanel.ToString());
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!drUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    var ui = (MessageAlertPanel)(uiComponent.GetUIForm(assetName).Logic);
                    ui.ShowAlert(content);
                }else
                {
                    uiComponent.OpenUIForm(EUIFormID.MessageAlertPanel,content);
                }
            }
        }
    }
}

