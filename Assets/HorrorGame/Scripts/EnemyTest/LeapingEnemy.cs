using UnityEngine;
using UnityEngine.AI;

public class LeapingEnemy: MonoBehaviour
{
	[Header("References")]
	public Transform[] patrolPoints;
	private Transform target;

	[Header("Movement")]
	public float walkSpeed = 1.5f;
	public float runSpeed = 3.5f;

	[Header("Vision Settings")]
	public float viewDistance = 6f;
	public float viewAngle = 60f;
	public LayerMask obstacleMask;

	[Header("Chase Settings")]
	public float chaseKeepTime = 2f;
	public float lostWaitTime = 1.5f;
	private float chaseTimer = 0f;
	private float waitTimer = 0f;

	[Header("Leap Attack")]
	public float leapSpeed = 12f;
	public float leapDuration = 0.35f;
	public float leapCooldown = 3f;
	public float leapRange = 5f;

	[Header("Timing")]
	public float preLeapDelay = 0.15f;
	public float postLeapStun = 0.25f;

	private float leapTimer = 0f;
	private Vector2 leapDirection;

	[Header("Visual")]
	public Transform spriteRoot;
	public LaserPointer laser;

	private NavMeshAgent agent;
	private int currentPatrolIndex = 0;

	private enum State { Patrol, Chase, Wait, Leap }
	private State state = State.Patrol;

	private Vector2 lastMoveDir = Vector2.right;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

		target = GameObject.FindWithTag("Player").transform;

		agent.speed = walkSpeed;
		agent.acceleration = 999f;
		agent.angularSpeed = 999f;

		GoToNextPatrolPoint();
	}

	void Update()
	{
		leapTimer += Time.deltaTime;

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

			case State.Leap:
				LeapUpdate();
				break;
		}

		UpdateSpriteRotation();
	}

	// -------------------------
	// パトロール
	// -------------------------
	void PatrolUpdate(bool canSeePlayer)
	{
		laser.SetActive(false);

		if (canSeePlayer)
		{
			state = State.Chase;
			agent.speed = runSpeed;
			chaseTimer = chaseKeepTime;
			return;
		}

		if (!agent.pathPending && agent.remainingDistance < 0.2f)
		{
			GoToNextPatrolPoint();
		}
	}

	// -------------------------
	// 追跡
	// -------------------------
	void ChaseUpdate(bool canSeePlayer)
	{
		float dist = Vector2.Distance(transform.position, target.position);

		laser.SetActive(canSeePlayer);

		// 見えている間は追跡継続
		if (canSeePlayer)
		{
			chaseTimer = chaseKeepTime;

			// ★ 飛びかかり条件
			if (dist < leapRange && leapTimer >= leapCooldown)
			{
				StartCoroutine(LeapAttack());
				return;
			}
		}
		else
		{
			chaseTimer -= Time.deltaTime;

			if (chaseTimer <= 0f)
			{
				state = State.Wait;
				waitTimer = lostWaitTime;
				agent.speed = 0f;
				agent.ResetPath();
				return;
			}
		}

		agent.SetDestination(target.position);
	}

	// -------------------------
	// 飛びかかり中
	// -------------------------
	void LeapUpdate()
	{
		// 固定方向へ直線移動
		Vector3 targetPos = (Vector2)transform.position + leapDirection * 10f;
		agent.SetDestination(targetPos);
	}

	// -------------------------
	// 見失い後の待機
	// -------------------------
	void WaitUpdate()
	{
		laser.SetActive(false);

		waitTimer -= Time.deltaTime;

		if (waitTimer <= 0f)
		{
			state = State.Patrol;
			agent.speed = walkSpeed;
			GoToNearestPatrolPoint();
		}
	}

	// -------------------------
	// 飛びかかりコルーチン
	// -------------------------
	private System.Collections.IEnumerator LeapAttack()
	{
		state = State.Leap;
		leapTimer = 0f;

		float originalSpeed = agent.speed;

		// 1. 溜め
		agent.isStopped = true;
		yield return new WaitForSeconds(preLeapDelay);

		// 2. 飛びかかり方向を確定
		leapDirection = (target.position - transform.position).normalized;

		// 3. 飛びかかり開始
		agent.isStopped = false;
		agent.speed = leapSpeed;

		yield return new WaitForSeconds(leapDuration);

		// 4. 硬直
		agent.isStopped = true;
		agent.speed = 0f;
		yield return new WaitForSeconds(postLeapStun);

		// 5. 追跡へ復帰
		agent.isStopped = false;
		agent.speed = originalSpeed;

		state = State.Chase;
	}

	// -------------------------
	// 視界判定
	// -------------------------
	bool CanSeePlayer()
	{
		if (target == null) return false;

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
	// パトロール地点
	// -------------------------
	void GoToNextPatrolPoint()
	{
		if (patrolPoints.Length == 0) return;

		agent.SetDestination(patrolPoints[currentPatrolIndex].position);
		currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
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

	// -------------------------
	// スプライト回転
	// -------------------------
	void UpdateSpriteRotation()
	{
		if (spriteRoot == null) return;

		Vector2 moveDir = agent.velocity.sqrMagnitude > 0.01f
			? agent.velocity.normalized
			: lastMoveDir;

		if (moveDir.sqrMagnitude < 0.001f)
			return;

		float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
		spriteRoot.rotation = Quaternion.Euler(0, 0, angle - 90f);
	}

	private void OnEnable()
	{
		TrapEvent.OtonarashiTrapActivated += GetPlayerPosition;
	}

	private void OnDisable()
	{
		TrapEvent.OtonarashiTrapActivated -= GetPlayerPosition;
	}
	private void GetPlayerPosition(Transform playerPos)
	{
		target = playerPos;
		state = State.Chase;
		agent.SetDestination(target.position);
	}
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
