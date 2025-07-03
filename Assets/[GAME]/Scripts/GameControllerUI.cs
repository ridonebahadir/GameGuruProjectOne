using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameControllerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button okBtn;
    [SerializeField] private TMP_InputField gridSizeInputField;

    private GridManager _gridManager;


    [Inject]
    private void Construct(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    private void OnEnable()
    {
        _gridManager.OnScoreAdd += UpdateScoreText;
    }

    private void OnDisable()
    {
        _gridManager.OnScoreAdd -= UpdateScoreText;
    }

    private void Start()
    {
        okBtn.onClick.AddListener(ClickedOkBtn);
    }

    private void ClickedOkBtn()
    {
        _gridManager.GridSize = GetInputValue();
        _gridManager.ResetAllGrid();
    }

    private int GetInputValue()
    {
        return int.TryParse(gridSizeInputField.text, out var value) ? value : 0;
    }

    private void UpdateScoreText(int score)
    {
        scoreText.SetText("Score " + score);
    }
}