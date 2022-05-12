

# ESGame Unity SDK !

This document demonstrades ESGame SDK for IOS.


# Prequiresites

- Xcode
- ESGame client id
- ESGame client secret
- GoogleService-Info.plist file
- Facebook id and facebook client token
- Appsflyer development key
# Install
- Download the lastest esgame sdk for ios (https://github.com/andyesgame/esgamesdkiosexample/releases) and extract to your computer
- Link ESSDK to your pod file, like this:

		pod 'ESSDK', :path => '../sdk/' 

- Run :
	
	
		pod install
# Functionality
SDK provide some functionality below:
- ESGame normal login: use email/account and password
- Fast login : use deviceId 
- Sign-in with Apple
- Log-in with Facebook
- Log-in with Google
- Apple in-app purchase
- Third-party payment (should only use with non Google version)
- Analystic ( Firebase, Facebook, Appslyer)

# SDK Config

## es_property.plist
ESGame has few parameters
|Property               |Description                          |Provider                         |
|----------------|-------------------------------|-----------------------------|
|es_appflyer_dev_key|`Appsflyer development key`            |ESGame|
|es_client_id|`Esgame client id`            |ESGame|
|es_client_secret|`Esgame client secret`            |ESGame|
|es_gg_client_id|`Google client id, we can get it from key CLIENT_ID from google info plist file with`            |ESGame|

# Info.plist
You need modify some attributes in Info.plist

- Add facebook schema (format fb+ facebookId) and google resversed id (you can get it from key REVERSED_CLIENT_ID in GoogleService-Info.plist) to  CFBundleURLSchemes , for example
		
		<array>
		<dict>
			<key>CFBundleURLSchemes</key>
			<array>
				<string>fb682118789108627</string>
				<string>com.googleusercontent.apps.1034628364602-0sbkc4vbcrp2uj1cpedbfroj8f89vv51</string>
			</array>
		</dict>
		</array>

- Add facebook's id to FacebookAppID, for example :

		<key>FacebookAppID</key>
		<string>682118789108627</string>
		<key>FacebookDisplayName</key>
		<string>$(PRODUCT_NAME)</string>
- Add Facebook application queries schemas:

		<key>LSApplicationQueriesSchemes</key>
		<array>
		<string>fbapi</string>
		<string>fbapi20130214</string>
		<string>fbapi20130410</string>
		<string>fbapi20130702</string>
		<string>fbapi20131010</string>
		<string>fbapi20131219</string>
		<string>fbapi20140410</string>
		<string>fbapi20140116</string>
		<string>fbapi20150313</string>
		<string>fbapi20150629</string>
		<string>fbapi20160328</string>
		<string>fbauth</string>
		<string>fb-messenger-share-api</string>
		<string>fbauth2</string>
		<string>fbshareextension</string>
		</array>
- Add NSCameraUsageDescription,NSUserTrackingUsageDescription,SKAdNetworkItems:
	
		<key>NSCameraUsageDescription</key>
		<string>Chúng tôi sử dụng camera để update avatar ingame và tính năng báo lỗi.</string>
		<key>NSUserTrackingUsageDescription</key>
		<string>$(PRODUCT_NAME) cần xin quyền AppTrackingTransparency để giúp bạn trải nghiệm trò chơi tốt hơn và chia sẻ nội dung cập nhật mới nhất thông qua quảng cáo cá nhân</string>
		<key>SKAdNetworkItems</key>
		<array>
			<dict>
				<key>SKAdNetworkIdentifier</key>
				<string>v9wttpbfk9.skadnetwork</string>
			</dict>
			<dict>
				<key>SKAdNetworkIdentifier</key>
				<string>n38lu8286q.skadnetwork</string>
			</dict>
		</array>

# Coding
## Init sdk:
- ApplicationDelegate:

	    - (**BOOL**)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions{
		[ESGameSDK  application:application 		didFinishLaunchingWithOptions:launchOptions];
			return  YES;
		}
		- (void)applicationDidBecomeActive:(UIApplication *)application{
			[ESGameSDK  applicationDidBecomeActive:application];
		}
		- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options {
			return [ESGameSDK application:application openURL:url options:options];
		}
		- (**BOOL**)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(**id**)annotation{
			return [ESGameSDK application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
		}
- ViewController: 
	Make Your view's controller implement SdkDelegate:

		@import ESSDK;
		@interface  ViewController : UIViewController<SdkDelegate>
		@end
	
	init in viewDidLoad method:

		- (void) viewDidLoad{
			[super  viewDidLoad];
			ESGameSDK *esgameSdk = [ESGameSDK sharedObject];
			esgameSdk.delegate = self;
			[esgameSdk init:self];
		}	

	Implement ESGame's callback:


- Login success callback

	  - (void)responseLogin:(BOOL)isSuccess :(nonnull NSString *)message :(NSInteger)errorCode :(nonnull User *)user 
|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|isSuccess|Bool            |user login state (authorized or unauthorized)|
|message|String            |message from server|
|errorCode|Integer            |response code from server|
|user|User            |User information|

- Log-out callback
	
		 -(void)responseLogout

- Apple inapp-purchase  callback

		-(**void**)paymentSuccess:(SKPaymentTransaction*)transaction
|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|transaction|SKPaymentTransaction            |Transaction's information|


## Command

ESGame'SDK provide some methods:
- Log-in
	
	- (void)login:(UIViewController *)view
ESGame will open Login view if user was not login in the past, or let user login.
- Log-out
	
	-(void)logout
Let user log-out.
- In-app purchase

		- (void)buyProduct:(NSString *)productId :(NSString *)server_id :(NSString *)player_id :(NSString *)extraData :(UIViewController *)rootView

|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|productID|String            |product's identifier|
|server_id|String            |Server' identifier|
|player_id|String            |ESGame Player's identifier|
|extra_data|String            |ESGame Transaction's information|
|rootView|UIViewController            |root's view controller which sdk's will be display|

- Send tracking analystic event:
	
		-(void) trackEvent : (NSString *) eventName :(NSDictionary *) data;

|Property               |Type                          |Description                         |
|----------------|-------------------------------|-----------------------------|
|eventName|NSString            |event's name|
|data|NSDictionary            |event's data|

  
