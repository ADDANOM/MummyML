using UnityEngine;
using MLAgents;

public class MummyAgentDS : Agent
{
    //에이젼트 초기화
    public override void Initialize()
    {

    }

    //에피소드의 초기화 (훈련이 시작될때 마다 호출)
    public override void OnEpisodeBegin()
    {

    }

    //관측정보를 수집하고 브레인에 전달하는 역할
    public override void CollectObservations(MLAgents.Sensors.VectorSensor sensor)
    {

    }

    //에이전트 행동명령을 받는 메소드
    public override void OnActionReceived(float[] vectorAction)
    {

    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f; //W, S (전/후)
        actionsOut[1] = 0f; //A, D (좌/우)

        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[1] = 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[1] = 2f;
        }
    }
}
