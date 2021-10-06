using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class OpeningManager : ChemicalSummonManager
{
    public static OpeningManager Instance { get; protected set; }

    [SerializeField]
    float generateSpan;
    [SerializeField]
    float cardScale, cardUpSpeed, cardRotationSpeed;
    [SerializeField]
    Transform generateLeftAnchor, generateRightAnchor;

    //data
    List<Substance> substances;
    float waitedTime;
    void Awake()
    {
        substances = Substance.GetAll();
        ManagerInit(Instance = this);
    }

    // Update is called once per frame
    void Update()
    {
        waitedTime += Time.deltaTime;
        if(waitedTime > generateSpan)
        {
            waitedTime = 0;
            SubstanceCard card = SubstanceCard.GenerateSubstanceCard(substances.GetRandomElement());
            card.transform.SetParent(transform);
            card.transform.position = Vector3.Lerp(generateLeftAnchor.position, generateRightAnchor.position, Random.Range(0F, 1F));
            card.transform.localScale = cardScale * Vector3.one;
            KineticMove mover = card.gameObject.AddComponent<KineticMove>();
            mover.positionMove = Vector3.up * cardUpSpeed * Random.Range(0.5F, 1.5F);
        }
    }
}
