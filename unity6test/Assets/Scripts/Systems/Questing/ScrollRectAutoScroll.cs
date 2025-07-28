using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scrollSpeed = 10f;
    private bool mouseOver = false;

    private List<Selectable> m_Selectables = new List<Selectable>();
    private ScrollRect m_ScrollRect;

    private Vector2 m_NextScrollPosition = Vector2.up;

    // Reference to the Input Actions
    public InputAction moveAction;
    //switched 2dvector to analog from digital normalized to make it work on webgl

    public Scrollbar bar;

    void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    void OnEnable()
    {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }

        // Enable the input action
        moveAction.Enable();
    }

    void OnDisable()
    {
        // Disable the input action when the object is disabled
        moveAction.Disable();
    }

    void Start()
    {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
        ScrollToSelected(true);
    }

    void Update()
    {
        InputScroll();

        if (!mouseOver)
        {
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
        
    }

    void InputScroll()
    {
        if (m_Selectables.Count > 0)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            if (moveInput.x != 0.0f || moveInput.y != 0.0f)
            {
                ScrollToSelected(false);
            }
        }
    }

    public void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

        if (selectedElement)
        {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
        }
        if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
            else
            {
                m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }
}
