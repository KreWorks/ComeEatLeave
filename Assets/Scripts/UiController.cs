using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
	public Slider healthBar;
	float maxHealth;
	float health;

	public void SetMaxHealth(float maxHealth)
	{
		this.maxHealth = maxHealth;
		this.SetHealth(maxHealth);
	}

	public void SetHealth(float newHealth)
	{
		this.health = newHealth;

		float healthValue = health * healthBar.maxValue / maxHealth;
		Debug.Log("maxHealth: " + maxHealth + " health: " + newHealth + " healthValue: " + healthValue);
		healthBar.value = healthValue;
	}

	public void ChangeHealth(float change)
	{
		health += change;
		SetHealth(health);
	}
}
