using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CornController : MonoBehaviour
{
	public Action<float> OnHealthChange;

	public Gradient color;
	public Color currentColor;
	public float scale = 10;
	public float maxScale = 100;
	public float growth = 0.1f;

	protected float health;

	protected GameManager gameManager;
	protected Renderer render;
    // Start is called before the first frame update
    void Start()
    {
		render = this.GetComponent<Renderer>();
		this.health = 100;
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
			ChangeHealth(-25);
		}
	}

	protected void DrawPlant()
	{
		float currentPercent = (scale + 0.0f) / (maxScale + 0.0f);

		currentColor = color.Evaluate(currentPercent);
		//Debug.Log("color: " + render.material.color);
		render.material.color = currentColor;
	}

	protected void GrowPlant()
	{
		transform.localScale = new Vector3(scale, scale, scale);
		this.transform.position = new Vector3(transform.position.x, GetHeightByScale(), transform.position.z);
	}

	protected float GetHeightByScale()
	{
		return 0.20f + scale / 200.0f;
	}

	protected void ChangeHealth(float healthChange)
	{
		this.health += healthChange;
		Debug.Log("Megharapta egy Sáska");
		OnHealthChange.Invoke(healthChange);
	}

	public void AddListenerOnHealthChange(Action<float> listener)
	{
		OnHealthChange += listener;
	}
	public void RemoveListenerOnHealthChange(Action<float> listener)
	{
		OnHealthChange -= listener;
	}
}
