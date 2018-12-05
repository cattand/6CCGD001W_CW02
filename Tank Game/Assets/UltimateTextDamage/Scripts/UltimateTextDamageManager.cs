using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Guirao.UltimateTextDamage
{
    [System.Serializable]
    public class TextDamageType
    {
        public string keyType;
        public int poolCount = 20;
        public UITextDamage prefab;
    }

    public class UltimateTextDamageManager : MonoBehaviour
    {
        public Camera theCamera;
        public Canvas canvas;

        public bool overlaping = true;
        public bool followsTarget = false;
        public float offsetUnits = 100; // This is if no overlaping
        public float damping = 20; // This is if no overlaping

        public List < TextDamageType > textTypes;

        private Dictionary< string , List< UITextDamage > > dTextTypes;
        private Dictionary< Transform , Queue< UITextDamage > > instancesInScreen;

        /// <summary>
        /// Start Monobehaviours, initializes the manager with the pools
        /// </summary>
        public void Start( )
        {
            if( theCamera == null )
                theCamera = Camera.main;

            // Allocate memory for our dictionaries
            instancesInScreen = new Dictionary<Transform , Queue<UITextDamage>>( );
            dTextTypes = new Dictionary<string , List<UITextDamage>>( );

            // Initialize all text types with a pool
            foreach( TextDamageType text in textTypes )
            {
                Initialize( text );
            }
        }

        /// <summary>
        /// Instantiates one text object of the desired type
        /// </summary>
        /// <param name="text">damage type</param>
        /// <returns></returns>
        UITextDamage AllocateOneInstance( TextDamageType text )
        {
            if( text == null )
                return null;

            GameObject g = Instantiate( text.prefab.gameObject ) as GameObject;
            g.transform.SetParent( transform );
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;

            UITextDamage td = g.GetComponent< UITextDamage >( );
            td.Canvas = this.canvas;
            td.Cam = theCamera;
            td.followsTarget = followsTarget;
            g.SetActive( false );

            return td;
        }

        /// <summary>
        /// Initializes a pool of objects
        /// </summary>
        /// <param name="text">damage type</param>
        void Initialize( TextDamageType text )
        {
            dTextTypes.Add( text.keyType , new List<UITextDamage>( ) );
            List< UITextDamage > container = dTextTypes[ text.keyType ];

            for( int i = 0 ; i < text.poolCount ; i++ )
            {
                container.Add( AllocateOneInstance( text ) );
            }

            // If original prefab is in the scene, disable
            if( text.prefab.gameObject.scene != null && text.prefab.gameObject.scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene( ) )
                text.prefab.gameObject.SetActive( false );
        }

        /// <summary>
        /// Shows a desired text.
        /// </summary>
        /// <param name="text">text to show as string</param>
        /// <param name="target">transform for the text position to show</param>
        /// <param name="key">key type</param>
        public void Add( string text , Transform target , string key = "default" )
        {
            // Get available text instance to use
            UITextDamage uiToUse = GetAvailableText( key );

            if( !instancesInScreen.ContainsKey( target ) )
                instancesInScreen.Add( target , new Queue<UITextDamage>( ) );

            instancesInScreen[ target ].Enqueue( uiToUse );

            // Subscribe to animation end event
            uiToUse.eventOnEnd += Label_eventOnEnd;

            // Show and set the text
            uiToUse.Show( text , target );
        }

        void LateUpdate( )
        {
            if( overlaping ) return;

            if( instancesInScreen != null )
            {
                foreach( var keypair in instancesInScreen )
                {
                    int i = keypair.Value.Count;
                    foreach( UITextDamage text in keypair.Value.ToArray( ) )
                    {
                        text.Offset = Mathf.Lerp( text.Offset , i * offsetUnits , Time.deltaTime * damping );
                        i--;
                    }
                }
            }
        }

        private void Label_eventOnEnd( UITextDamage obj )
        {
            obj.eventOnEnd -= Label_eventOnEnd;

            List<Transform> keys = new List<Transform> (instancesInScreen.Keys);
            foreach( Transform key in keys )
            {
                List< UITextDamage> aux = new List<UITextDamage>( instancesInScreen[ key ].ToArray( ) );
                aux.Remove( obj );
                instancesInScreen[ key ] = new Queue<UITextDamage>( aux );
            }
        }

        private UITextDamage GetAvailableText( string keyType )
        {
            bool ok = dTextTypes.ContainsKey( keyType );

            if( !ok )
            {
                Debug.LogError( "Text Damage -> Cannot find keyType " + keyType + " on  manager " + gameObject.name );
                return null;
            }

            List< UITextDamage > candidates = dTextTypes[ keyType ];
            for( int i = 0 ; i < candidates.Count ; i++ )
            {
                if( candidates[ i ].gameObject.activeSelf ) continue;
                candidates[ i ].label.transform.localPosition = Vector3.zero;
                candidates[ i ].transform.localScale = Vector3.one;
                return candidates[ i ];
            }

            // Instantiate new
            List< UITextDamage > container = dTextTypes[ keyType ];
            UITextDamage newInstance = AllocateOneInstance( textTypes.Find( t => t.keyType == keyType ) );
            container.Add( newInstance );

            return newInstance;
        }
    }
}