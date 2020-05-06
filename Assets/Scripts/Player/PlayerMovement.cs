using UnityEngine;

namespace Robbie
{
	///<summary>
	/// 角色移动
	///</summary>
	public class PlayerMovement : MonoBehaviour
	{
        /// <summary>
        /// 主角的刚体组件
        /// </summary>
        private Rigidbody2D rb;
        /// <summary>
        /// 主角的boxcollider组件
        /// </summary>
        private BoxCollider2D coll;

        /// <summary>
        /// 站立移动的speed
        /// </summary>
        [Header("移动参数")]        
        public float speed = 8f;
        /// <summary>
        /// 下蹲时移动的speed
        /// </summary>
        public float crouchSpeedDivisor = 3f;

        /// <summary>
        /// 跳跃的力
        /// </summary>
        [Header("跳跃参数")]
        public float jumpForce; 
        /// <summary>
        /// 长按跳跃键的力
        /// </summary>
        public float jumpHoldForce; 
        /// <summary>
        /// 跳跃的持续时间
        /// </summary>
        public float jumpHoldDuration; 
        /// <summary>
        /// 下蹲跳的力
        /// </summary>
        public float crouchJumpBoost; 
        /// <summary>
        /// 悬挂时跳跃的力
        /// </summary>
        public float hangingJumpForce; 
        /// <summary>
        /// 跳跃加持时间
        /// </summary>
        private float jumpTime;

        /// <summary>
        /// 是否下蹲状态
        /// </summary>
        [Header("状态")]
        public bool isCrouch;
        /// <summary>
        /// 是否在地面
        /// </summary>
        public bool isOnGround;
        /// <summary>
        /// 是否跳跃状态
        /// </summary>
        public bool isJump;
        /// <summary>
        /// 主角头上是否遮盖
        /// </summary>
        public bool isHeadBlocked;
        /// <summary>
        /// 攀爬状态
        /// </summary>
        public bool isHanging;

        /// <summary>
        /// 脚射线偏移量
        /// </summary>
        [Header("环境监测")]
        public float footOffset;
        /// <summary>
        /// 头顶检测的射线长度
        /// </summary>
        public float headClearance;
        /// <summary>
        /// 双脚射线检测的长度
        /// </summary>
        public float groundDisstance;    
        /// <summary>
        /// 主角眼睛高度位置
        /// </summary>
        public float eyeHeight;
        /// <summary>
        /// 主角离墙的距离(可悬挂的距离，也是悬挂射线的长度
        /// </summary>
        public float grabDistance;
        /// <summary>
        /// 自上而下的悬挂射线
        /// </summary>
        public float reachOffset;
        /// <summary>
        /// 角色身高(collider的高度
        /// </summary>
        private float playerHeight;

        /// <summary>
        /// Layer:Ground 地面
        /// </summary>
        private LayerMask groundMask;

        //按键检测
        /// <summary>
        /// 按下跳跃键
        /// </summary>
        private bool jumpPressed;
        /// <summary>
        /// 长按跳跃键
        /// </summary>
        private bool jumpHeld;
        /// <summary>
        /// 长按蹲下键
        /// </summary>
        private bool crouchHeld;
        /// <summary>
        /// 按下下蹲键
        /// </summary>
        private bool crouchPressed;

        //主角boxcollider尺寸
        /// <summary>
        /// 主角collider尺寸
        /// </summary>
        private Vector2 colliderStandSize;
        /// <summary>
        /// 主角collider的偏移量
        /// </summary>
        private Vector2 colliderStandOffset;
        /// <summary>
        /// 下蹲时主角的collider的尺寸
        /// </summary>
        private Vector2 colliderCrouchSize;
        /// <summary>
        /// 下蹲时主角的collider的偏移量
        /// </summary>
        private Vector2 colliderCrouchOffset;

        private Vector2 moveVector2;
        /// <summary>
        /// 左右按键返回值
        /// </summary>
        public float xVelocity;


        /*-------------生命周期------------*/
        private void Start()
        {
            moveVector2 = new Vector2();
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();

            groundMask = LayerMask.GetMask("Ground");

            playerHeight = coll.size.y;

            colliderStandSize = coll.size;
            colliderStandOffset = coll.offset;

            colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
            colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
        }

        private void FixedUpdate()
        {
            PhysicsCheck();
            GroundMovement();
            MidAirMovement();            
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
                jumpPressed = true;

            jumpHeld = Input.GetButton("Jump");
            crouchHeld = Input.GetButton("Crouch");
            crouchPressed = Input.GetButtonDown("Crouch");          
        }
        /*------------End-生命周期------------*/


        /// <summary>
        /// 主角状态监测(是否在地面,头顶是否被遮盖
        /// </summary>
        private void PhysicsCheck()
        {
            //角色左右脚的射线检测是否是地面
            RaycastHit2D leftFootCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDisstance, groundMask); 
            RaycastHit2D rightFootCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDisstance, groundMask);

            if (leftFootCheck || rightFootCheck)
                isOnGround = true;
            else
                isOnGround = false;

