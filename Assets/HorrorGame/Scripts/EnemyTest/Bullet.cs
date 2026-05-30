using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 8f;
	public float lifeTime = 3f;

	private Vector2 direction;
	private Rigidbody2D rb;

	public void Init(Vector2 dir)
	{
		direction = dir.normalized;
		Destroy(gameObject, lifeTime);
	}

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			Debug.Log("弾がプレイヤーに命中！");
			Destroy(gameObject);
			return;
		}

		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			Debug.Log("弾が壁に衝突して消滅");
			Destroy(gameObject);
		}
	}
}
