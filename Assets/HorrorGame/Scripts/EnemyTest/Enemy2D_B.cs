using UnityEngine;

public class Enemy2D_B : MonoBehaviour
{
	public Enemy2D enemy;

	[Header("Sprite")]
	public SpriteRenderer bodySprite;

	public Sprite frontSprite;   // 正面（上）
	public Sprite backSprite;    // 背面（下）
	public Sprite sideSprite;    // 横（左右反転で対応）

	void Update()
	{
		if (enemy == null || enemy.spriteRoot == null) return;

		UpdateDirectionSprite();
	}

	void UpdateDirectionSprite()
	{
		// 敵の向き
		Vector2 dir = enemy.spriteRoot.up;

		float absX = Mathf.Abs(dir.x);
		float absY = Mathf.Abs(dir.y);

		// -------------------------
		// 横向き（X が大きい）
		// -------------------------
		if (absX > absY)
		{
			bodySprite.sprite = sideSprite;

			// 右向き
			if (dir.x > 0)
				bodySprite.flipX = false;
			else
				bodySprite.flipX = true;

			return;
		}

		// -------------------------
		// 縦向き（Y が大きい）
		// -------------------------

		// 上向き
		if (dir.y > 0)
		{
			bodySprite.sprite = frontSprite;
			bodySprite.flipX = false;
		}
		else
		{
			// 下向き
			bodySprite.sprite = backSprite;
			bodySprite.flipX = false;
		}
	}
}
