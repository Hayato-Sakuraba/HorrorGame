using UnityEngine;
using System.Collections;

public class EnemyMelee : MonoBehaviour
{
	public Enemy2D enemy;

	[Header("Attack Settings")]
	public float attackRange = 1.2f;
	public float preDelay = 0.3f;
	public float attackDuration = 0.1f;
	public float postDelay = 0.4f;
	public float attackCooldown = 1.0f;

	[Header("Hitbox Visual (あなたの白い円スプライト)")]
	public SpriteRenderer hitboxSprite;
	public float forwardOffset = 0.7f;
	public int playerLayer = 8;

	private bool isAttacking = false;
	private float cooldownTimer = 0f;

	void Start()
	{
		if (hitboxSprite != null)
			hitboxSprite.enabled = false;
	}

	void Update()
	{
		if (enemy == null || enemy.Target == null) return;

		enemy.IsMovementStopped = isAttacking;

		if (!enemy.IsChasingPlayerStable)
		{
			cooldownTimer = 0f;
			return;
		}

		cooldownTimer += Time.deltaTime;

		if (isAttacking) return;

		float dist = Vector2.Distance(transform.position, enemy.Target.position);

		if (dist <= attackRange && cooldownTimer >= attackCooldown)
		{
			StartCoroutine(AttackRoutine());
		}
	}

	private IEnumerator AttackRoutine()
	{
		isAttacking = true;
		cooldownTimer = 0f;

		// --- 前隙 ---
		yield return new WaitForSeconds(preDelay);

		// --- 攻撃 ---
		if (hitboxSprite != null)
			hitboxSprite.enabled = true; // ← 位置は絶対に触らない

		Attack();
		yield return new WaitForSeconds(attackDuration);

		// --- 後隙 ---
		if (hitboxSprite != null)
			hitboxSprite.enabled = false;

		yield return new WaitForSeconds(postDelay);

		isAttacking = false;
	}

	private void Attack()
	{
		float radius = hitboxSprite.bounds.size.x * 0.5f;

		Vector3 hitPos = hitboxSprite.transform.position; // ← これが正しい

		int layerMask = 1 << playerLayer;

		Collider2D hit = Physics2D.OverlapCircle(hitPos, radius, layerMask);

		if (hit != null)
		{
			Debug.Log("プレイヤーに命中！");
		}
	}

	private void OnDrawGizmos()
	{
		if (hitboxSprite == null) return;

		float radius = hitboxSprite.bounds.size.x * 0.5f;
		Vector3 hitPos = hitboxSprite.transform.position;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(hitPos, radius);
	}
}
