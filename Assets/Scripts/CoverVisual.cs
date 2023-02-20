using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CoverVisual : MonoBehaviour
{
    [SerializeField] private Sprite noCoverSprite;

    [SerializeField] private Sprite fullCoverSprite;
    [SerializeField] private Sprite halfCoverSprite;
    [SerializeField] private GridVisual gridVisual;
    [SerializeField] private SpriteRenderer spriteRendererNorth;
  //  [SerializeField] private SpriteRenderer spriteRendererSouth;
  //  [SerializeField] private SpriteRenderer spriteRendererEast;
  //  [SerializeField] private SpriteRenderer spriteRendererWest;

    private Vector3 mousePosition;
    private void Start()
    {
        MousePointer.Instance.onMousePositionChange += MousePointer_OnMousePositionChange;
    }

    private void MousePointer_OnMousePositionChange(object sender, EventArgs e)
    {
    }

    public void EnableSpriteRenderer()
    {
        //spriteRendererNorth.enabled = true;
  //      spriteRendererSouth.enabled = true;
  //      spriteRendererEast.enabled = true;
  //      spriteRendererWest.enabled = true;

   //     mousePosition = MousePointer.Instance.GetPosition().normalized;
    }

    public void SetSprite(CoverType type, SpriteRenderer spriteRenderer)
    {
      /*  spriteRendererNorth = spriteRenderer;
        if(type == CoverType.Full)
        {
            spriteRendererNorth.sprite = fullCoverSprite;
        }
        else if (type == CoverType.Half)
        {
            spriteRendererNorth.sprite = halfCoverSprite;
        }*/
    }
    public SpriteRenderer GetSpriteRenderer(string direction)
    {
        
       // switch(direction)
      //  {
       //     case "North":
      //          return spriteRendererNorth;
              

       // }
        return null;
        

    }
    public void DisableSpriteRenderer()
    {
      //  spriteRendererNorth.enabled = false;
  //      spriteRendererSouth.enabled = false;
 //       spriteRendererEast.enabled = false;
 //       spriteRendererWest.enabled = false;
    }
}