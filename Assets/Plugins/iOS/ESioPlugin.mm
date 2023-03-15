

#import "ESWebViewController.h"
#import "ESWebViewDelegate.h"
#import <AppTrackingTransparency/ATTrackingManager.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import "ESioPlugin.h"
#import "ESGameSDK.h"

ESioPlugin *obj;
 @implementation ESioPlugin
+ (id) shareObject {
    if (obj == nil) {
        obj = [[ESioPlugin alloc] init];
        
    }
    return obj;
}
 
+(void)login{
    NSLog(@"dfslogin");
    [[ESGameSDK sharedObject] getUserInfo];
}
+(void)logout{
    [[ESGameSDK sharedObject] logout];
}
+(void)checkSession{
    [[ESGameSDK sharedObject] login:UnityGetGLViewController()];
}
+(void)startInGameMain{
    [[ESGameSDK sharedObject] startInGameMain];
}

+(void)deleteAccount{
    [[ESGameSDK sharedObject] deleteAccount];
}
+(void)billing:(NSString *) productId :(NSString *)serverId :(NSString *)playerId :(NSString *)extraData {
    [[ESGameSDK sharedObject]buyProduct:productId :serverId :playerId :extraData :UnityGetGLViewController()];
}
+(void)setDeviceManagement:(NSString *) deviceId{
    [[ESGameSDK sharedObject] setDeviceManaged:deviceId];
}
+(void)config:(NSString *) appsflyerId :(NSString *)clientId :(NSString *)clientSecret :(NSString *)ggId :(NSString *)appleId {
    ESGameSDK *esgameSdk = [ESGameSDK sharedObject];
    esgameSdk.delegate = self;
    [esgameSdk init:UnityGetGLViewController()];
    [ESGameSDK config:appsflyerId :clientId :clientSecret :ggId :appleId];
}
+(void)trackingEvent:(NSString *) eventName :(NSString *) mData{
    NSError *error = nil;
    NSLog(@"data %@",mData);
    NSData *data = [mData dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData:data
                                                                 options:kNilOptions
                                                                   error:&error];
    [[ESGameSDK sharedObject] trackEvent:eventName :jsonResponse];
}
+(void)setFloatingButtonEnable:(bool) floatingButtonEnable {
    [[ESGameSDK sharedObject] setFloatingButtonEnable:floatingButtonEnable];
}
+(void)initProducts:(NSString *)productString{
    NSArray<NSString *> *array = nil;
    if([productString containsString:@","]){
        array = [productString componentsSeparatedByString:@","];
    }else{
        array = @[productString];
    }
    [[ESGameSDK sharedObject] setProductItems:array];
}

+ (void)responseLogin:(BOOL)isSuccess :(nonnull NSString *)message :(NSInteger)errorCode :(nonnull User *)user {
  printf("----------------callback--login");
  NSLog(@"%@", message);
  NSLog(isSuccess ? @"YES" : @"NO");
  NSLog(@"%ld",(long)errorCode);
  NSLog(@"%@",user);
    if (isSuccess) {
        /**
         @property (nonatomic) NSInteger id;
         @property (nonatomic) NSString *name;
         @property (nonatomic) NSInteger active;
         @property (nonatomic) NSString *address;
         @property (nonatomic) NSString *birthday;
         @property (nonatomic) NSString *email;
         @property (nonatomic) NSString *full_name;
         @property (nonatomic) NSString *gender;
         @property (nonatomic) NSString *national_id;
         @property (nonatomic) NSString *phone;
         @property (nonatomic) NSString *provider;
         @property (nonatomic) bool is_new;
         */
        NSString *type = user.loginType;
        NSDictionary *userDict = @{
            @"id":@(user.id),
            @"name":user.name,
            @"active":@(user.active),
            @"address":user.address,
            @"birthday":user.birthday,
            @"email":user.email,
            @"full_name":user.full_name,
            @"gender":user.gender,
            @"national_id":user.national_id,
            @"phone":user.phone,
            @"provider":user.provider,
            @"loginType":type,
            @"is_new":@(user.is_new)
            
        };
        NSDictionary *dict = @{
            @"message":message,
            @"code":@(errorCode),
            @"user":userDict,
            
        };
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict
                                                           options:NSJSONWritingPrettyPrinted // Pass 0 if you don't care about the readability of the generated string
                                                             error:&error];
        NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage("ESSceneCanvas", "onLoginSuccess", [jsonString UTF8String]);
    }else{
        UnitySendMessage("ESSceneCanvas", "onLoginFailure", "");
    }
}

+ (void)responseLogout {
  printf("----------------callback--logout");
    UnitySendMessage("ESSceneCanvas", "onLogout", "");
}

