//
//  ESioPlugin.h
//  Unity-iPhone
//
//  Created by ESGame on 18/06/2021.
//
#import "SdkDelegate.h"
#ifndef ESioPlugin_h
#define ESioPlugin_h
@interface ESioPlugin : NSObject<SdkDelegate>
+(void)login;
+(void)logout;
+(void)checkSession;
+(void)initProducts:(NSString *) productString;
+(void)setDeviceManagement:(NSString *) deviceId;
+(void)billing:(NSString *) productId :(NSString *)serverId :(NSString *)playerId :(NSString *)extraData ;
+(void)config:(NSString *) appsflyerId :(NSString *)clientId :(NSString *)clientSecret :(NSString *)ggId ;
@end

#endif /* ESioPlugin_h */
