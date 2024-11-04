using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Vuforia;
using System.IO;

public class GPSController : MonoBehaviour
{

    StreamWriter file;

    string message = "Initializing GPS";
    string bina_adi = "";
    float thisLat;
    float thisLong;
    float heading;
    float startingLat;
    float startingLong;

    float m_center_long = 37.86561506377567f;
    float m_center_lat = 32.53635060223918f;

    float s_center_long = 37.865565992359045f;
    float s_center_lat = 32.53695384947102f;

    float k_center_long = 37.865571564355136f;
    float k_center_lat = 32.53746415863819f;

    float a_center_long = 37.86437018841045f;
    float a_center_lat = 32.53740632646285f;


    float b_center_long = 37.86436158545007f;
    float b_center_lat = 32.53640378497506f;

    float c_center_long = 37.86296515927692f;
    float c_center_lat = 32.53633920693693f;

    float h_center_long = 37.866398230590974f;
    float h_center_lat = 32.53692823953483f;

    // hukuk fakültesi

    float h_sol_on_lat = 37.86625854696444f;
    float h_sol_on_long =  32.53637625461052f; 
    float h_sag_on_lat = 37.866225594913764f;
    float h_sag_on_long = 32.5375345600471f;
    float h_sol_arka_lat = 37.86653646158719f;
    float h_sol_arka_long = 32.53629398001576f;
    float h_sag_arka_lat = 37.8665001948857f;
    float h_sag_arka_long = 32.5376076063359f;


    // kütüphane

    float k_sol_on_lat = 37.86522575214186f;
    float k_sol_on_long = 32.537347238740075f; 
    float k_sag_on_lat = 37.86522252971064f;
    float k_sag_on_long = 32.537607115490594f;
    float k_sol_arka_lat = 37.86586443603999f;
    float k_sol_arka_long = 32.53733430542692f;
    float k_sag_arka_lat = 37.86588591872487f;
    float k_sag_arka_long = 32.53762139440261f;


    //sosyal hizmetler
    float s_sol_on_lat = 37.86534884972026f;
    float s_sol_on_long = 32.53672611220122f; 
    float s_sag_on_lat = 37.86535099800406f;
    float s_sag_on_long = 32.53717919575055f;
    float s_sol_arka_lat = 37.86573016911559f;
    float s_sol_arka_long = 32.53677373359489f;
    float s_sag_arka_lat = 37.865723724297155f;
    float s_sag_arka_long = 32.53715198352496f;


    // M binası
    float m_sol_on_lat = 37.86528794930003f;
    float m_sol_on_long = 32.53619010786683f; 
    float m_sag_on_lat = 37.865281504442926f;
    float m_sag_on_long = 32.53651937579156f;
    float m_sol_arka_lat = 37.86595957615202f;
    float m_sol_arka_long = 32.53621909068168f;
    float m_sag_arka_lat =37.86595420548675f ;
    float m_sag_arka_long = 32.53652794943753f;


    // A Blok
    float a_sol_on_lat = 37.86499280433043f;
    float a_sol_on_long = 32.53728538253246f; 
    float a_sag_on_lat = 37.86372264998187f;
    float a_sag_on_long =  32.53726589772489f;
    float a_sol_arka_lat = 37.865004619947634f;
    float a_sol_arka_long = 32.53758335639824f;
    float a_sag_arka_lat = 37.86371727915357f ;
    float a_sag_arka_long = 32.53754482303304f;


    // B Blok
    float b_sol_on_lat = 37.863720071800984f;
    float b_sol_on_long = 32.536519099961865f; 
    float b_sag_on_lat = 37.86498435039326f;
    float b_sag_on_long = 32.53657046307227f;
    float b_sol_arka_lat = 37.863712295886145f;
    float b_sol_arka_long = 32.5362039158991f;
    float b_sag_arka_lat = 37.86505951623832f;
    float b_sag_arka_long = 32.536242146340236f;


    // C Blok
    float c_sol_on_lat = 37.86229544573154f;
    float c_sol_on_long = 32.53644355577467f; 
    float c_sag_on_lat = 37.86281379516143f;
    float c_sag_on_long = 32.536442846959616f;
    float c_sol_arka_lat = 37.862292853709924f;
    float c_sol_arka_long = 32.53616448655243f;
    float c_sag_arka_lat =37.86281128271f;
    float c_sag_arka_long = 32.5361801568838f;



