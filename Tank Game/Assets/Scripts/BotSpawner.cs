using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace TanksMP
{
    /// <summary>
    /// Responsible for spawning AI bots when in offline mode, otherwise gets disabled.
    /// </summary>
    /// 

	public class BotSpawner : MonoBehaviour
	{
		/// <summary>
		/// Amount of bots to spawn across all teams.
		/// </summary>
		public int maxBots;
        
		/// <summary>
		/// Selection of bot prefabs to choose from.
		/// </summary>
		public GameObject[] prefabs;

		IEnumerator Start ()
		{
			//wait a second for all script to initialize
			yield return new WaitForSeconds (0.25f);

			//loop over bot count
			for (int i = 0; i < maxBots; i++)
			{
				//randomly choose bot from array of bot prefabs
				int randIndex = Random.Range (0, prefabs.Length);
				GameObject obj = (GameObject)GameObject.Instantiate (prefabs [randIndex], Vector3.zero, Quaternion.identity);

				//let the local host determine the team assignment
				SinglePlayer p = obj.GetComponent<SinglePlayer> ();
				p.teamIndex = GameManagerSinglePlayer.GetInstance ().GetTeamFill ();
                
				//increase corresponding team size
				GameManagerSinglePlayer.GetInstance ().size [p.teamIndex]++;
				GameManagerSinglePlayer.GetInstance ().ui.OnTeamSizeChanged (p.teamIndex);
                
				yield return new WaitForSeconds (0.25f);
			}
		}
	}
}