float  HorizontalAngle()
	{    
        Vector3 directionToPlayer = player.position - transform.position;

        // 2. Flatten the vector (ignore vertical difference)
        directionToPlayer.y = 0;

        // 3. Calculate the angle between the enemy's forward and this flat direction
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle;
	}