    public static float distance = 0;
    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //    public GameObject cube;

    void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(30, 30, 1000, 1000), message);

        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(30, 60, 1000, 1000), "Heading "+heading);
    }

    public void Reset()
    {

        startingLat = Input.location.lastData.latitude;
        startingLong = Input.location.lastData.longitude;
    }

    IEnumerator StartGPS()
    {
        message = "Starting";

        if (!Input.location.isEnabledByUser)
        {
            message = "Location not enabled by user";
            //           yield break;
        }

        Input.location.Start(5, 0);

        int maxWait = 5;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            message = "Timeout";
            yield break;
        }


        if (Input.location.status == LocationServiceStatus.Failed)
        {
            message = "Unabled to determine device location";
            yield break;
        }
        else
        {
            Input.compass.enabled = true;
            message = "Lat: " + Input.location.lastData.latitude +
              "\nLong: " + Input.location.lastData.longitude +
                      "\nHoriz Acc: " + Input.location.lastData.horizontalAccuracy +
                      "\nVert Acc: " + Input.location.lastData.verticalAccuracy +
                      "\n==========" +
                      "\nHeading: " + Input.compass.trueHeading;


            startingLat = Input.location.lastData.latitude;
            startingLong = Input.location.lastData.longitude;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGPS());

//        string path = Application.persistentDataPath + "/datalar.txt";
        string path = "datalar.txt";
        //Write some text to the test.txt file
        file = new StreamWriter(path, true);

    }

    void OnDisable()
    {
        file.Close();
    }

    // Update is called once per frame
    void Update()
    {
        DateTime lastUpdate = epoch.AddSeconds(Input.location.lastData.timestamp);
        DateTime rightNow = DateTime.Now;

        Boolean iceride = false;
        Boolean arkada = false;
        Boolean solarada = false;
        Boolean sagarada = false;

        float sol_angle_m_bina, sag_angle_m_bina;
        float sol_angle_s_bina, sag_angle_s_bina;
        float sol_angle_k_bina, sag_angle_k_bina;
        float sol_angle_a_bina, sag_angle_a_bina;
        float sol_angle_b_bina, sag_angle_b_bina;
        float sol_angle_c_bina, sag_angle_c_bina;
        float sol_angle_h_bina, sag_angle_h_bina;


        thisLat = Input.location.lastData.latitude;
        thisLong = Input.location.lastData.longitude;

        heading = Input.compass.trueHeading;

        string[] binalar = { "M-Bina", "Sosyal", "K�t�phane", "A Blok", "B Blok", "C Blok", "Hukuk" };

        float[] longs = { m_center_long, s_center_long, k_center_long, a_center_long, b_center_long, c_center_long, h_center_long };
        float[] lats = { m_center_lat, s_center_lat, k_center_lat, a_center_lat, b_center_lat, c_center_lat, h_center_lat };


        int ind = 0;
        float min = 6371000f;
        for (int i = 0; i < 7; i++)
        {

            float distance_m = Haversine(thisLat, thisLong, longs[i], lats[i]);
            if (distance_m < min)
            {
                min = distance_m;
                ind = i;
            }
        }

        float angle_bina = angle_vector(thisLat, thisLong, lats[ind], longs[ind]);

        message = "En Yak�n Bina " + binalar[ind] + "Uzakl�k " + min.ToString("0.0000")+" A�� "+angle_bina;


        if (thisLat < s_sol_on_lat)
            iceride = true;
        else
            iceride = false;

        if (thisLat > s_sol_on_lat && thisLong < s_sol_on_long && thisLat < s_sol_arka_lat)

            solarada = true;
        else
            solarada = false;

        if (thisLat > s_sol_arka_lat && thisLong < s_sag_arka_long)
             arkada = true;
        else
            arkada = false;

        if (thisLat < s_sag_arka_lat && thisLong > s_sag_arka_long && thisLat > s_sag_on_lat)
            sagarada = true;
        else
            sagarada = false;

        if (iceride == true)
        {


            ///// M Blok 
            if (thisLat < m_sol_on_lat)
                sol_angle_m_bina = angle_vector(thisLat, thisLong, m_sol_on_lat, m_sol_on_long);
            else
                sol_angle_m_bina = angle_vector(thisLat, thisLong, m_sag_on_lat, m_sag_on_long);

            if (thisLat < m_sol_on_lat)
                sag_angle_m_bina = angle_vector(thisLat, thisLong, m_sag_on_lat, m_sag_on_long);
            else
                sag_angle_m_bina = angle_vector(thisLat, thisLong, m_sag_arka_lat, m_sag_arka_long);


            ///// Sosyal 

                sol_angle_s_bina = angle_vector(thisLat, thisLong, s_sol_on_lat, s_sol_on_long);

                sag_angle_s_bina = angle_vector(thisLat, thisLong, s_sag_on_lat, s_sag_on_long);

            ///// K�t�phane

            if (thisLat < m_sol_on_lat)
                sol_angle_k_bina = angle_vector(thisLat, thisLong, k_sol_on_lat, k_sol_on_long);
            else
                sol_angle_k_bina = angle_vector(thisLat, thisLong, k_sol_arka_lat, k_sol_arka_long);

            if (thisLat < m_sol_on_lat)
                sag_angle_k_bina = angle_vector(thisLat, thisLong, k_sag_on_lat, k_sag_on_long);
            else
                sag_angle_k_bina = angle_vector(thisLat, thisLong, k_sol_on_lat, k_sol_on_long);


            ///// A Blok
            sol_angle_a_bina = angle_vector(thisLat, thisLong, a_sol_on_lat, a_sol_on_long);
            sag_angle_a_bina = angle_vector(thisLat, thisLong, a_sag_on_lat, a_sag_on_long);

            ///// B Blok
            sol_angle_b_bina = angle_vector(thisLat, thisLong, b_sol_on_lat, b_sol_on_long);
            sag_angle_b_bina = angle_vector(thisLat, thisLong, b_sag_on_lat, b_sag_on_long);

            ///// C Blok
            sol_angle_c_bina = angle_vector(thisLat, thisLong, c_sol_on_lat, c_sol_on_long);
            sag_angle_c_bina = angle_vector(thisLat, thisLong, c_sag_on_lat, c_sag_on_long);


            if (heading > sol_angle_m_bina && heading < sag_angle_m_bina && thisLong < s_sol_on_long)
                bina_adi = "M Binas�"; else
            if (thisLong > s_sol_on_long && heading > sol_angle_m_bina && 
                    sag_angle_m_bina > sol_angle_s_bina && heading < sol_angle_s_bina)
                bina_adi = "M Binas�"; else
            if (heading > sol_angle_s_bina && heading < sag_angle_s_bina)
              bina_adi = "Sosyal"; else
            if (heading > sol_angle_k_bina && heading < sag_angle_k_bina && thisLong > s_sag_on_long)
    
                  bina_adi = "K�t�phane"; else

    if (thisLong < s_sag_on_long && heading < sag_angle_k_bina 
                && sag_angle_s_bina > sol_angle_k_bina && heading > sag_angle_s_bina)
            bina_adi = "K�t�phane"; else
         if (heading > sol_angle_a_bina && heading < sag_angle_a_bina)
            bina_adi = "A Blok"; else
    if (heading > sol_angle_b_bina && heading < sag_angle_b_bina)
      bina_adi = "B Blok"; else
    if (heading > sol_angle_c_bina && heading < sag_angle_c_bina)
      bina_adi = "C Blok"; else
                bina_adi = "";

        } // if (icerde==true)

        if (solarada == true)
        {
            sol_angle_m_bina = angle_vector(thisLat, thisLong, m_sag_on_lat, m_sag_on_long);
            sag_angle_m_bina = angle_vector(thisLat, thisLong, m_sag_arka_lat, m_sag_arka_long);

            sol_angle_s_bina = angle_vector(thisLat, thisLong, s_sol_arka_lat, s_sol_arka_long);
            sag_angle_s_bina = angle_vector(thisLat, thisLong, s_sol_on_lat, s_sol_on_long);

            sol_angle_h_bina = angle_vector(thisLat, thisLong, h_sol_on_lat, h_sol_on_long);
            sag_angle_h_bina = angle_vector(thisLat, thisLong, s_sol_arka_lat, s_sol_arka_long);

            sol_angle_b_bina = angle_vector(thisLat, thisLong, b_sol_on_lat, b_sol_on_long);
            sag_angle_b_bina = angle_vector(thisLat, thisLong, b_sag_arka_lat, b_sag_arka_long);

            if (sag_angle_b_bina > sol_angle_m_bina)
                sag_angle_b_bina = sol_angle_m_bina;

            if (heading > sol_angle_m_bina && heading < sag_angle_m_bina)        
                bina_adi = "M Blok"; else
            if (heading > sol_angle_s_bina && heading < sag_angle_s_bina)        
                bina_adi = "Sosyal"; else
            if (heading > sol_angle_h_bina && heading < sag_angle_h_bina)        
                bina_adi = "Hukuk"; else
            if (heading > sol_angle_b_bina && heading < sag_angle_b_bina)        
                bina_adi = "B Blok"; else
                bina_adi = "";
        }


        if (arkada == true)
        {
            sol_angle_h_bina = angle_vector(thisLat, thisLong, h_sol_on_lat, h_sol_on_long);
            sag_angle_h_bina = angle_vector(thisLat, thisLong, h_sag_on_lat, h_sag_on_long);

            sol_angle_m_bina = angle_vector(thisLat, thisLong, m_sag_on_lat, m_sag_on_long);
            sag_angle_m_bina = angle_vector(thisLat, thisLong, m_sol_arka_lat, m_sol_arka_long);

            sol_angle_s_bina = angle_vector(thisLat, thisLong, s_sag_arka_lat, s_sag_arka_long);
            sag_angle_s_bina = angle_vector(thisLat, thisLong, s_sol_arka_lat, s_sol_arka_long);

            sol_angle_k_bina = angle_vector(thisLat, thisLong, k_sag_arka_lat, k_sag_arka_long);
            sag_angle_k_bina = angle_vector(thisLat, thisLong, s_sag_arka_lat, s_sag_arka_long);

            if (heading > sol_angle_h_bina && heading < sag_angle_h_bina)        
                bina_adi = "Hukuk"; else
            if (heading > sol_angle_m_bina && heading < sag_angle_m_bina)        
              bina_adi = "M Blok"; else
            if (heading > sol_angle_s_bina && heading < sag_angle_s_bina)        
                bina_adi = "Sosyal"; else
            if (heading > sol_angle_k_bina && heading < sag_angle_k_bina)        
                bina_adi = "K�t�phane"; else
                bina_adi = "";
        }

        if (sagarada == true)
        {
            sol_angle_h_bina = angle_vector(thisLat, thisLong, h_sol_on_lat, h_sol_on_long);
            sag_angle_h_bina = angle_vector(thisLat, thisLong, h_sag_on_lat, h_sag_on_long);

            sol_angle_s_bina = angle_vector(thisLat, thisLong, s_sag_on_lat, s_sag_on_long);
            sag_angle_s_bina = angle_vector(thisLat, thisLong, s_sag_arka_lat, s_sag_arka_long);

            sol_angle_k_bina = angle_vector(thisLat, thisLong, k_sol_arka_lat, k_sol_arka_long);
            sag_angle_k_bina = angle_vector(thisLat, thisLong, k_sol_on_lat, k_sol_on_long);

            sol_angle_a_bina = angle_vector(thisLat, thisLong, a_sol_arka_lat, a_sol_arka_long);
            sag_angle_a_bina = angle_vector(thisLat, thisLong, a_sag_on_lat, a_sag_on_long);

            if (heading > sol_angle_s_bina && heading < sag_angle_s_bina)        
                bina_adi = "Sosyal"; else
            if (heading > sol_angle_k_bina && heading < sag_angle_k_bina)
                bina_adi = "K�t�phane"; else
            if (heading > sol_angle_h_bina && heading < sag_angle_h_bina && sol_angle_h_bina > sag_angle_s_bina)
                bina_adi = "Hukuk"; else
            if (heading > sag_angle_s_bina && heading < sag_angle_h_bina && sol_angle_h_bina < sag_angle_s_bina)
                bina_adi = "Hukuk"; else
            if (heading > sol_angle_a_bina && heading < sag_angle_a_bina)
                bina_adi = "A Blok";
                bina_adi = "";
        }


        String satir = thisLat + "," + thisLong + "," + heading + "," + bina_adi;

        TextMesh ctext1_var = GameObject.Find("Text").GetComponent<TextMesh>();
        ctext1_var.GetComponent<TextMesh>().text = "G�r�nen Bina \n\n"+bina_adi;

        file.WriteLine(satir);

        // message = "Lat: " + thisLat +
        //   "\nLong: " +  thisLong +
        //           "\nLast Update: " + lastUpdate.ToString("HH:mm:ss") +
        //           "\nRight Now: " + rightNow.ToString("HH:mm:ss") +
        //           "\n==========" +
        //           "\nHeading: " + Input.compass.trueHeading;


        // message = "Distance Travelled: " + distance +
        //         "\nUpdate Time: " + lastUpdate.ToString("HH:mm:ss") + 
        //         "\nNow: " + rightNow.ToString("HH:mm:ss");


        //        if (thisLat > 30.0)
        //            cube.SetActive(true);
        //        else
        //            cube.SetActive(false);
    }

    float Haversine(float lat1, float long1, float lat2, float long2)
    {
        float earthRad = 6371000;
        float lRad1 = lat1 * Mathf.Deg2Rad;
        float lRad2 = lat2 * Mathf.Deg2Rad;
        float dLat = (lat2 - lat1) * Mathf.Deg2Rad;
        float dLong = (long2 - long1) * Mathf.Deg2Rad;
        float a = Mathf.Sin(dLat / 2.0f) * Mathf.Sin(dLat / 2.0f) +
                    Mathf.Cos(lRad1) * Mathf.Cos(lRad2) *
                    Mathf.Sin(dLong / 2.0f) * Mathf.Sin(dLong / 2.0f);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        return earthRad * c;

    }

    float vektor_mag(float x, float y)
    {
        double karekok = Math.Sqrt(x * x + y * y);
        return (float)karekok;
    }

    float cos_angle(float vektor1_x, float vektor1_y, float vektor2_x, float vektor2_y)
    {
        float pay = vektor1_x * vektor2_x + vektor1_y * vektor2_y;
        float vektor1_mag = vektor_mag(vektor1_x, vektor1_y);
        float vektor2_mag = vektor_mag(vektor2_x, vektor2_y);

        float payda = vektor1_mag * vektor2_mag;

        double bolme = pay / payda;
        double cos = Math.Acos(bolme);
        cos = cos * 180 / Math.PI;
        return (float) cos;
    }

    float angle_vector(float tel_lat, float tel_long, float bina_lat, float bina_long)
    {

        float kuzey_uc_lat = 37.866547f;

        float kuzey_vektor_x = 0.0f;
        float kuzey_vektor_y = kuzey_uc_lat - tel_lat;

        float vektor_x = bina_long - tel_long;
        float vektor_y = bina_lat - tel_lat;

        return cos_angle(kuzey_vektor_x, kuzey_vektor_y, vektor_x, vektor_y);
    }


    float ucgen_angle(float tel_lat, float tel_long, float bina_sol_lat, float bina_sol_long,
        float bina_sag_lat, float bina_sag_long, string bina, Boolean iceride)
    {
        float height = 0;
        float floor;
        float ucgen_alan;

        float ab = Math.Abs(Haversine(tel_lat, tel_long, bina_sol_lat, bina_sol_long));
        float ac = Math.Abs(Haversine(tel_lat, tel_long, bina_sag_lat, bina_sag_long));
        float ab_carpi_ac = ab * ac;

        float sin_alfa;


        floor = Math.Abs(Haversine(bina_sol_lat, bina_sol_long, bina_sag_lat, bina_sag_long));

        ucgen_alan = height * floor;

        sin_alfa = ucgen_alan / ab_carpi_ac;

        double alfa = Math.Round(Math.Asin(sin_alfa), 2);

        return (float)alfa;
    }

}