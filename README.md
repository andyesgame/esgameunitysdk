


# ESGame Unity SDK !

This document demonstrades ESGame SDK for IOS. It bases on ESGame android SDK (https://github.com/andyesgame/esgamesdkiosexample#readme) and ESGame iOS SDK (https://github.com/andyesgame/esgamesdkandroidexample#readme).


# Prequiresites

- Xcode
- ESGame client id
- ESGame client secret
- GoogleService-Info.plist file
- Facebook id and facebook client token
- Appsflyer development key
# Install
- Download the lastest esgame sdk for unity (https://github.com/andyesgame/esgameunitysdk/releases) and extract to your computer
- Import ESGame unity package to your project.
- Download ESG iOS SDK ( https://github.com/andyesgame/esgamesdkiosexample/releases) and change ipod path in /Assets/ESDependency.xml , for example :

		<iosPods>
		    <iosPod name="ESSDK" path="../essdkios/sdk/">
		    </iosPod>
		</iosPods>

# Functionality
SDK provide some functionality below:
- ESGame normal login: use email/account and password
- Fast login : use deviceId 
- Sign-in with Apple
- Log-in with Facebook
- Log-in with Google
- Apple in-app purchase
- Google in-app billing
- Third-party payment (should only use with non Google version)
- Analystic ( Firebase, Facebook, Appslyer)

# SDK Config

## /Assets/esgconfig/esg_config.json
Create or modify esg_config.json and put it to  /Assets/esgconfig  folder, then asign it to config file property in ESGameSDK script inside /Assets/ESGameSdk/ESSDKScene
ESGame has few parameters
|Property               |Description                          |Provider                         |
|----------------|-------------------------------|-----------------------------|
|schemaStandAlone|`Your app scheme (On desktop platform)`            |CP|
|clientIdAndroid|`Esgame client id for Android`            |ESGame|
|clientSecretAndroid|`Esgame client secret for Android`            |ESGame|
|clientIdIos|`Esgame client id for iOS`            |ESGame|
|clientSecretIos|`Esgame client secret for iOS`            |ESGame|
|fbId|`Facebook client id`            |ESGame|
|fbClientToken|`Facebook client secret`            |ESGame|
|ggClientIdAndroid|`Google client id for Android`            |ESGame|
|appFlyerKey|`Appsflyer development key`            |ESGame|
|appFlyerAndroidId|`Appsflyer client id for Android`            |ESGame|
|appFlyerIosId|`Appsflyer client id for iOS`            |ESGame|
|appFlyerWindowId|`Appsflyer client id for Window platform`            |ESGame|
|appleId|`Apple client id`            |ESGame|


# Coding
## Init sdk:
- Create you controller script and add below lines to it:

	    void Start()
	    {
	        Screen.orientation = ScreenOrientation.LandscapeLeft;
	        /**
	         * set it if you wwant open esgame auto login behavior or not
	         */
	        ESGameSDK.autoLogin = false;
	        /**
	         * Deprecated , use if you want use unity method to get device id .Usefull if you have already production mode
	         */
	        ESGameSDK.useAndroidUnityDeviceId = true;
	        /**
	        * Deprecated , use if you want use unity method to get device id .Usefull if you have already production mode
	        */
	        ESGameSDK.useIosUnityDeviceId = true;
	        StartCoroutine(this.waitESSDKInit());
	    }
	    private IEnumerator waitESSDKInit()
	    {
	        yield return new WaitForEndOfFrame();
	        if (ESGameSDK.instance != null)
	        { 
	            ESGameSDK.instance.loginSuccessEvent.AddListener(this.onUserSignIn);
	            ESGameSDK.instance.loginFailureEvent.AddListener(this.onUserSignInError);
	            ESGameSDK.instance.logOutEvent.AddListener(this.onUserSignOut);
	            ESGameSDK.instance.billingEvent.AddListener(this.ESBillingEvent);
	            ESGameSDK.instance.uiEvent.AddListener(this.ESUIEvent);
	            /**
	             * Open login behavio , if usser token is available then sdk wwil notify login success event,.
	             * If user tojen is unavailble then sdk wil open login view
	             */
	            ESGameSDK.instance.login();
	        }
	        else
	        {
	            StartCoroutine(this.waitESSDKInit());
	        }
		    }
		    private void ESUIEvent(bool changed)
		    {
		    }
		    private void OnDestroy()
		    {
				ESGameSDK.instance.loginSuccessEvent.RemoveListener(this.onUserSignIn);   													ESGameSDK.instance.loginFailureEvent.RemoveListener(this.onUserSignInError);
		      ESGameSDK.instance.logOutEvent.RemoveListener(this.onUserSignOut);
		      ESGameSDK.instance.billingEvent.RemoveListener(this.ESBillingEvent);
		      ESGameSDK.instance.uiEvent.RemoveListener(this.ESUIEvent);
		    }
		    public void onUserSignIn(ESUser eSUser)
		    {
		    }
		    public void onUserSignInError(ESErrorEvent eSErrorEvent)
		    {
		    }
		    public void ESBillingEvent(ESBillingResult billingResult)
		    {
		    }	
		    public void onUserSignOut()
		    {
		    }	

	Implement ESGame's callback:


- Login success callback

	  void onUserSignIn(ESUser eSUser)
|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|eSUser|ESUser            |user's information|
- Login error callback

	  void onUserSignInError(ESErrorEvent event)
|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|event|ESErrorEvent            |error's information|
- Log-out callback
	
		void onUserSignOut()

- Payment  callback

		void ESBillingEvent(ESBillingResult billingResult)
|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|billingResult|ESBillingResult            |Transaction's information|


## Command

ESGame'SDK provide some methods:
- Log-in
	
	 ESGameSDK.instance.login()
ESGame will open Login view if user was not login in the past, or let user login.
- Log-out
	
	ESGameSDK.instance.logOut();
Let user log-out.
- In-app purchase ( Google and Applge)

		ESGameSDK.instance.billing(productId, serverId, playerId, extra);

|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|productID|String            |product's identifier|
|server_id|String            |Server' identifier|
|player_id|String            |ESGame Player's identifier|
|extra_data|String            |ESGame Transaction's information|
- Web purchase ( Third party payment, use for non google )

		ESGameSDK.instance.webBilling(productId, serverId, playerId, extra)

|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|productID|String            |product's identifier|
|server_id|String            |Server' identifier|
|player_id|String            |ESGame Player's identifier|
|extra_data|String            |ESGame Transaction's information|

- Send tracking analystic event:
	
		 ESGameSDK.instance.trackingEvent(eventName, eventData)

|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|eventName|string            |event's name|
|eventData|Dictionary<string, object>            |event's data|

  
