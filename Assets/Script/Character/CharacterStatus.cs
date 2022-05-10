using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ĳ���� �ɷ�ġ Ŭ����
/// 
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    public delegate void HpChangedHandler(int current, int max);

    public HpBar hpBar;

    [SerializeField]
    private int maxHp = 100;
    private int _currentHp = 100;

    public int CurrentHp
    {
        get => _currentHp;
        set
        {
            _currentHp = value;
            if (_currentHp <= 0)
            {
                _currentHp = 0;
            }
            else if (_currentHp > maxHp)
            {
                _currentHp = maxHp;
            }
            onHpChanged?.Invoke(_currentHp, maxHp);
        }
    }

    public event HpChangedHandler onHpChanged;

    // Start is called before the first frame update
    void Start()
    {
        onHpChanged += hpBar.DrawHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("HP Restore");
            CurrentHp += 10;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("HP Lost");
            CurrentHp -= 10;
        }
    }
}
