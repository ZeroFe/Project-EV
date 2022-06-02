using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * UI�� Window�� View�� �����ϸ�,
 * View : ���ӿ��� ��� ���̰ų�, Window �ȿ� ���� �μ� UI
 * Window : ���׷��̵峪 �޴� �� Ŀ���� ���̴� UI
 */

/// <summary>
/// ��ü Window�� �����ƴ� Ŭ������, ��� Window�� �� Ŭ������ ���ļ� �Ѿ��Ѵ�
/// ESC�� �����츦 ���ų� Ŀ���� ���̰� �ϴ� ���� ������ �Ѵ�
/// </summary>
[DisallowMultipleComponent]
public class WindowSystem : MonoBehaviour
{
    public static WindowSystem Instance { get; private set; }

    public Texture2D cursorTexture;

    class WindowClass
    {
        public GameObject windowObject;
        public bool isUserExitable;

        public WindowClass(GameObject windowObject, bool isUserExitable)
        {
            this.windowObject = windowObject;
            this.isUserExitable = isUserExitable;
        }
    }

    // ���� �����찡 ������ 
    // �� �� ���� �����츦 ����
    private Stack<WindowClass> windowStack = new Stack<WindowClass>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Window System Actived");
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        // ���� ���� �� ������� ���� ��Ȱ��ȭ �����̹Ƿ� Ŀ���� �� ������� �Ѵ�
        SetCursorDisplay(false);
    }
    
    void Update()
    {
        // â �ݱ�
        // â ������ ������ �޴� �ѱ�
#if !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
#else
        if (Input.GetKeyDown(KeyCode.Backspace))
        {

        }
#endif
    }

    public void OpenWindow(GameObject windowObject, bool isUserExitable)
    {
        if (windowStack.Count == 0)
        {
            SetCursorDisplay(true);
        }
        windowObject.SetActive(true);
        windowStack.Push(new WindowClass(windowObject, isUserExitable));
    }

    public bool CloseWindow(bool isUserExit)
    {
        // �� �̻� �����찡 ���� ���� ���� �� �����츦 �� �� ����
        if (windowStack.Count == 0)
        {
            return false;
        }

        var wc = windowStack.Pop();
        // ������ �� �ִ� Window���� Ȯ���Ѵ�
        if (isUserExit && !wc.isUserExitable)
        {
            windowStack.Push(wc);
            return false;
        }

        // ������ ����
        wc.windowObject.SetActive(false);

        // �����츦 ���� �����ߴٸ� Ŀ���� ����
        if (windowStack.Count == 0)
        {
            SetCursorDisplay(false);
        }

        return true;
    }

    public void SetCursorDisplay(bool display)
    {
        Time.timeScale = display ? 0 : 1;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }
}
