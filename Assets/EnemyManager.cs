﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 追加しましょう

// 敵の出現を制御するコンポーネント
public class EnemyManager : MonoBehaviour
{
    public Enemy[] m_enemyPrefabs; // 敵のプレハブを管理する配列
    public float m_interval; // 出現間隔（秒）

    private float m_timer; // 出現タイミングを管理するタイマー

    public int m_score; // スコア
    public Text score_text = null; // Textオブジェクト

    private void Start()
    {
        m_score = -1;
        ScoreDisplay();
    }

    // 毎フレーム呼び出される関数
    private void Update()
    {
        // 出現タイミングを管理するタイマーを更新する
        m_timer += Time.deltaTime;

        // まだ敵が出現するタイミングではない場合、
        // このフレームの処理はここで終える
        if ( m_timer < m_interval ) return;

        // 出現タイミングを管理するタイマーをリセットする
        m_timer = 0;

        // 出現する敵をランダムに決定する
        var enemyIndex = Random.Range( 0, m_enemyPrefabs.Length );

        // 出現する敵のプレハブを配列から取得する
        var enemyPrefab = m_enemyPrefabs[ enemyIndex ];

        // 敵のゲームオブジェクトを生成する
        var enemy = Instantiate( enemyPrefab );

    }

    public void ScoreDisplay()
    {
        m_score += 1;
        score_text.text = "Score: " + m_score.ToString();
    }
}