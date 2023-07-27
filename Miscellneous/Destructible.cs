
using UnityEngine;

public class Destructible : MonoBehaviour {

	public GameObject destroyedVersion;	// Reference to the shattered version of the object

	public void DestroyObject()
	{
		// Spawn a shattered object
	    Instantiate(destroyedVersion, transform.position, transform.rotation);
		// Remove the current object
		Destroy(gameObject);
		GameManager.Instance.SpawnRandomCrateItem(transform);
		
	}

}
