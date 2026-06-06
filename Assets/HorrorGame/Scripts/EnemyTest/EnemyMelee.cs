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

	[Header("Sprites")]
	public SpriteRenderer bodySprite;
	public Sprite idleSprite;
	public Sprite preAttackSprite;
	public Sprite attackSprite;
	public Sprite postAttackSprite;

	[Header("Hitbox Visual")]
	public SpriteRenderer hitboxSprite;
	public float forwardOffset = 0.7f;

	private bool isAttacking = false;
	private float cooldownTimer = 0f;

	void Start()
	{
		if (hitboxSprite != null)
			hitboxSprite.enabled = false;

		bodySprite.sprite = idleSprite;
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

		// 敵の移動を完全停止
		float totalStop = preDelay + attackDuration + postDelay;
		enemy.StopMovementFor(totalStop);

		// --- 前隙 ---
		bodySprite.sprite = preAttackSprite;
		yield return new WaitForSeconds(preDelay);

		// --- 攻撃 ---
		bodySprite.sprite = attackSprite;
		OnAttackStart();
		yield return new WaitForSeconds(attackDuration);
		OnAttackEnd();

		// --- 後隙 ---
		bodySprite.sprite = postAttackSprite;
		yield return new WaitForSeconds(postDelay);

		// Idle に戻す
		bodySprite.sprite = idleSprite;

		isAttacking = false;
	}

	public void OnAttackStart()
	{
		if (hitboxSprite != null)
			hitboxSprite.enabled = true;

		// ★ 敵の向きは spriteRoot.up を使う
		Vector3 forward = enemy.spriteRoot.up;

		hitboxSprite.transform.position =
			transform.position + forward * forwardOffset;

		Attack();
	}

	public void OnAttackEnd()
	{
		if (hitboxSprite != null)
			hitboxSprite.enabled = false;
	}

	private void Attack()
	{
		float radius = hitboxSprite.bounds.size.x > 0
			? hitboxSprite.bounds.size.x * 0.5f
			: hitboxSprite.transform.lossyScale.x * 0.5f;

		Vector3 hitPos = hitboxSprite.transform.position;

		Collider2D hit = Physics2D.OverlapCircle(hitPos, radius);

		if (hit != null && hit.CompareTag("Player"))
		{
			Debug.Log("プレイヤーに命中！");
		}
	}

	private void OnDrawGizmos()
	{
		if (hitboxSprite == null) return;

		float radius = hitboxSprite.bounds.size.x > 0
			? hitboxSprite.bounds.size.x * 0.5f
			: hitboxSprite.transform.lossyScale.x * 0.5f;

		Vector3 hitPos = hitboxSprite.transform.position;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(hitPos, radius);
	}
}
