namespace Muciojad.SpaceHorror.Gameplay.TriggerEvents
{
    using System;
    using System.Collections.Generic;
    using NaughtyAttributes;
    using UnityEngine;

    public class TriggerEventSpot : MonoBehaviour
    {
        #region Public Variables
        public event Action EventTriggered; 
        #endregion

        #region Inspector
        [BoxGroup("Settings")] [SerializeField]
        private bool _UseTags;

        [BoxGroup("Settings")] [SerializeField] [ShowIf(nameof(_UseTags))]
        private List<string> _TriggeringTags = new List<string>();

        [BoxGroup("Settings")] [SerializeField]
        private bool _DisableWhenTriggered;
        #endregion
        
        #region Unity Methods
        private void OnTriggerEnter(Collider other)
        {
            if (!_UseTags)
            {
                EventTriggered?.Invoke();
                return;
            }
            if (!_TriggeringTags.Contains(other.tag)) return;
            EventTriggered?.Invoke();
            if (_DisableWhenTriggered)
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}