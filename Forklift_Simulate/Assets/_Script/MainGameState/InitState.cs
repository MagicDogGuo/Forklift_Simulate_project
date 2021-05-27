using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : IMainGameState
{
    public InitState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "InitState";
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
