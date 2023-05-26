// Created by dylan@hathora.dev

using System.Collections.Generic;
using Hathora.Cloud.Sdk.Model;
using Hathora.Scripts.Net.Common;
using UnityEditor;

namespace Hathora.Scripts.Utils.Editor.ConfigStyle
{
    public class HathoraConfigPostAuthBodyRoomLobbyUI : HathoraConfigUIBase
    {
        #region Vars
        // Foldouts
        private bool isLobbySettingsFoldout;
        #endregion // Vars


        #region Init
        public HathoraConfigPostAuthBodyRoomLobbyUI(
            NetHathoraConfig _config, 
            SerializedObject _serializedConfig)
            : base(_config, _serializedConfig)
        {
            if (!HathoraConfigUI.ENABLE_BODY_STYLE)
                return;
        }
        #endregion // Init


        #region UI Draw
        public void Draw()
        {
            if (!IsAuthed)
                return; // You should be calling HathoraConfigPreAuthBodyUI.Draw()

            insertLobbySettingsFoldout();
        }
        
        private void insertLobbySettingsFoldout()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            isLobbySettingsFoldout = EditorGUILayout.Foldout(
                isLobbySettingsFoldout, 
                "Lobby Settings (optional)");
            
            if (isLobbySettingsFoldout)
            {
                EditorGUILayout.EndVertical(); // End of foldout box skin
                return;
            }
    
            EditorGUI.indentLevel++;
            InsertSpace2x();
            
            // TODO: Add lobby settings UI here.
            
            EditorGUILayout.EndVertical(); // End of foldout box skin
            InsertSpace3x();
            EditorGUI.indentLevel--;
        }
        #endregion // UI Draw

        
        #region Event Logic
        // TODO
        #endregion // Event Logic
    }
}
