// Created by dylan@hathora.dev

using Hathora.Net.Client;
using Hathora.Net.Server;
using UnityEngine;

namespace Hathora.Net.Common
{
    public abstract class NetMgrBase : MonoBehaviour
    {
        protected static NetServerMgr s_ServerMgr => NetServerMgr.Singleton;
        protected static NetClientMgr s_ClientMgr => NetClientMgr.Singleton;
        protected static NetCommonMgr s_NetCommonMgr => NetCommonMgr.Singleton;
        protected static NetUI s_netUi => NetUI.Singleton;
    }
}
