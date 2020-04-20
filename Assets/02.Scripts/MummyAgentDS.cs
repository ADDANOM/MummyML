using UnityEngine;
using MLAgents;

public class MummyAgentDS : Agent
{
    public Transform planeTr;
    public Transform targetTr;
    public Renderer planeRd;

    private Color originColor;
    private Transform mummyTr;

    //에이젼트 초기화
    public override void Initialize()
    {
        maxStep = 5000; //최대 시도횟수
        mummyTr = GetComponent<Transform>();
        originColor = planeRd.material.color;
    }

    //에피소드의 초기화 (훈련이 시작될때 마다 호출)
    public override void OnEpisodeBegin()
    {

    }

    //관측정보를 수집하고 브레인에 전달하는 역할
    public override void CollectObservations(MLAgents.Sensors.VectorSensor sensor)
    {
        //#1. 관측정보
        //에이전트의 위치와 바닥(중앙)의 위치간의 거리 : 떨어질수록 - 패널티
        Vector3 dist1 = planeTr.localPosition - mummyTr.localPosition;
        float norX1 = Mathf.Clamp(dist1.x, -1f, +1f);
        float norZ1 = Mathf.Clamp(dist1.z, -1f, +1f);

        //#2. 관측정보
        Vector3 dist2 = targetTr.localPosition - mummyTr.localPosition;

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
