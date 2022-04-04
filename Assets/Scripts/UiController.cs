using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
	public Slider healthBar;
	public TMP_Text cornCounter;
	public TMP_Text locustCounter;
	public TMP_Text waveText;
	public TMP_Text endText;
	public Image locustImage;
	public GameObject endPanel;
	float maxHealth;
	float health;
	int waveCount = 1;

	public void SetMaxHealth(float maxHealth)
	{
		this.maxHealth = maxHealth;
		this.SetHealth(maxHealth);
		endPanel.SetActive(false);
	}

	public void SetHealth(float newHealth)
	{
		this.health = newHealth;

		float healthValue = health * healthBar.maxValue / maxHealth;
		healthBar.value = healthValue;
	}

	public void ChangeHealth(float change)
	{
		health += change;
		SetHealth(health);
	}

	public void SetLocustTimer(float time)
	{
		if (locustImage.gameObject.activeSelf)
		{
			locustImage.gameObject.SetActive(false);
			waveText.gameObject.SetActive(true);
		}

		string timeText = Mathf.FloorToInt(time).ToString();
		timeText += (time - Mathf.FloorToInt(time)).ToString().Substring(1, 3);

		locustCounter.text = timeText;
		string counterText = waveCount == 1 ? "1st" : (waveCount == 2 ? "2nd" : (waveCount == 3 ? "3rd" : waveCount.ToString()+"th"));
		waveText.text = "The " + counterText + " wave is coming in " +Mathf.FloorToInt(time).ToString();
	}

	public void SetLocustCounter(int locustCount)
	{
		if (locustCount == 0)
		{
			waveCount++;
		}
		if (!locustImage.gameObject.activeSelf)
		{
			locustImage.gameObject.SetActive(true);
			if (waveCount == 1)
			{
				waveText.text = "Click your mouse and catch as many as you can!";
			}
			else
			{
				waveText.gameObject.SetActive(false);
			}	
		}
		locustCounter.text = locustCount.ToString();
	}

	public void SetCornCounter(int cornCount)
	{
		cornCounter.text = cornCount.ToString();
	}

	public void EndPanel()
	{
		endText.text = "Sadly the locusts ate all of your crops.You fought brave and survived " + waveCount.ToString() + " waves.";
		endPanel.SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
