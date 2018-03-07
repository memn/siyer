﻿using UnityEngine;
using UnityEngine.Assertions;

public class PuzzleObject : MonoBehaviour
{
    private bool _selected;
    public char Letter { get; private set; }

    public void SetCharacter(char c)
    {
        var textMesh = transform.Find("Text").GetComponent<TextMesh>();
        Letter = c;
        textMesh.text = c.ToString();
    }

    public bool isSelected()
    {
        return _selected;
    }

    public void Select()
    {
        Assert.IsFalse(_selected);
        GetComponent<SpriteRenderer>().color = Color.yellow;
        _selected = true;
    }

    public void Unselect()
    {
        Assert.IsTrue(_selected);
        GetComponent<SpriteRenderer>().color = Color.white;
        _selected = false;
    }

    public void Correctify()
    {
        Assert.IsTrue(_selected);
        GetComponent<SpriteRenderer>().color = Color.green;
        GetComponent<CircleCollider2D>().enabled = false;
    }
}