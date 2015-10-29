
using System.Collections.Generic;
using UnityEngine;

public class DynamicAddTexture:MonoBehaviour
{
    private List<string> textures = new List<string>();
    private GameObject obj;

    private void LoadSprite()
    {
        obj = GameObject.Find("Grid").gameObject;
        if (obj == null)
        {
            return;
        }

        if (textures.Count > 0)
        {
            for (int i = 0; i < textures.Count; i++)
            {
                GameObject sprite = Instantiate(Resources.Load("Texture", typeof (GameObject))) as GameObject;
                sprite.transform.parent = obj.transform;
                sprite.transform.position = new Vector3(80 * i,0f,0f);
                sprite.transform.localScale = Vector3.one;
                sprite.name = textures[i];

                UISprite _sprite = sprite.GetComponent<UISprite>();
                _sprite.atlas = Resources.Load("MyAtals", typeof (UIAtlas)) as UIAtlas;
                _sprite.spriteName = textures[i];
                _sprite.MakePixelPerfect();  //刷新界面
            }
        }
    }
}