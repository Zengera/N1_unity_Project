﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    // 移動可能な範囲
    public static Vector2 m_moveLimit = new Vector2( 7f, 4f );

    // 指定された位置を移動可能な範囲に収めた値を返す
    public static Vector3 ClampPosition( Vector3 position )
    {
        return new Vector3
        (
            Mathf.Clamp( position.x, -m_moveLimit.x, m_moveLimit.x ),
            Mathf.Clamp( position.y, -m_moveLimit.y, m_moveLimit.y ),
            0
        );
    }

    // 指定された角度（ 0 ～ 360 ）をベクトルに変換して返す
    public static Vector3 GetDirection( float angle )
    {
        return new Vector3
        (
            Mathf.Cos( angle * Mathf.Deg2Rad ),
            Mathf.Sin( angle * Mathf.Deg2Rad ),
            0
        );
    }
}
