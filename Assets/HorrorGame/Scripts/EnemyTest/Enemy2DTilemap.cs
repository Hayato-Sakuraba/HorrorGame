using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy2DTilemap : MonoBehaviour
{
	private enum State
	{
		Patrol,
		Chase,
		Return
	}

	[Header("参照")]
	public Transform player;
	public Tilemap wallTilemap;

	[Header("巡回")]
	public Transform[] patrolPoints;
	public float patrolSpeed = 2f;
	public float waitTime = 1f;

	[Header("追跡")]
	public float chaseSpeed = 4f;
	public float loseTime = 2f;

	[Header("視界")]
	public float viewDistance = 5f;
	[Range(0, 360)]
	public float viewAngle = 90f;

	private State currentState;

	private int currentPatrolIndex = 0;
	private bool justReturnedToPatrol = false;

	private bool isWaiting = false;
	private float waitCounter = 0f;

	private float lostTimer = 0f;

	private Vector3 lastKnownPosition;

	void Start()
	{
		currentState = State.Patrol;
	}

	void Update()
	{
		UpdateVision();

		switch (currentState)
		{
			case State.Patrol:
				Patrol();
				break;

			case State.Chase:
				Chase();
				break;

			case State.Return:
				ReturnToPatrol();
				break;
		}
	}

	//========================
	// 視界判定
	//========================
	void UpdateVision()
	{
		if (CanSeePlayer())
		{
			currentState = State.Chase;

			lastKnownPosition = player.position;

			lostTimer = loseTime;
		}
		else
		{
			if (currentState == State.Chase)
			{
				lostTimer -= Time.deltaTime;

				if (lostTimer <= 0f)
				{
					currentPatrolIndex =
						GetNearestPatrolPoint();

					currentState = State.Return;
				}
			}
		}
	}

	bool CanSeePlayer()
	{
		Vector2 origin = transform.position;
		Vector2 target = player.position;

		Vector2 dir = target - origin;

		float distance = dir.magnitude;

		// 距離
		if (distance > viewDistance)
			return false;

		dir.Normalize();

		// 角度
		float angle = Vector2.Angle(transform.up, dir);

		if (angle > viewAngle / 2f)
			return false;

		// 壁チェック
		float step = 0.2f;

		for (float i = 0; i < distance; i += step)
		{
			Vector2 checkPos = origin + dir * i;

			Vector3Int cell =
				wallTilemap.WorldToCell(checkPos);

			if (wallTilemap.HasTile(cell))
			{
				Debug.DrawRay(origin, dir * distance, Color.red);
				return false;
			}
		}

		Debug.DrawRay(origin, dir * distance, Color.green);

		return true;
	}
	int GetNearestPatrolPoint()
	{
		int nearestIndex = 0;

		float nearestDistance = Mathf.Infinity;

		for (int i = 0; i < patrolPoints.Length; i++)
		{
			float distance =
				Vector3.Distance(
					transform.position,
					patrolPoints[i].position
				);

			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestIndex = i;
			}
		}

		return nearestIndex;
	}
	//========================
	// 巡回
	//========================
	void Patrol()
	{
		if (patrolPoints.Length == 0)
			return;

		if (isWaiting)
		{
			waitCounter -= Time.deltaTime;

			if (waitCounter <= 0f)
			{
				isWaiting = false;

				if (justReturnedToPatrol)
				{
					justReturnedToPatrol = false;
				}
				else
				{
					currentPatrolIndex =
						(currentPatrolIndex + 1)
						% patrolPoints.Length;
				}
			}

			return;
		}

		Vector3 target =
			patrolPoints[currentPatrolIndex].position;

		MoveTo(target, patrolSpeed);

		if (Vector3.Distance(transform.position, target) < 0.1f)
		{
			isWaiting = true;
			waitCounter = waitTime;
		}
	}

	//========================
	// 追跡
	//========================
	void Chase()
	{
		MoveTo(lastKnownPosition, chaseSpeed);
	}

	//========================
	// 巡回復帰
	//========================
	void ReturnToPatrol()
	{
		Vector3 target =
			patrolPoints[currentPatrolIndex].position;

		MoveTo(target, patrolSpeed);

		if (Vector3.Distance(transform.position, target) < 0.1f)
		{
			currentState = State.Patrol;

			isWaiting = true;
			waitCounter = waitTime;

			// ←追加
			justReturnedToPatrol = true;
		}
	}

	//========================
	// 移動
	//========================
	void MoveTo(Vector3 target, float speed)
	{
		target.z = transform.position.z;

		Vector3 nextPos =
			Vector3.MoveTowards(
				transform.position,
				target,
				speed * Time.deltaTime
			);

		// タイル壁判定
		Vector3Int cell =
			wallTilemap.WorldToCell(nextPos);

		if (!wallTilemap.HasTile(cell))
		{
			transform.position = nextPos;
		}

		// 向き
		Vector3 dir = target - transform.position;

		if (dir.magnitude > 0.05f)
		{
			float angle =
				Mathf.Atan2(dir.y, dir.x)
				* Mathf.Rad2Deg - 90f;

			Quaternion rot =
				Quaternion.Euler(0, 0, angle);

			transform.rotation =
				Quaternion.Slerp(
					transform.rotation,
					rot,
					6f * Time.deltaTime
				);
		}
	}

	//========================
	// Gizmo
	//========================
	void OnDrawGizmos()
	{
		Vector3 origin = transform.position;

		Vector3 forward = transform.up;

		Gizmos.color = Color.yellow;

		Vector3 left =
			Quaternion.Euler(0, 0, viewAngle / 2f)
			* forward;

		Vector3 right =
			Quaternion.Euler(0, 0, -viewAngle / 2f)
			* forward;

		Gizmos.DrawLine(
			origin,
			origin + left * viewDistance
		);

		Gizmos.DrawLine(
			origin,
			origin + right * viewDistance
		);

		Gizmos.color = Color.red;

		Gizmos.DrawLine(
			origin,
			origin + forward * viewDistance
		);
	}
}