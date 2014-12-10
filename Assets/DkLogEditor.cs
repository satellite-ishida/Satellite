using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// DkLog に対応するエディタ処理
/// </summary>

public class DkLogEditor : EditorWindow
{
    /// <summary>
    /// ログウィンドウを開く
    /// </summary>
    [MenuItem("DkTools/Log")]
    public static void Open()
    {
        DkLogEditor window = EditorWindow.GetWindow<DkLogEditor>();
        window.Initialize();
        window.ShowUtility();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        title = "DkLog";
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Repaint();
    }

    /// <summary>
    /// ウィンドウ内描画
    /// </summary>
    void OnGUI()
    {
        GUI.color = Color.black;
        BeginWindows();
        DkLog.DrawLogWindow(new Rect(0, 0, position.width, position.height), true);
        EndWindows();
    }
}