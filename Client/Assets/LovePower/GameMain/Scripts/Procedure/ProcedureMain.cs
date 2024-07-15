using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

namespace LovePower
{
    public class ProcedureMain : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }


        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.UI.OpenUIForm(EUIFormID.LoadingPanel);
            GameEntry.UI.OpenUIForm(EUIFormID.MessageAlertPanel);
            GameEntry.UI.OpenUIForm(EUIFormID.CreateOrJoinRoomPanel, this);

        }
    }
}