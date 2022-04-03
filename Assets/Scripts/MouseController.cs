using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
	public GameObject net;
	public LayerMask mouseLayer;
	public GameManager gameManager;

    void Start()
    {
		net.SetActive(false);
	}

    void Update()
    {
		if (Input.GetMouseButton(0))
		{
			net.SetActive(true);

			Ray ray;
			RaycastHit hitData;
			Vector3 worldPosition;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitData, 100, mouseLayer))
			{
				worldPosition = hitData.point;
				this.transform.position = worldPosition;
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			net.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Locust")
		{
			gameManager.DecreaseLocust();
			gameManager.flock.RemoveFlockUnit(other.GetComponent<FlockUnit>());
			Destroy(other.gameObject.GetComponent<FlockUnit>());
			Destroy(other.gameObject);
		}
		
	}

}