+ (void)deleteUserCallback:(BOOL)isSuccess {
    bool status = isSuccess == 1?true:false;
    NSDictionary *dict = @{
        @"status":@(status)
    };
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict
                                                       options:NSJSONWritingPrettyPrinted
                                                         error:&error];
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    UnitySendMessage("ESSceneCanvas", "onDeleteUserCallback", [jsonString UTF8String]);
}
+(void)retreiveFirebaseMessaingToken{
    [[ESGameSDK sharedObject] retreiveFirebaseMessaingToken];
}
+(void)onFireBaseTokenChange:(NSString*)token{
    NSLog(@"bonFireBaseTokenChange: %@", token);
    UnitySendMessage("ESSceneCanvas", "onFireBaseTokenChange", [token UTF8String]);
}
+(void)paymentSuccess:(SKPaymentTransaction*)transaction{
    NSLog(@"buyProduct return: %@", transaction.payment.productIdentifier);
    NSDictionary *dict = @{
        @"productId":transaction.payment.productIdentifier,
        @"orderId":transaction.transactionIdentifier
        
    };
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict
                                                       options:NSJSONWritingPrettyPrinted // Pass 0 if you don't care about the readability of the generated string
                                                         error:&error];
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    UnitySendMessage("ESSceneCanvas", "onBillingResult", [jsonString UTF8String]);
}

@end
 
 
 /*******************************************************
  */
 #pragma mark -    C String Helpers
 /*
  ********************************************************/
 
 
 // Converts C style string to NSString
 NSString* CreateNSString (const char* string)
 {
     NSString *rs =  [NSString stringWithUTF8String: string];
     return rs;
 }
 
 
 NSArray* ExplodeNSStringFromCString (const char* string)
 {
     if (string)
         return [[NSString stringWithUTF8String: string] componentsSeparatedByString:@","];
     else
         return [NSArray new];
 }
 
 // Helper method to create C string copy
 char* MakeStringCopy (const char* string)
 {
     if (string == NULL)
         return NULL;
     
     char* res = (char*)malloc(strlen(string) + 1);
     strcpy(res, string);
     return res;
 }
 
 
 
 extern "C"
 {
 // function definition, called from c# or javascript unity code
 void _login()
     {
       
         // calls the method to display our view controller over the unity controller
         [ESioPlugin  login];
     }
 
 void _logout()
     {
         // calls the method to display our view controller over the unity controller
     [ESioPlugin  logout];
     }
 void _checkSession()
     {
         // calls the method to display our view controller over the unity controller
     [ESioPlugin  checkSession];
     }
 void _billing(const char* productId,const char* serverId,const char* playerId,const char* extra)
     {
         // calls the method to display our view controller over the unity controller
     [ESioPlugin billing:CreateNSString(productId) :CreateNSString(serverId) :CreateNSString(playerId) :CreateNSString(extra)];
     }
 void _config(const char* appsflyerId,const char* clientId,const char* clientSecret,const char* ggId,const char* appleId)
     {
         // calls the method to display our view controller over the unity controller
     [ESioPlugin config:CreateNSString(appsflyerId) :CreateNSString(clientId) :CreateNSString(clientSecret) :CreateNSString(ggId) :CreateNSString(appleId)];
     }
 void _trackingEvent(const char* eventName,const char* data)
     {
         // calls the method to display our view controller over the unity controller
     [ESioPlugin trackingEvent:CreateNSString(eventName) :CreateNSString(data)];
     }
 void _setProducts(const char* productString)
     {
         // calls the method to display our view controller over the unity controller
     NSString *params =CreateNSString(productString);
     NSLog(@"_setProducts %@",params);
     [ESioPlugin initProducts:params];
     }
 void _setDeviceManagement(const char* deviceIdString)
     {
         // calls the method to display our view controller over the unity controller
     [ESioPlugin  setDeviceManagement:CreateNSString(deviceIdString)];
     }
 bool _isIosAbove13(){
     if (@available(iOS 13.0, *)) {
         return true;
     }
     return false;
 }
 void _setFloatingButtonEnable(bool floatingButtonEnable){
     [ESioPlugin setFloatingButtonEnable:floatingButtonEnable];
 }
 
 void _startInGameMain(){
     [ESioPlugin startInGameMain];
 }
 
 void _deleteAccount(){
     [ESioPlugin deleteAccount];
 }
 void _check(const char* url,const char* title)
     {
         NSString* murl = CreateNSString(url);
         NSString* mtitle = CreateNSString(title);
         // calls the method to display our view controller over the unity controller
         //[[ESioPlugin singleton] loadWebViewUrl:murl :mtitle];
     }
 
void _retreiveFirebaseMessaingToken()
    {
        [ESioPlugin retreiveFirebaseMessaingToken];
    }
 }
// end of extern C block
