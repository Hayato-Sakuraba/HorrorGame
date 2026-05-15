using UnityEngine;
using UnityEngine.AI;

public class EnemyTrap : MonoBehaviour
{
	public Transform player;
	public GameObject laserObject;
	public GameObject viewObject;

	public float wakeUpDistance = 4f;

	public float chaseSpeed = 5f;

	public float viewDistance = 7f;
	public float viewAngle = 60f;

	public float searchForwardDistance = 2f;
	public float loseTime = 2f;

	private NavMeshAgent agent;

	private Vector3 lastSeenPosition;

	private float lostTimer = 0f;

	private enum State
	{
		Sleep,
		Chase,
		Search,
		Wait
	}

	private State currentState;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();

		currentState = State.Sleep;

		agent.isStopped = true;

		// 最初は非表示
		laserObject.SetActive(false);
		viewObject.SetActive(false);
	}

	void Update()
	{
		float distance = Vector3.Distance(transform.position, player.position);

		switch (currentState)
		{
			// =========================
			// 待機状態
			// =========================
			case State.Sleep:

				// 近づいたら起動
				if (distance < wakeUpDistance)
				{
					WakeUp();
				}

				break;

			// =========================
			// 追跡状態
			// =========================
			case State.Chase:

				if (CanSeePlayer())
				{
					Vector3 direction = (player.position - transform.position).normalized;
					lastSeenPosition = player.position + direction * searchForwardDistance;

					lostTimer = 0f;
				}
				else
				{
					lostTimer += Time.deltaTime;

					if (lostTimer > loseTime)
					{
						currentState = State.Wait;
					}
				}

				agent.SetDestination(lastSeenPosition);

				// 最後の位置に着いたら停止
				if (!agent.pathPending && agent.remainingDistance < 0.5f)
				{
					currentState = State.Wait;
				}

				break;

			// =========================
			// 停止状態
			// =========================
			case State.Wait:

				agent.isStopped = true;

				// OFF
				laserObject.SetActive(false);
				viewObject.SetActive(false);

				// 再起動判定
				if (distance < wakeUpDistance)
				{
					WakeUp();
				}

				break;
		}
	}

	void WakeUp()
	{
		currentState = State.Chase;

		agent.isStopped = false;

		agent.speed = chaseSpeed;

		lastSeenPosition = player.position;

		lostTimer = 0f;

		// ON
		laserObject.SetActive(true);
		viewObject.SetActive(true);

		Debug.Log("敵が起動した！");
	}

	bool CanSeePlayer()
	{
		Vector3 direction = player.position - transform.position;

		float distance = direction.magnitude;

		// 距離チェック
		if (distance > viewDistance)
			return false;

		// 角度チェック
		float angle = Vector3.Angle(transform.forward, direction);

		if (angle > viewAngle)
			return false;

		// Raycast
		Ray ray = new Ray(transform.position + Vector3.up * 0.5f, direction.normalized);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, viewDistance))
		{
			if (hit.transform == player)
			{
				return true;
			}
		}

		return false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.6f);

		Vector3 origin = transform.position + Vector3.up * 0.5f;

		Vector3 leftDir =
			Quaternion.Euler(0, -viewAngle, 0) * transform.forward;

		Vector3 rightDir =
			Quaternion.Euler(0, viewAngle, 0) * transform.forward;

		Gizmos.DrawLine(origin, origin + leftDir * viewDistance);
		Gizmos.DrawLine(origin, origin + rightDir * viewDistance);

		int segments = 30;

		Vector3 prevPoint = origin + leftDir * viewDistance;

		for (int i = 1; i <= segments; i++)
		{
			float angle =
				-viewAngle + (viewAngle * 2f) * i / segments;

			Vector3 dir =
				Quaternion.Euler(0, angle, 0) * transform.forward;

			Vector3 nextPoint =
				origin + dir * viewDistance;

			Gizmos.DrawLine(prevPoint, nextPoint);

			prevPoint = nextPoint;
		}
	}
}