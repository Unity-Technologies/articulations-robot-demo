using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerBoard : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Material material; 
    Texture2D texture; 

    Vector3 valuesColor1;

    Vector3 valuesColor2;

    [SerializeField] float width = 5.0f; // it will be the number of pattern of each color on each face 
    
    
    public void CheckerBoardChange(){
        // this function is to randomly select to do the checker pattern or not 
        float checker = Random.Range(0,2);

        // we randomly select to generate a checker pattern 
        if (checker == 0){
            CheckerBoardLaunch();
        }
    }   
    
    
    void CheckerBoardLaunch()
    {   
        // this function is the launch function
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material; 
        texture = new Texture2D(128, 128, TextureFormat.RGBA32, true, true); // 64 x 64 is the size of the texture. The smaller it is the bigger is the pattern (for a fixed width)
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        material.SetTexture("_MainTex", texture);
        CreateCheckerBoard();

    }

    void CreateCheckerBoard(){
        // this function is to perform the evaluation of the color of the chunk on all the texture of the object 
        valuesColor1 = new Vector3(Random.value, Random.value, Random.value);
        valuesColor2 = new Vector3(Random.value, Random.value, Random.value); 
        for (int y = 0; y < texture.height; y++){
            for (int x = 0; x < texture.width; x++){
                Color temp = EvaluateCheckerBoardPixel(x, y, valuesColor1, valuesColor2);
                texture.SetPixel(x, y , temp);
            }
        }
        // upload the texture to the GPU
        texture.Apply();
    }

    Color EvaluateCheckerBoardPixel(int x, int y, Vector3 valuesColor1, Vector3 valuesColor2){  
        // this function is to evalute which color is the chunk for a given (x, y) position

       float valueX = (x % (width * 2.0f)) / (width * 2.0f); // in our case there is only two different patterns: let's say black and white
       // in order to avoid keeping tracking of whether it is balck or white, we consider the whole space : width * 2.0f. Then we normalize valueX
       int vX = 1;
       if (valueX < 0.5f){
           vX = 0;
       }

       float valueY = (y % (width * 2.0f)) / (width * 2.0f); 
       int vY = 1;
       if (valueY < 0.5f){
           vY = 0;
       }

       if (vX == vY){
           return new Color(valuesColor1[0], valuesColor1[1], valuesColor1[2], 1.0f);
       }

       return new Color(valuesColor2[0], valuesColor2[1], valuesColor2[2], 1.0f);
    }
}
