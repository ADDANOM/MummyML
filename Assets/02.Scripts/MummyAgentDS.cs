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
        mummyTr.localPosition = new Vector3(0.0f, 0.05f, 0.0f);
        targetTr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f)
                                            ,0.5f
                                            ,Random.Range(-4.0f, 4.0f));
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
        float norX2 = Mathf.Clamp(dist2.x, -1f, +1f);
        float norZ2 = Mathf.Clamp(dist2.z, -1f, +1f);

        sensor.AddObservation(norX1);
        sensor.AddObservation(norZ1);
        sensor.AddObservation(norX2);
        sensor.AddObservation(norZ2);
    }

    //에이전트 행동명령을 받는 메소드
    public override void OnActionReceived(float[] vectorAction)
    {
        //에이전트의 이동 
        float h = 0;
        float v = 0;

        switch((int)vectorAction[0])
        {
            case 0: v = 0f; break;
            case 1: v = +1f; break;
            case 2: v = -1f; break;
        }

        switch((int)vectorAction[1])
        {
            case 0: h = 0f; break;
            case 1: h = -1f; break;
            case 2: h = +1f; break;
        }

        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        mummyTr.Translate(dir * 0.5f);

        SetReward(-0.001f);
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

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("TARGET"))
        {
            SetReward(+1.0f);
            EndEpisode();
        }
        if (coll.collider.CompareTag("DEAD_ZONE"))
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

}
