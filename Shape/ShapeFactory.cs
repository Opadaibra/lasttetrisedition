﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class ShapeFactory : MonoBehaviour
{
    //config parameters
    [SerializeField] private List<string> views;
    [SerializeField] private List<string> style;
    [SerializeField] private List<Shape> twoDShapes;
    [SerializeField] private List<Shape> threeDShapes;
    [SerializeField] private List<Sprite> shapeTexture;
    [SerializeField] private Shape mainTwoDBlock;
    [SerializeField] private Shape mainThreeDBlock;
    
    ///
    private Dictionary<string ,List<Shape>> _shapeMap;
    private Dictionary<string, Sprite> _textureMap;
    private Dictionary<string, Shape> _mainBlocks;
    private void Awake()
    {
        _shapeMap = new Dictionary<string , List<Shape>>() ;
        _textureMap = new Dictionary<string, Sprite>() ;
        _mainBlocks  = new Dictionary<string, Shape>();
 
        //fill shape map
        _shapeMap.Add(views[0], twoDShapes);
        _shapeMap.Add(views[1], threeDShapes);
        
        //fill main blocks map
        _mainBlocks.Add(views[0] ,mainTwoDBlock);
        _mainBlocks.Add(views[1] ,mainThreeDBlock);
        
        //fill texture map
        _textureMap.Add(style[0],shapeTexture[0]);
        _textureMap.Add(style[1],shapeTexture[1]);
      /*  var minSize = Math.Min(style.Count, shapeTexture.Count);
        for (var i = 0; i < minSize; i++)
            _textureMap.Add(style[i], shapeTexture[i]);*/
        
    }
    public Shape GetShape(string shapeStyle , string view)
    {
        var randomChoice = GenerateRandomShape(view);
        BlindTexture(randomChoice, shapeStyle);
        var randomShape = Instantiate(randomChoice, transform.position+randomChoice.InstantiateDis, Quaternion.identity) as Shape ;
        return randomShape ;
    }

    private Shape GenerateRandomShape(string view)
    {
        var shapes = _shapeMap[view];
        var randomIndex = UnityEngine.Random.Range(0, shapes.Count);
        return shapes[randomIndex];
    }
    private void BlindTexture(Shape shape, string shapeStyle)
    {
        //test for three D
        var currSprite = _textureMap[shapeStyle];
        shape.ChangeTexture(currSprite);
        if(shapeStyle=="Laser")
            shape.ChangeColor(Color.white);
        else
            shape.ChangeColor(shape.shapeColor);
        /* for(int i=0 ;i<shape.transform.childCount ;i++)
        for (int j = 0; j < shape.transform.GetChild(i).childCount; j++)
            shape.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().sprite = currSprite;*/
          // shape.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = currSprite;
    }

    public Shape GetMainBlock(string view,string style)
    {
        var mainBlock = Instantiate(_mainBlocks[view]);
        var mainBlockSprite = _textureMap[style]; 
        mainBlock.ChangeTexture(mainBlockSprite);
        return mainBlock; 
    }
}