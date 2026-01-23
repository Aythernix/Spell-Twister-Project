using UnityEditor;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	#region Variables

	private float currentHealth;
	private float maxHealth;

	#endregion

	#region Unity Functions

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	#endregion

	#region Private Functions

	private bool HealthEmpty()
	{
		return currentHealth <= 0f;
	}

	private void OnHealthEmpty()
	{
		Debug.Break();
	}

	#endregion

	#region Public API

	/// <summary>
	///  Removes health from the player
	/// </summary>
	/// <param name="damage"> The amount of health to remove from the player </param>
	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		if (HealthEmpty())
		{
			OnHealthEmpty();
		}
	}

	#endregion
}