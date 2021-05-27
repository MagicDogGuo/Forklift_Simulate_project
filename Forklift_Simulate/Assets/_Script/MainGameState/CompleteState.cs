using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteState : IMainGameState
{
    public CompleteState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompleteState";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
