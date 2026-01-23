using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
    public AudioClip rollDiceSound;
    [SerializeField] GameObject diePrefab;
    [SerializeField] Transform successDieContainer;
    [SerializeField] float timeBetweenRolls;
    private Vector3 targetScale = new Vector3(1, 1, 1);
    private List<GameObject> dice = new List<GameObject>();
    void Start()
    {
        // spinner = dieCube.GetComponent<DiceSpinner>();
    }
    void Awake()
    {
        instance = this;
    }

    public void Roll(int numberOfDie, System.Action<DiceResult> onFinished)
    {
        if (dice.Count() > 0)
        {
            ClearDice();
        }
        HUDController.instance.EnableDice();
        dice = CreateDice(numberOfDie);
        AudioSource.PlayClipAtPoint(rollDiceSound, GameObject.FindGameObjectWithTag("Player").transform.position);
        StartCoroutine(DelayRoll(dice, onFinished));
    }
    private IEnumerator DelayRoll(List<GameObject> dice, System.Action<DiceResult> onFinished)
    {
        List<int> diceRolls = new List<int>();
        GameObject currentDie = null;
        GameObject lastDie = dice.Last();
        while (currentDie != lastDie)
        {
            foreach (GameObject die in dice)
            {

                die.SetActive(true);
                // Vector3 initialScale = die.transform.localScale;
                // die.transform.localScale = Vector3.Lerp(die.transform.position, initialScale * 2f, 0.5f * Time.deltaTime);
                DiceSpinner spinner = die.GetComponentInChildren<DiceSpinner>();
                int dieRoll = Random.Range(1, 7);
                diceRolls.Add(dieRoll);
                spinner.RollToFace(dieRoll, () =>
                {
                    if (die == dice.Last())
                    {
                        onFinished.Invoke(GetDiceRollResults(diceRolls));
                    }
                });


                yield return new WaitForSeconds(timeBetweenRolls);
                currentDie = die;
            }
        }
    }
    private DiceResult GetDiceRollResults(List<int> diceRolls)
    {
        int numberOfSixes = diceRolls.Count(x => x == 6);
        DiceSuccessState state = DiceSuccessState.FAILURE;
        if (numberOfSixes >= 3)
        {
            state = DiceSuccessState.CRITICAL_SUCCESS;
        }
        else if (numberOfSixes < 3 && numberOfSixes >= 1)
        {
            state = DiceSuccessState.LIMITED_SUCCESS;
        }
        return new DiceResult { numberOfSuccesses = numberOfSixes, result = state };
    }

    public void ClearDice() =>
        dice.ForEach(x => Destroy(x));

    private List<GameObject> CreateDice(int numberOfDie)
    {
        List<GameObject> dice = new List<GameObject>();
        for (int die = 0; die < numberOfDie; die++)
        {
            GameObject newDie = Instantiate(diePrefab, successDieContainer);
            newDie.SetActive(false);
            // newDie.transform.localScale = new Vector3(10, 10, 10);
            dice.Add(newDie);
        }
        return dice;
    }

}
