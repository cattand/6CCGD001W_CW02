using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guirao.UltimateTextDamage
{
    public class TextDamageClickerChaotic : MonoBehaviour
    {
        public UltimateTextDamageManager textManager;
        public Transform overrideTransform;

        private void OnMouseUpAsButton( )
        {
            if( Random.value < 0.4f )
                textManager.Add( ( Random.Range( 450f , 100f ) * 2 ).ToString( "0" ) , overrideTransform != null ? overrideTransform : transform , "critical" );
            else
                textManager.Add( Random.Range( 450f , 100f ).ToString( "0" ) , overrideTransform != null ? overrideTransform : transform );
        }

        public bool autoclicker = true;
        public float clickRate = 1;

        float lastTimeClick;
        private void Update( )
        {
            if( !autoclicker )
                return;

            if( Time.time - lastTimeClick >= 1f / clickRate )
            {
                lastTimeClick = Time.time;
                OnMouseUpAsButton( );
            }
        }
    }
}
