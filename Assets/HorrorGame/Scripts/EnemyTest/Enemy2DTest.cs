using UnityEngine;

public class Enemy2DTest : MonoBehaviour
{
	public Transform[] waypoints; // 巡回ポイント
	public Transform player;

	public float patrolSpeed = 2f; // 巡回中速度
	public float chaseSpeed = 4f;// 追跡中速度
	public float waitTime = 1f; // 各ポイントで止まる時間


	private int currentIndex = 0;
	private float waitCounter = 0f;
	private bool isWaiting = false;

	[Header("視界設定")] 
	public float viewDistance = 5f; // 視界距離
	public float viewAngle = 90f; // 視野角（扇の広さ）
	public LayerMask obstacleMask; // 壁用
	private enum State { Patrol, Chase }
	private State currentState;
	private Vector3 lastKnownPosition;

	private float lostTimer = 0f;

	public float loseTime = 2f;
	void Start()
	{
		currentState = State.Patrol;

	}

	// プレイヤー視認
	bool CanSeePlayer()
	{
		Vector3 origin = transform.position;
		Vector3 targetPos = player.position;

		// Z固定（2D）
		origin.z = 0;
		targetPos.z = 0;

		Vector3 direction = targetPos - origin;

		float distance = direction.magnitude;

		// 距離チェック
		if (distance > viewDistance)
			return false;

		direction.Normalize();

		// 角度チェック
		float angle = Vector3.Angle(transform.up, direction);

		// 全角度式
		if (angle > viewAngle / 2f)
			return false;

		// 壁チェック
		RaycastHit hit;

		if (Physics.Raycast(
			origin,
			direction,
			out hit,
			viewDistance,
			~0))
		{
			// 最初に当たったのがプレイヤー
			if (hit.transform == player)
			{
				Debug.DrawRay(
					origin,
					direction * viewDistance,
					Color.green);

				return true;
			}
		}

		Debug.DrawRay(
			origin,
			direction * viewDistance,
			Color.red);

		return false;
	}
	void Update()
	{
		float distanceToPlayer =
			Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer < viewDistance && CanSeePlayer())
		{
			currentState = State.Chase;

			// 最後に見た位置更新
			lastKnownPosition = player.position;

			// タイマーリセット
			lostTimer = loseTime;

		}
		else
		{
			lostTimer -= Time.deltaTime;

			if (lostTimer > 0f)
			{
				currentState = State.Chase;

			}
			else
			{
				currentState = State.Patrol;

			}
		}

		if (currentState == State.Patrol)
		{
			Patrol();
		}
		else if (currentState == State.Chase)
		{
			Chase();
		}
	}
	void Patrol() // 巡回
	{
		// 待機状態制御
		if (waypoints.Length == 0) return;

		if (isWaiting)
		{
			waitCounter -= Time.deltaTime;
			if (waitCounter <= 0f)
			{
				isWaiting = false;
				currentIndex = (currentIndex + 1) % waypoints.Length;
			}
			return;
		}
		// 巡回ポイント決定
		Vector3 targetPos = waypoints[currentIndex].position;

		Move(targetPos, patrolSpeed);
		// 到達判定
		if (Vector3.Distance(transform.position, targetPos) < 0.1f)
		{
			isWaiting = true;
			waitCounter = waitTime;
		}
	}
	void Chase() // 追跡
	{
		Vector3 targetPos = lastKnownPosition;
		Move(targetPos, chaseSpeed);
	}

	public LayerMask wallMask;
	public float avoidDistance = 0.5f;
	void Move(Vector3 target, float speed)
	{
		target.z = transform.position.z;

		Vector3 dir = (target - transform.position).normalized;

		// 前方壁チェック
		RaycastHit2D hit = Physics2D.Raycast(
			transform.position,
			dir,
			avoidDistance,
			wallMask
		);

		// 壁がある
		if (hit)
		{
			// 左右回避方向
			Vector3 left = Quaternion.Euler(0, 0, 90) * dir;
			Vector3 right = Quaternion.Euler(0, 0, -90) * dir;

			// 左チェック
			bool leftBlocked = Physics2D.Raycast(
				transform.position,
				left,
				avoidDistance,
				wallMask
			);

			// 右チェック
			bool rightBlocked = Physics2D.Raycast(
				transform.position,
				right,
				avoidDistance,
				wallMask
			);

			// 空いてる方向へ
			if (!leftBlocked)
				dir = left;
			else if (!rightBlocked)
				dir = right;
			else
				return;
		}

		transform.position += dir * speed * Time.deltaTime;

		// 回転
		float angle =
			Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

		Quaternion targetRotation =
			Quaternion.Euler(0, 0, angle);

		transform.rotation = Quaternion.Slerp(
			transform.rotation,
			targetRotation,
			5f * Time.deltaTime
		);
	}
	void OnDrawGizmos()
	{
		Vector3 origin = transform.position;

		Vector3 forward = transform.up;

		Gizmos.color = Color.yellow;

		Vector3 left =
			Quaternion.Euler(0, 0, viewAngle / 2f) * forward;

		Vector3 right =
			Quaternion.Euler(0, 0, -viewAngle / 2f) * forward;

		Gizmos.DrawLine(origin, origin + left * viewDistance);
		Gizmos.DrawLine(origin, origin + right * viewDistance);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(origin, origin + forward * viewDistance);
	}
}