// Created by dylan@hathora.dev

using System;
using Hathora.Cloud.Sdk.Model;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Hathora.Scripts.Net.Common.Models
{
    /// <summary>
    /// Set transpport configurations for where the server will listen.
    /// Unlike TransportInfo, here, you can customize the nickname (instead of "default").
    /// Leave the nickname null and we'll ignore this class.
    /// </summary>
    [Serializable]
    public class ExtraTransportInfo : TransportInfo
    {
        [SerializeField, Tooltip("Choose an arbitrary name to identify this transpport easier. " +
             "`Default` is reserved. Leave this empty and we'll ignore this class.")]
        private string _transportNickname;

        /// <summary>
        /// Override this if you want the name to be custom
        /// </summary>
        /// <returns></returns>
        public override string GetTransportNickname()
        {
            Assert.IsFalse(_transportNickname == "default", 
                "Extra Transport nickname cannot be 'default' (reserved)");
            
            return _transportNickname;   
        }
    }
}
