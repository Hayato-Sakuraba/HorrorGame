using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{
	public Enemy2D enemy;            // Enemy2D を参照
	public GameObject bulletPrefab;
	public Transform firePoint;

	public float shootInterval = 1.2f;
	private float shootTimer = 0f;

	void Update()
	{
		// Enemy2D が Chase 状態で、かつプレイヤーが視界に入っている時だけ射撃
		if (enemy.IsChasingPlayerStable)
		{
			shootTimer += Time.deltaTime;
			if (shootTimer >= shootInterval)
			{
				Shoot();
				shootTimer = 0f;
			}
		}
		else
		{
			// 視界外ならタイマーをリセット
			shootTimer = 0f;
		}
	}

	void Shoot()
	{
		if (bulletPrefab == null || firePoint == null) return;

		Vector2 dir = (enemy.Target.position - firePoint.position).normalized;

		GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

		var bullet = b.GetComponent<Bullet>();
		if (bullet != null)
		{
			bullet.Init(dir);
		}
	}
}