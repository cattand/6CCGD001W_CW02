using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Guirao.UltimateTextDamage
{
    public class UITextDamage : MonoBehaviour
    {
        public event Action< UITextDamage > eventOnEnd;

        [Header( "The Text ui of the item" )]
        public Text label;

        public Canvas Canvas { get; set; }
        public float Offset { get; set; }
        public Camera Cam { get; set; }
        public float limitPercentTop { get; set; }
        public float limitPercentBottom { get; set; }
        public float limitPercentLeft { get; set; }
        public float limitPercentRight { get; set; }
        public bool followsTarget { get; set; }

        private RectTransform rectTransform;
        private Transform toFollow;
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private bool firstTime;

        /// <summary>
        /// Start Monobehaviour
        /// </summary>
        void Start( )
        {
            rectTransform = transform as RectTransform;
        }

        /// <summary>
        /// Shows the ui text
        /// </summary>
        /// <param name="text">string that will be filled</param>
        /// <param name="transform">transform in world space where the text will be positioned</param>
        public void Show( string text , Transform transform )
        {
            label.text = text;
            Offset = 0;
            toFollow = transform;
            firstTime = true;
            gameObject.SetActive( true );
        }

        /// <summary>
        /// LateUpdate from Monobehaviour
        /// </summary>
        void LateUpdate( )
        {
            if( !toFollow ) return;

            UpdatePosition( );
        }

        /// <summary>
        /// Animation event, must call this at the end of the text animation.
        /// </summary>
        public void End( )
        {
            if( eventOnEnd != null ) eventOnEnd( this );
            gameObject.SetActive( false );
        }

        /// <summary>
        /// Updates the position of the text
        /// </summary>
        private void UpdatePosition( )
        {
            Vector3 uiWorldPos = toFollow.position;
            Quaternion rot = toFollow.rotation;

            if( firstTime )
            {
                initialPosition = uiWorldPos;
                initialRotation = toFollow.rotation;
            }

            if( Canvas.renderMode == RenderMode.WorldSpace )
            {
                if( !followsTarget )
                {
                    uiWorldPos = initialPosition;
                    rot = initialRotation;
                }

                transform.position = uiWorldPos + Vector3.up * Offset;
                transform.rotation = rot;
                transform.localRotation *= Quaternion.Euler( 0 , 180 , 0 );
            }
            else
            {
                if( !followsTarget )
                {
                    uiWorldPos = initialPosition;
                }


                Vector2 screenPoint;

                if( Canvas.renderMode == RenderMode.ScreenSpaceCamera )
                    screenPoint = Canvas.worldCamera.WorldToScreenPoint( uiWorldPos );
                else
                    screenPoint = Cam.WorldToScreenPoint( uiWorldPos );

                Vector2 output;
                RectTransformUtility.ScreenPointToLocalPointInRectangle( transform.parent as RectTransform , screenPoint + Vector2.up * Offset , Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera , out output );
                rectTransform.anchoredPosition3D = output;
            }

            transform.SetAsLastSibling( );

            firstTime = false;
        }
    }
}
