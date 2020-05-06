using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Robbie
{
	///<summary>
	/// 彩蛋
	///</summary>
	public class EasterEgg : MonoBehaviour
	{        
        private LayerMask playerMask;
        private TilemapRenderer renderer;

        RaycastHit2D hit00;
        RaycastHit2D hit01;
        RaycastHit2D hit02;
        RaycastHit2D hit03;
        RaycastHit2D hit04;
        RaycastHit2D hit05;
        RaycastHit2D hit06;
        RaycastHit2D hit07;
        RaycastHit2D hit08;
        RaycastHit2D hit09;


        private void Start()
        {
            playerMask = LayerMask.GetMask("Player");
            renderer = GetComponentInParent<TilemapRenderer>();
        }

        private void Update()
        {
            hit00 = Raycast(new Vector2(0, 0), Vector2.right, 8f, playerMask);
            hit01 = Raycast(new Vector2(0, 1), Vector2.right, 8f, playerMask);
            hit02 = Raycast(new Vector2(0, 2), Vector2.right, 8f, playerMask);
            hit03 = Raycast(new Vector2(0, 3), Vector2.right, 8f, playerMask);
            hit04 = Raycast(new Vector2(0, 4), Vector2.right, 8f, playerMask);
            hit05 = Raycast(new Vector2(0, 5), Vector2.right, 8f, playerMask);
            hit06 = Raycast(new Vector2(0, 6), Vector2.right, 8f, playerMask);
            hit07 = Raycast(new Vector2(0, 7), Vector2.right, 8f, playerMask);
            hit08 = Raycast(new Vector2(0, 8), Vector2.right, 8f, playerMask);
            hit09 = Raycast(new Vector2(0, 9), Vector2.right, 8f, playerMask);


            if (hit00 || hit01 || hit02 || hit03 || hit04 || hit05 || hit06 || hit07 || hit08 || hit09)
                renderer.enabled = false;
            else
                renderer.enabled = true;            
        }


        /// <summary>
        /// 封装射线检测( Physics2D.Raycast()
        /// </summary>
        /// <param name="offset">射线起点</param>
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

    }
}