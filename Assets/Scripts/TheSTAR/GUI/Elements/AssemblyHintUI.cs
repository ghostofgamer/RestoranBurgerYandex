using System;
using TheSTAR.GUI;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyHintUI : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private GameObject checkTotalObject;
    [SerializeField] private RecipeElementUI[] recipeItems;
    [SerializeField] private PointerButton button;

    public void Init(Action onClickAction)
    {
        button.Init(onClickAction);
    }

    public void Set(Sprite mainSprite, Sprite[] ingredientSprites, bool totalCompleted, bool[] checks)
    {
        iconImg.sprite = mainSprite;
        checkTotalObject.gameObject.SetActive(totalCompleted);

        foreach (var ingredient in recipeItems) ingredient.gameObject.SetActive(false);

        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            recipeItems[i].gameObject.SetActive(true);
            recipeItems[i].SetIcon(ingredientSprites[i]);
            recipeItems[i].SetCompleted(checks[i]);
        }
    }

    public void Set(bool[] checks)
    {
        for (int i = 0; i < checks.Length; i++)
        {
            recipeItems[i].SetCompleted(checks[i]);
        }
    }
}