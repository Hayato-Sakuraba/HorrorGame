using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
	public Transform[] patrolPoints; // 巡回ポイント
	public Transform target;         // プレイヤー
	public LaserPointer laser;

	public float chaseDistance = 5f;

	public float patrolSpeed = 2f;
	public float chaseSpeed = 4f;

	private NavMeshAgent agent;
	private int currentPoint = 0;

	public float viewDistance = 7f;
	public float viewAngle = 60f;

	private enum State
	{
		Patrol,
		Chase
	}

	private State currentState;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();

		currentState = State.Patrol;
		agent.speed = patrolSpeed;

		GoToNextPoint();
	}

	private float lostTimer = 0f;
	public float loseTime = 2f;

	void Update()
	{
		if (CanSeePlayer())
		{
			currentState = State.Chase;
			lostTimer = 0f;
		}
		else
		{
			lostTimer += Time.deltaTime;

			if (lostTimer > loseTime)
			{
				currentState = State.Patrol;
			}
		}

		switch (currentState)
		{
			case State.Patrol:
				Patrol();
				break;

			case State.Chase:
				Chase();
				break;
		}
	}
	bool CanSeePlayer()
	{
		Vector3 direction = target.position - transform.position;
		float distance = direction.magnitude;

		// ① 距離チェック
		if (distance > viewDistance)
			return false;

		// ② 角度チェック
		float angle = Vector3.Angle(transform.forward, direction);
		if (angle > viewAngle)
			return false;

		// ③ 壁チェック（Raycast）
		Ray ray = new Ray(transform.position + Vector3.up * 0.5f, direction.normalized);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, viewDistance, ~0))
		{
			// 最初に当たったのがプレイヤーならOK
			if (hit.transform == target)
			{
				return true;
			}
		}
		Debug.DrawRay(transform.position + Vector3.up * 0.5f, direction.normalized * viewDistance, Color.red);
		return false;
	}
	void Patrol()
	{
		agent.speed = patrolSpeed;

		laser.ShowLaser(false);

		if (!agent.pathPending && agent.remainingDistance < 0.5f)
		{
			GoToNextPoint();
		}
	}

	void Chase()
	{
		agent.speed = chaseSpeed;
		agent.SetDestination(target.position);

		laser.ShowLaser(true);
	}

	void GoToNextPoint()
	{
		if (patrolPoints.Length == 0) return;

		agent.destination = patrolPoints[currentPoint].position;

		currentPoint = (currentPoint + 1) % patrolPoints.Length;
	}
	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.7f, 1, 0, 0.7f);

		Vector3 origin = transform.position + Vector3.up * 0.5f;

		// 扇の左右の方向を計算
		Vector3 leftDir = Quaternion.Euler(0, -viewAngle, 0) * transform.forward;
		Vector3 rightDir = Quaternion.Euler(0, viewAngle, 0) * transform.forward;

		// 扇の線
		Gizmos.DrawLine(origin, origin + leftDir * viewDistance);
		Gizmos.DrawLine(origin, origin + rightDir * viewDistance);

		// 扇の弧を細かく描く
		int segments = 30; // 分割数（増やすと滑らか）
		Vector3 prevPoint = origin + leftDir * viewDistance;

		for (int i = 1; i <= segments; i++)
		{
			float angle = -viewAngle + (viewAngle * 2f) * i / segments;
			Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;
			Vector3 nextPoint = origin + dir * viewDistance;

			Gizmos.DrawLine(prevPoint, nextPoint);
			prevPoint = nextPoint;
		}
	}
}