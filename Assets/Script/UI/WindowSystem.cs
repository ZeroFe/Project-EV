using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * UI를 Window와 View로 구분하며,
 * View : 게임에서 상시 보이거나, Window 안에 들어가는 부속 UI
 * Window : 업그레이드나 메뉴 등 커서가 보이는 UI
 */

/// <summary>
/// 정체 Window를 관리아는 클래스로, 모든 Window는 이 클래스를 거쳐서 켜야한다
/// ESC로 윈도우를 끄거나 커서를 보이게 하는 등의 관리를 한다
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

    // 여러 윈도우가 켜져도 
    // 끌 수 없는 윈도우를 구분
    private Stack<WindowClass> windowStack = new Stack<WindowClass>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Window System Actived");
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        // 게임 시작 시 윈도우는 전부 비활성화 상태이므로 커서를 안 보여줘야 한다
        SetCursorDisplay(false);
    }
    
    void Update()
    {
        // 창 닫기
        // 창 닫을거 없으면 메뉴 켜기
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
        // 더 이상 윈도우가 켜져 있지 않을 땐 윈도우를 끌 수 없다
        if (windowStack.Count == 0)
        {
            return false;
        }

        var wc = windowStack.Pop();
        // 종료할 수 있는 Window인지 확인한다
        if (isUserExit && !wc.isUserExitable)
        {
            windowStack.Push(wc);
            return false;
        }

        // 윈도우 종료
        wc.windowObject.SetActive(false);

        // 윈도우를 전부 종료했다면 커서를 띄운다
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
