using System;
using UnityEngine;

namespace ScriptHeadComments.Editor
{
    [CreateAssetMenu(fileName = "ScriptHeadComments", menuName = "ScriptableObjects/ScriptHeadComments", order = 1)]
    public class ScriptHeadComments : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        private bool isInitialized;
        public string authorName;
        public string assembleName;

        private void OnEnable()
        {
            if (!isInitialized)
            {
                authorName = Environment.UserName;
                assembleName = "NAMESPACE";
                isInitialized = true;
            }
        }
    }
}
