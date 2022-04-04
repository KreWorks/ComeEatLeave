using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CornController : MonoBehaviour
{
	public Action<float> OnHealthChange;
	public Action OnPlantDeath;

	public float scale = 10;
	public float maxScale = 100;
	public float growth = 0.1f;
	public AudioSource biteSound;

	protected float health;
	protected bool isDead;
	public bool IsDead { get { return isDead; } }
	protected GameManager gameManager;
	protected Renderer render;
    // Start is called before the first frame update
    void Start()
    {
		render = this.GetComponent<Renderer>();
		this.health = 100;
		this.isDead = false;
		GrowPlant();
		DrawPlant();
	}

    // Update is called once per frame
    void Update()
    {
        if (scale < maxScale)
		{
			scale += growth * Time.deltaTime;
			GrowPlant();
			DrawPlant();
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Locust")
		{
			ChangeHealth(-50);
			biteSound.PlayOneShot(biteSound.clip);
		}
	}

	protected void DrawPlant()
	{
		float currentPercent = (scale + 0.0f) / (maxScale + 0.0f);
		render.material.SetFloat("_scale",scale); // color = currentColor;
	}

	protected void GrowPlant()
	{
		transform.localScale = new Vector3(scale, scale, scale);
		this.transform.position = new Vector3(transform.position.x, GetHeightByScale(), transform.position.z);
	}

	protected float GetHeightByScale()
	{
		return scale / 100.0f;
	}

	protected void ChangeHealth(float healthChange)
	{
		if (health > 0 && !isDead)
		{
			this.health += healthChange;
			OnHealthChange.Invoke(healthChange);
		}
		else if (health <= 0 && !isDead)
		{
			DestroyPlant();
		}
	}

	protected void DestroyPlant()
	{
		isDead = true;
		OnPlantDeath.Invoke();
		foreach (Action<float> listener in OnHealthChange.GetInvocationList())
		{
			RemoveListenerOnHealthChange(listener);
		}
		foreach(Action listener in OnPlantDeath.GetInvocationList())
		{
			RemoveListenerOnPlantDeath(listener);
		}
		Destroy(this.gameObject);
		Destroy(this);
	}

	public void AddListenerOnHealthChange(Action<float> listener)
	{
		OnHealthChange += listener;
	}
	public void RemoveListenerOnHealthChange(Action<float> listener)
	{
		OnHealthChange -= listener;
	}

	public void AddListenerOnPlantDeath(Action listener)
	{
		OnPlantDeath += listener;
	}
	public void RemoveListenerOnPlantDeath(Action listener)
	{
		OnPlantDeath -= listener;
	}
}
