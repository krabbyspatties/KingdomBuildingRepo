using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Townsperson : MonoBehaviour
{
    public TownspersonData data;

    public enum State
    {
        Idle,
        Working
    }

    public State currentState = State.Idle;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (data != null && data.sprite != null)
            spriteRenderer.sprite = data.sprite;
    }

    public void AssignToCave(CaveInstance cave)
    {
        currentState = State.Working;
        cave.StartProduction();
    }

    public void UnassignFromCave(CaveInstance cave)
    {
        currentState = State.Idle;
        cave.StopProduction();
    }
}