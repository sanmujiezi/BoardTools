using System.Collections.Generic;
using UnityEngine;

namespace GirlBoardCreater
{
    public class PolygonOffsetUtility
    {
        
        /// <summary>
        /// 计算偏移后的点
        /// </summary>
        /// <param name="point">当前点</param>
        /// <param name="prevPoint">前一个点</param>
        /// <param name="nextPoint">后一个点</param>
        /// <param name="delta">偏移量（正数为膨胀，负数为收缩）</param>
        /// <returns>偏移后的点</returns>
        public static Vector2 GetOffsetPoint(Vector2 point, Vector2 prevPoint, Vector2 nextPoint, float delta)
        {
            // 计算前一条边和后一条边的向量
            Vector2 edgePrev = point - prevPoint;
            Vector2 edgeNext = nextPoint - point;

            // 计算两条边的法线方向（旋转90度）
            Vector2 normalPrev = new Vector2(-edgePrev.y, edgePrev.x).normalized;
            Vector2 normalNext = new Vector2(-edgeNext.y, edgeNext.x).normalized;

            // 添加安全校验
            if (normalPrev == Vector2.zero || normalNext == Vector2.zero)
            {
                Debug.LogError("零向量异常，请检查输入点是否共线");
                return point;
            }

// 添加角度阈值限制（防止尖刺）
            float angle = Vector2.Angle(normalPrev, normalNext);
            if (angle > 150f) 
            {
                return point + normalPrev * delta;
            }
            
            // 计算平均法线方向
            Vector2 averageNormal = (normalPrev + normalNext).normalized;

            // 沿法线方向偏移
            Vector2 offsetPoint = point + averageNormal * delta;

            return offsetPoint;
        }
    }
}