using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
	private Collider2D col;

	void Awake()
	{
		col = GetComponent<Collider2D>();
		col.enabled = false; // 普段は無効
	}

	public void EnableHitbox()
	{
		col.enabled = true;
	}

	public void DisableHitbox()
	{
		col.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("近接攻撃命中！");

			// ダメージ処理を入れるならここ
			// other.GetComponent<PlayerHealth>()?.TakeDamage(1);
		}
	}
}