            //角色头顶的射线检测头顶是否遮盖
            RaycastHit2D leftHeadCheck = Raycast(new Vector2(-footOffset, coll.size.y), Vector2.up, headClearance, groundMask); 
            RaycastHit2D rightHeadCheck = Raycast(new Vector2(footOffset, coll.size.y), Vector2.up, headClearance, groundMask);

            if (leftHeadCheck || rightHeadCheck)
                isHeadBlocked = true;
            else
                isHeadBlocked = false;

            //悬挂射线检测
            //悬挂射线发射方向，主角当前的x
            float direction = transform.localScale.x;
            Vector2 grabDir = new Vector2(direction, 0f);

            //头顶的射线
            RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundMask);
            //从眼睛发射的射线
            RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundMask);
            //头顶 自上而下的射线
            RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundMask);

            //悬挂条件 成立
            if(!isOnGround && rb.velocity.y < 0 && ledgeCheck && wallCheck && !blockedCheck)
            {
                //悬挂墙上前，更改主角的位置，改成符合悬挂在墙上的位置
                Vector3 pos = transform.position;                  //获取主角pos

                pos.x += (wallCheck.distance - 0.05f) * direction; //主角pos加上wallCheck的射线到墙上的距离跟方向

                pos.y -= ledgeCheck.distance;                      //主角 定住时 要减去高出的部分，高出为ledgeCheck的射线距离

                transform.position = pos;                          //重置主角位置

                //更改 rb 的类型，让主角定在墙上
                rb.bodyType = RigidbodyType2D.Static;
                isHanging = true;
            }

            if (isHanging)
                isOnGround = false;
        }

        /// <summary>
        /// 地面上的移动
        /// </summary>
        private void GroundMovement()
        {
            if (isHanging)
                return;

            //长按下蹲键 非下蹲状态 在地面上,执行下蹲
            if (crouchHeld && !isCrouch && isOnGround)
                Crouch();
            //没有长按下蹲键 下蹲状态 头上没有遮盖，执行自动站立
            else if (!crouchHeld && isCrouch && !isHeadBlocked)
                StandUp();
            //不在地面上 下蹲状态，执行自动站立
            else if (!isOnGround && isCrouch)
                StandUp();

            xVelocity = Input.GetAxis("Horizontal");

            //下蹲状态，移动速度慢点
            if (isCrouch)
                xVelocity /= crouchSpeedDivisor;

            moveVector2.x = xVelocity * speed;
            moveVector2.y = rb.velocity.y;
            //rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
            
            rb.velocity = moveVector2;

            FilpDirction();
        }

        /// <summary>
        /// 角色跳跃
        /// </summary>
        private void MidAirMovement()
        {           
            //判断是否悬挂
            if(isHanging)
            {
                //悬挂时按下跳跃键，执行跳跃
                if (jumpPressed)
                {                    
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                    isHanging = false;

                    AudioManager.Instance.PlayerJumpAudio();
                }

                //悬挂时按下下蹲键，取消悬挂，自然落下
                if(crouchPressed)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    isHanging = false;                    
                }
            }

            //  单击跳跃键      在地面         非跳跃状态  头顶无遮盖  使用单击跳跃键的跳跃
            if (jumpPressed && isOnGround && !isJump && !isHeadBlocked)
            {
                // 下蹲状态    使用下蹲跳
                if (isCrouch)
                {
                    StandUp();
                    rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
                }

                isOnGround = false;
                isJump = true;                

                jumpTime = Time.time + jumpHoldDuration;

                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

                AudioManager.Instance.PlayerJumpAudio();
            }

            //      跳跃状态    不在地面（下蹲跳加强跳跃力
            else if (isJump && !isOnGround)
            {
                //长按跳跃键
                if (jumpHeld)                
                    rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
                
                if (jumpTime < Time.time)
                    isJump = false;                
            }

            //自动蹲伏期间，按了跳，在离开自动蹲伏的时候会跳一下，上面if都不成立，要重置下跳跃键
            jumpPressed = false;
        }       

        /// <summary>
        /// 根据移动更改脸的朝向
        /// </summary>
        private void FilpDirction()
        {
            if (xVelocity < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            if (xVelocity > 0)
                transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 角色下蹲
        /// </summary>
        private void Crouch()
        {
            isCrouch = true;
            coll.size = colliderCrouchSize;
            coll.offset = colliderCrouchOffset;
        }

        /// <summary>
        /// 角色自动站立
        /// </summary>
        private void StandUp()
        {
            isCrouch = false;
            coll.size = colliderStandSize;
            coll.offset = colliderStandOffset;
        }

        /// <summary>
        /// 封装射线检测( Physics2D.Raycast() ）
        /// </summary>
        /// <param name="offset">射线起点偏移量</param>
        /// <param name="rayDiraction">射线方向</param>
        /// <param name="length">射线长度</param>
        /// <param name="layerMask">检测的Layer</param>
        /// <returns></returns>
        private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layerMask)
        {
            Vector2 pos = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layerMask);

            Color color = hit ? Color.red : Color.green;

            Debug.DrawRay(pos + offset, rayDiraction * length, color);

            return hit;
        }

    /*--------------------------------------------------------*/
    }
}