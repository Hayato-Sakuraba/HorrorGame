using UnityEngine;
using UnityEngine.AI;

public class Enemy2D : MonoBehaviour
{
	[Header("References")]
	public Transform[] patrolPoints;
	private Transform target;

	[Header("Movement")]
	public float walkSpeed = 1.5f;
	public float runSpeed = 3.5f;

	[Header("Vision Settings")]
	public float viewDistance = 6f;      // 視界距離
	public float viewAngle = 60f;        // 視野角（左右30°）
	public LayerMask obstacleMask;       // 壁などの障害物レイヤー

	[Header("Chase Settings")]
	public float chaseKeepTime = 2f;
	public float lostWaitTime = 1.5f; // ← 見失った後の待機時間
	private float chaseTimer = 0f;
	private float waitTimer = 0f;

	public LaserPointer laser;

	private NavMeshAgent agent;
	private int currentPatrolIndex = 0;

	private enum State { Patrol, Chase, Wait }
	private State state = State.Patrol;
	private Vector2 lastMoveDir = Vector2.right; // 初期値は右

	private void OnEnable()
	{
		TrapEvent.OtonarashiTrapActivated += GetPlayerPosition;
	}

	private void OnDisable()
	{
		TrapEvent.OtonarashiTrapActivated -= GetPlayerPosition;
	}

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

		target = GameObject.FindWithTag("Player").transform;  

		agent.speed = walkSpeed;
		GoToNextPatrolPoint();
	}

	void Update()
	{
		bool canSeePlayer = CanSeePlayer();

		switch (state)
		{
			case State.Patrol:
				PatrolUpdate(canSeePlayer);
				break;

			case State.Chase:
				ChaseUpdate(canSeePlayer);
				break;

			case State.Wait:
				WaitUpdate();
				break;
		}
	}

	// -------------------------
	// パトロール状態
	// -------------------------
	void PatrolUpdate(bool canSeePlayer)
	{
		laser.SetActive(false);
		// プレイヤーを見つけたら追跡へ
		if (canSeePlayer)
		{
			state = State.Chase;
			agent.speed = runSpeed;
			chaseTimer = chaseKeepTime;
			return;
		}

		// パトロールポイントに到達したら次へ
		if (!agent.pathPending && agent.remainingDistance < 0.2f)
		{
			GoToNextPatrolPoint();
		}
	}

	// -------------------------
	// 追跡状態
	// -------------------------
	void ChaseUpdate(bool canSeePlayer)
	{
		laser.SetActive(canSeePlayer);
		if (canSeePlayer)
		{
			chaseTimer = chaseKeepTime;
		}
		else
		{
			chaseTimer -= Time.deltaTime;

			if (chaseTimer <= 0f)
			{
				// ★ Wait 状態へ
				state = State.Wait;
				waitTimer = lostWaitTime;
				agent.speed = 0f; // 立ち止まる
				agent.ResetPath();
				return;
			}
		}

		agent.SetDestination(target.position);
	}

	private void GetPlayerPosition(Transform playerPos)
	{
		target = playerPos;
		state = State.Chase;
		agent.SetDestination(target.position);
	}

	void GoToNearestPatrolPoint()
	{
		if (patrolPoints.Length == 0) return;

		float bestDist = Mathf.Infinity;
		int bestIndex = 0;

		Vector2 pos = transform.position;

		for (int i = 0; i < patrolPoints.Length; i++)
		{
			float d = Vector2.Distance(pos, patrolPoints[i].position);
			if (d < bestDist)
			{
				bestDist = d;
				bestIndex = i;
			}
		}

		currentPatrolIndex = bestIndex;
		agent.SetDestination(patrolPoints[currentPatrolIndex].position);
	}



	void WaitUpdate()
	{
		laser.SetActive(false);
		waitTimer -= Time.deltaTime;

		if (waitTimer <= 0f)
		{
			state = State.Patrol;
			agent.speed = walkSpeed;

			// ★ 最寄りのパトロールポイントへ向かう
			GoToNearestPatrolPoint();
		}
	}

	// -------------------------
	// 視界判定
	// -------------------------
	bool CanSeePlayer()
	{
		// ★ target が null の場合は見えていない扱いにする
		if (target == null)
			return false;

		// 現在の移動方向（停止中なら lastMoveDir を使う）
		Vector2 moveDir = agent.velocity.sqrMagnitude > 0.01f
			? agent.velocity.normalized
			: lastMoveDir;

		if (agent.velocity.sqrMagnitude > 0.01f)
			lastMoveDir = moveDir;

		Vector2 pos = transform.position;
		Vector2 playerPos = target.position;
		Vector2 dirToPlayer = (playerPos - pos).normalized;

		float distanceToPlayer = Vector2.Distance(pos, playerPos);
		if (distanceToPlayer > viewDistance)
			return false;

		float angle = Vector2.Angle(moveDir, dirToPlayer);
		if (angle > viewAngle * 0.5f)
			return false;

		RaycastHit2D hit = Physics2D.Raycast(pos, dirToPlayer, viewDistance, obstacleMask);
		if (hit && hit.collider.transform != target)
			return false;

		return true;
	}


	// -------------------------
	// パトロール地点へ移動
	// -------------------------
	void GoToNextPatrolPoint()
	{
		if (patrolPoints.Length == 0) return;

		agent.SetDestination(patrolPoints[currentPatrolIndex].position);
		currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
	}

	// -------------------------
	// Gizmo（視界の可視化）
	// -------------------------
	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;

		// 現在の向き（停止中は lastMoveDir）
		Vector2 moveDir = agent.velocity.sqrMagnitude > 0.01f
			? agent.velocity.normalized
			: lastMoveDir;

		Gizmos.color = new Color(1f, 1f, 0f, 0.25f); // 半透明の黄色

		int segments = 30; // 扇形の分割数
		float halfAngle = viewAngle * 0.5f;

		Vector3 origin = transform.position;

		// 扇形を描く
		for (int i = 0; i < segments; i++)
		{
			float angle1 = -halfAngle + (viewAngle / segments) * i;
			float angle2 = -halfAngle + (viewAngle / segments) * (i + 1);

			Vector3 dir1 = Quaternion.Euler(0, 0, angle1) * moveDir;
			Vector3 dir2 = Quaternion.Euler(0, 0, angle2) * moveDir;

			Vector3 p1 = origin + dir1 * viewDistance;
			Vector3 p2 = origin + dir2 * viewDistance;

			Gizmos.DrawLine(origin, p1);
			Gizmos.DrawLine(p1, p2);
		}

		// 中央方向の赤線
		Gizmos.color = Color.red;
		Gizmos.DrawLine(origin, origin + (Vector3)moveDir * viewDistance);
	}

}