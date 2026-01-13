using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
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

    public void Roll(int numberOfDie)
    {
        if (dice.Count() > 0)
        {
            dice.ForEach(x => Destroy(x));
        }
        HUDController.instance.EnableDice();
        dice = CreateDice(numberOfDie);
        ShowDieResult();
        Debug.Log("ready to start");
        StartCoroutine(DelayRoll(dice));
    }
    private IEnumerator DelayRoll(List<GameObject> dice)
    {
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
                spinner.RollToFace(Random.Range(1, 7));


                yield return new WaitForSeconds(timeBetweenRolls);
                currentDie = die;
            }
        }
    }
    private void ShowDieResult()
    {

    }

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
