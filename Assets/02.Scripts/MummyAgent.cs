using UnityEngine;
using MLAgents;

public class MummyAgent : Agent
{
    public Transform targetTr;
    private Transform tr;
    private Rigidbody mummyRb;

    public Material rightMt;
    public Material wrongMt;
    private Material originMt;
    public MeshRenderer plane;

    //에이전트의 초기화 작업
    public override void Initialize()
    {
        tr = transform;
        originMt = plane.material;

        mummyRb = GetComponent<Rigidbody>();
    }

    //에피소드 시작시 호출
    public override void OnEpisodeBegin()
    {
        //스테이지(환경)를 랜덤 초기화
        //주인공 캐릭터의 물리 초기화
        mummyRb.velocity = Vector3.zero;
        mummyRb.angularVelocity = Vector3.zero;
        tr.localPosition = new Vector3(0.0f, 0.05f, 0.0f);

        //Target 위치를 랜덤하게 변경
        targetTr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f)
                                            , 0.5f
                                            , Random.Range(-4.0f, 4.0f));

        Invoke("RevertMaterial", 0.2f);
    }

    void RevertMaterial()
    {
        plane.material = originMt;
    }

    //주변의 환경정보를 수집
    public override void CollectObservations(MLAgents.Sensors.VectorSensor sensor)
    {
        //Brain 전달할 관측 데이터
        sensor.AddObservation(targetTr.localPosition);   //3
        sensor.AddObservation(tr.localPosition);         //3
        sensor.AddObservation(mummyRb.velocity.x);  //1
        sensor.AddObservation(mummyRb.velocity.z);  //1
    }

    //Brain으로 부터 전달된 명령을 수행하는 메소드
    public override void OnActionReceived(float[] vectorAction)
    {
        //액션 데이터(Brain으로 부터 전달받은 데이터), 정규화
        float h = Mathf.Clamp(vectorAction[0], -1f, 1f) * 2.0f;
        float v = Mathf.Clamp(vectorAction[1], -1f, 1f) * 2.0f;

        //이동할 방향벡터 계산
        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);

        mummyRb.AddForce(dir * 10.0f);

        SetReward(-0.001f);
    }


    // public override void Heuristic(float[] actionsOut)
    // {
    //     actionsOut[0] = Input.GetAxis("Horizontal");
    //     actionsOut[1] = Input.GetAxis("Vertical");
    // }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal"); //Left, Right Key
        action[1] = Input.GetAxis("Vertical");   //Up, Down Key

        return action;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("TARGET"))
        {
            SetReward(+1.0f);
            plane.material = rightMt;
            EndEpisode();
        }
        if (coll.collider.CompareTag("DEAD_ZONE"))
        {
            SetReward(-1.0f);
            plane.material = wrongMt;
            EndEpisode();
        }
    }


}
