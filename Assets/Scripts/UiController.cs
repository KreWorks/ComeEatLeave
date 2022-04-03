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
	public Image locustImage;
	public GameObject endPanel;
	float maxHealth;
	float health;

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
		waveText.text = "A wave is coming in " +Mathf.FloorToInt(time).ToString();
	}

	public void SetLocustCounter(int locustCount)
	{
		if (!locustImage.gameObject.activeSelf)
		{
			locustImage.gameObject.SetActive(true);
			waveText.gameObject.SetActive(false);
		}
		locustCounter.text = locustCount.ToString();
	}

	public void SetCornCounter(int cornCount)
	{
		cornCounter.text = cornCount.ToString();
	}

	public void EndPanel()
	{
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
