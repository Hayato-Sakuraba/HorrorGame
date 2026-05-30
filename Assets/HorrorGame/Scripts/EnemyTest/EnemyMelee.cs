using UnityEngine;
using System.Collections;

public class EnemyMelee : MonoBehaviour
{
	public Enemy2D enemy;
	public float attackRange = 1.2f;
	public float attackInterval = 1.0f;

	public float preAttackDelay = 0.2f;
	public float attackActiveTime = 0.2f;
	public float postAttackStun = 0.25f;

	public MeleeHitbox hitbox;

	public SpriteRenderer spriteRenderer;
	public Sprite normalSprite;
	public Sprite attackSprite;

	private float attackTimer = 0f;
	private bool isAttacking = false;

	void Update()
	{
		if (enemy == null || enemy.Target == null) return;
		if (isAttacking) return;

		if (enemy.IsChasingPlayerStable)
		{
			attackTimer += Time.deltaTime;

			float dist = Vector2.Distance(transform.position, enemy.Target.position);

			if (dist <= attackRange && attackTimer >= attackInterval)
			{
				StartCoroutine(AttackRoutine());
				attackTimer = 0f;
			}
		}
		else attackTimer = 0f;
	}

	IEnumerator AttackRoutine()
	{
		isAttacking = true;

		// ★ 攻撃中は Enemy2D の AI を完全停止
		enemy.IsExternalControl = true;

		// ▼ 前隙
		spriteRenderer.sprite = normalSprite;
		hitbox.DisableHitbox();
		yield return new WaitForSeconds(preAttackDelay);

		// ▼ 攻撃開始
		spriteRenderer.sprite = attackSprite;
		hitbox.EnableHitbox();
		yield return new WaitForSeconds(attackActiveTime);

		// ★ 攻撃判定はここで必ず終了
		hitbox.DisableHitbox();

		// ▼ 後隙（攻撃スプライトのままでもOK）
		yield return new WaitForSeconds(postAttackStun);

		// ▼ 復帰
		spriteRenderer.sprite = normalSprite;
		enemy.IsExternalControl = false;
		isAttacking = false;
	}
}
