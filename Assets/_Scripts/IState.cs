using UnityEngine;
using System.Collections;


    public interface IState
    {
        void OnEnter(string prevState); //前一个状态
        void OnExit(string nextState);
        void OnUpdate();

    }

