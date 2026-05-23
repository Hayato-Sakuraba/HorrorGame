using UnityEngine;

public class LaserPointer : MonoBehaviour
{
	public float maxDistance = 10f;
	public float laserWidth = 0.05f;
	public LayerMask obstacleMask;
	public Transform playerRoot;

	private Transform enemy;
	private SpriteRenderer playerSr;
	private SpriteRenderer sr;

	private bool isActive = false; // ← 追加：レーザーON/OFF

	void Awake()
	{
		enemy = transform.parent;
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = false; // ← 初期は非表示

		playerSr = playerRoot.GetComponentInChildren<SpriteRenderer>();
	}

	// ← 追加：外部からレーザーのON/OFFを切り替える
	public void SetActive(bool active)
	{
		isActive = active;
		sr.enabled = active;
	}

	Vector3 GetPlayerCenter()
	{
		return playerSr.bounds.center;
	}

	void Update()
	{
		if (!isActive) return; // ← OFF のときは処理しない

		transform.position = enemy.position;

		Vector3 playerCenter = GetPlayerCenter();
		Vector2 dir = (playerCenter - enemy.position).normalized;
		transform.up = dir;

		RaycastHit2D hit = Physics2D.Raycast(enemy.position, dir, maxDistance, obstacleMask);

		float dist;
		if (hit)
		{
			dist = hit.distance;
		}
		else
		{
			dist = Vector2.Distance(enemy.position, playerCenter);
			dist = Mathf.Min(dist, maxDistance);
		}

		transform.localScale = new Vector3(laserWidth, dist, 1f);
		transform.position += transform.up * (dist * 0.5f);
	}
}
