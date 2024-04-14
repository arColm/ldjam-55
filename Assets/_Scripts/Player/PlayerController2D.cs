using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : Controller2D
{
	public Vector2 up;
	Vector2 right;

	public Vector2 colliderSize;
    private void Start()
    {
		base.Start();
		colliderSize = collider.bounds.size;
    }
    protected override void HorizontalCollisions(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, right * directionX * rayLength, Color.red);

			if (hit)
			{
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}

	protected override void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, up * rayLength, Color.red);

			if (hit)
			{
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	protected override void UpdateRaycastOrigins()
	{
		float angle = -Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
		up = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
		right = new Vector2(Mathf.Sin(angle + Mathf.PI/2), Mathf.Cos(angle + Mathf.PI/2));
		//Debug.DrawRay(transform.position, up,Color.green);
		//Debug.DrawRay(transform.position, right, Color.blue);

		//print(colliderSize.y);


		raycastOrigins.bottomLeft = (Vector2)transform.position - (right * colliderSize.x / 2) - up * colliderSize.y / 2;
		raycastOrigins.bottomRight = (Vector2)transform.position + (right * colliderSize.x / 2) - up * colliderSize.y / 2;
		raycastOrigins.topLeft = (Vector2)transform.position - (right * colliderSize.x / 2) + up * colliderSize.y / 2;
		raycastOrigins.topRight = (Vector2)transform.position + (right * colliderSize.x / 2) + up * colliderSize.y / 2;
		//Debug.DrawRay(raycastOrigins.bottomLeft, new Vector2(-1, -1), Color.white);
		//Debug.DrawRay(raycastOrigins.bottomRight, new Vector2(1, -1), Color.white);
		//Debug.DrawRay(raycastOrigins.topLeft, new Vector2(-1, 1), Color.white);
		//Debug.DrawRay(raycastOrigins.topRight, new Vector2(1, 1), Color.white);
	}
}
