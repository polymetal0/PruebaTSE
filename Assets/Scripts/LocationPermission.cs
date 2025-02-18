using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class LocationPermission : MonoBehaviour
{
    [SerializeField]
    private Image locGrantedImg;
    [SerializeField]
    private Text locGrantedTxt;
    [SerializeField]
    private Text locationTxt;

    void Awake()
    {
        /* FineLocation has higher precission than 
         * CoarseLocation, so we'll go with that */
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            locGrantedImg.color = Color.green;
            locGrantedTxt.text = "GRANTED";
            Input.location.Start();
            StartCoroutine(GetLocationData());
            Debug.Log("permission granted");
        }
        else
        {
            locGrantedImg.color = Color.red;
            locGrantedTxt.text = "REFUSED";
            Debug.Log("permission denied");
        }
    }

    IEnumerator GetLocationData()
    {
        /* Wait in case location service is still initalizing */
        while (Input.location.status == LocationServiceStatus.Initializing) {
            yield return new WaitForSeconds(1);
        }
        /* Display GPS coord. each second if location service is running */
        while (Input.location.status == LocationServiceStatus.Running)
        {
            locationTxt.text = "Location: " + Input.location.lastData.latitude + ", " + Input.location.lastData.longitude;
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }
}
