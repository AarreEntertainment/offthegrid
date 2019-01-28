using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weatherControl : MonoBehaviour {

	public int dayGod(float day){
		int arraysize = 8;
		if (day < 60 && day > 280)
			arraysize += 2;
		return(int) Mathf.Floor (Random.Range (0, arraysize));
	}

	public ParticleSystem cloudLight;
	public ParticleSystem cloudDark;
	public ParticleSystem rain;
	public ParticleSystem snow;

	public GameObject sunRotator;
	public GameObject sun;

	public Text time;
	public Text date;

	public Sprite battery10;
	public Sprite battery20;
	public Sprite battery40;
	public Sprite battery60;
	public Sprite battery80;
	public Sprite batteryfull;

	public GameObject battery;
	public GameObject batteryLoading;
	public Text batterytext;
	public Text temperature;
	public Text hunger;
	public Text happiness;

	public float hungerDecay;
	public float batteryDecay;
	public float happinessDecay;
	public float tempdecay;


	public float hung;
	public float temp;
	public float happines;
	public float batter;

	float secondTimer= 1.0f;
	float hourSecs = 30f;
	float days = 0f;
	float hour = 14f;
	public GameObject processObject;

	public GameObject youLoose;

	public void restart()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public void youLose(){
		PlayerPrefs.SetFloat ("hunger", 0);
		youLoose.SetActive (true);
		Time.timeScale = 0;
	}

	void Update () {
		if (secondTimer > 0.0f) {
			secondTimer -= Time.deltaTime;

			if (process > 0) {
				if (!processObject.activeSelf)
					processObject.SetActive (true);
				process -= Time.deltaTime;
			} else {
				if (processObject.activeSelf)
					processObject.SetActive (false);
			}
		}
		else {
			string timetext;
			float timeval = 60 - hourSecs * 2;
			if (timeval < 10)
				timetext = "0" + timeval.ToString ();
			else {
				timetext = timeval.ToString ();

			}

			time.text = hour.ToString () + ":" + timetext;
			if (sun.GetComponent<Light> ().intensity>0){
				batteryValue(1 * (sun.GetComponent<Light> ().intensity - 1));
			}

			if(hung>0)
				setHunger (-hungerDecay);


			if(batter>0)
				eatBattery(batteryDecay);

			if (happines > 0)
				setHappiness (-happinessDecay);

			if (batter <= 0)
				happinessDecay = 1;
			else
				happinessDecay = 0.1f;
			
			PlayerPrefs.SetFloat ("hunger", hung);
			PlayerPrefs.SetFloat ("battery", batter);
			PlayerPrefs.SetFloat ("happiness", happines);

			hourSecs--;
			secondTimer = 1.0f;
		}

		if (hourSecs <= 0.0f) {
			hour++;
			hourSecs = 30f;
		}

		if (hour == 24f) {
			hour = 0f;
			days++;
			setWeather (dayGod (days));
			date.text = "Day " + days.ToString ();
		}
	}


	public void loadIcon(bool isloading){
		batteryLoading.SetActive (isloading);
	}

	public float process = 0;

	public void eatBattery(float amount){
		batter -= amount;
	}
	public void setProcess(float amount){
		process = amount;
	}
	public void setTemp (float tempp)
	{
		temp += tempp;	
		temperature.text = Mathf.Round(temp).ToString () + "°C";
	}

	public void setHunger(float hungr)
	{
		hung += hungr;
		if (hung > 100)
			hung = 100;
		hunger.text = Mathf.Round(hung).ToString() + "%";
	}

	public void setHappiness(float happy)
	{
		happines += happy;

		if (happines > 100)
			happines = 100;
		happiness.text = Mathf.Round(happines).ToString () + "%";
	}

	public void batteryValue(float value)
	{
		if (value < 0)
			value = 0;
		if (batter >= 100)
			value = 0;

		if (value == 0)
			loadIcon (false);
		else
			loadIcon (true);

		batter += value/5;
		batterytext.text = Mathf.Round(batter).ToString() + "%";
		if (batter > 80)
			battery.GetComponent<Image> ().sprite = batteryfull;
		else if (batter > 60)
			battery.GetComponent<Image> ().sprite = battery80;
		else if (batter > 40)
			battery.GetComponent<Image> ().sprite = battery60;
		else if (batter > 20)
			battery.GetComponent<Image> ().sprite = battery40;
		else if (batter > 10)
			battery.GetComponent<Image> ().sprite = battery20;
		else
			battery.GetComponent<Image> ().sprite = battery10;
	}

	// Use this for initialization
	void setSunLight(float sunLight)
	{
		if(sun.activeSelf)
			sun.GetComponent<Light> ().intensity = sunLight;
	}
		

	void setCloudProfile (int profile){
		ParticleSystem.ColorOverLifetimeModule colorover1;
		ParticleSystem.ColorOverLifetimeModule colorover2;
		Gradient grad = new Gradient ();
		switch (profile) {
		case 0:
			RenderSettings.fogDensity = 0.0f;
			cloudLight.Stop ();
			cloudDark.Stop ();
			break;
		case 1:
			RenderSettings.fogDensity = 0.0f;
			cloudLight.Play ();
			cloudDark.Stop ();
			colorover1 = cloudLight.colorOverLifetime;
			grad.SetKeys (new GradientColorKey[]{ new GradientColorKey (Color.white, 0.0f) }, 
				new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (0.3f, 0.2f),
					new GradientAlphaKey (0.3f, 0.8f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
			colorover1.color = grad;
			break;
		case 2:
			RenderSettings.fogDensity = 0.005f;
			cloudLight.Play ();
			cloudDark.Play ();
			colorover1 = cloudLight.colorOverLifetime;
			colorover2 = cloudDark.colorOverLifetime;
			grad.SetKeys (new GradientColorKey[]{ new GradientColorKey (Color.white, 0.0f) }, 
				new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (0.3f, 0.2f),
					new GradientAlphaKey (0.3f, 0.8f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
			colorover1.color = grad;
			grad.SetKeys (new GradientColorKey[]{ new GradientColorKey (Color.white, 0.0f) }, 
				new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (0.3f, 0.2f),
					new GradientAlphaKey (0.3f, 0.8f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
			colorover2.color = grad;

			break;
		case 3:
			RenderSettings.fogDensity = 0.01f;
			cloudLight.Play ();
			cloudDark.Play ();
			colorover1 = cloudLight.colorOverLifetime;
			colorover2 = cloudDark.colorOverLifetime;
			grad.SetKeys (new GradientColorKey[]{ new GradientColorKey (Color.white, 0.0f) }, 
				new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (0.3f, 0.2f),
					new GradientAlphaKey (0.3f, 0.8f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
			colorover1.color = grad;
			grad.SetKeys (new GradientColorKey[]{ new GradientColorKey (Color.white, 0.0f) }, 
				new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (0.8f, 0.2f),
					new GradientAlphaKey (0.8f, 0.8f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
			colorover2.color = grad;
			break;
		case 4:
			RenderSettings.fogDensity = 0.03f;
			cloudLight.Play ();
			cloudDark.Play ();
			colorover1 = cloudLight.colorOverLifetime;
			colorover2 = cloudDark.colorOverLifetime;
			grad.SetKeys (new GradientColorKey[]{ new GradientColorKey (Color.white, 0.0f) }, 
				new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (0.3f, 0.2f),
					new GradientAlphaKey (0.3f, 0.8f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
			colorover1.color = grad;
			grad.SetKeys (new GradientColorKey[]{ new GradientColorKey (Color.white, 0.0f) }, 
				new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (0.3f, 0.2f),
					new GradientAlphaKey (0.3f, 0.8f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
			colorover2.color = grad;
			break;
		}
	}

	void setRainProfile(int profile){
		ParticleSystem.EmissionModule emis;
		ParticleSystem.MainModule main;
		switch (profile) {
		case 0:
			rain.Stop ();
			snow.Stop ();
			break;
		case 1:
			rain.Play ();
			emis = rain.emission;
			emis.rateOverTime = 100;
			snow.Stop ();
			break;
		case 2:
			rain.Play ();
			emis = rain.emission;
			emis.rateOverTime = 300;
			snow.Stop ();
			break;
		case 3:
			rain.Play ();
			emis = rain.emission;
			emis.rateOverTime = 800;
			snow.Stop ();
			break;
		case 4:
			rain.Stop ();
			snow.Play ();
			emis = snow.emission;
			emis.rateOverTime = 100;
			main = snow.main;
			main.startSize = 0.1f;
			break;
		case 5:
			rain.Stop ();
			snow.Play ();
			emis = snow.emission;
			emis.rateOverTime = 300;
			main = snow.main;
			main.startSize = 0.5f;
			break;

		}
	}

	public void setWeather(int weatherProfile){
		switch (weatherProfile) {
		case 0:
			setSunLight (2f);
			setCloudProfile (0);
			setRainProfile (0);
			break;
		case 1:
			setSunLight (1.5f);
			setCloudProfile (1);
			setRainProfile (0);
			break;
		case 2:
			setSunLight (0.45f);
			setCloudProfile (2);
			setRainProfile (0);
			break;
		case 3: 
			setSunLight (0.02f);
			setCloudProfile (3);
			setRainProfile (0);
			break;
		case 4:
			setSunLight (0.45f);
			setCloudProfile (1);
			setRainProfile (1);
			break;
		case 5:
			setSunLight (0.2f);
			setCloudProfile (2);
			setRainProfile (2);
			break;
		case 6:
			setSunLight (0.2f);
			setCloudProfile (3);
			setRainProfile (3);
			break;
		case 7:
			setSunLight (0.2f);
			setCloudProfile (4);
			setRainProfile (0);
			break;
		case 8:
			setSunLight (0.45f);
			setCloudProfile (2);
			setRainProfile (4);
			break;
		case 9:
			setSunLight (0.45f);
			setCloudProfile (3);
			setRainProfile (5);
			break;
		}
	}


	void Start () {
		setWeather (dayGod(days));
	}
	
	// Update is called once per frame

}
