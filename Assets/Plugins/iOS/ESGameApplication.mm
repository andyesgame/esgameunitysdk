//
//  ESGameApplication.mm
//  Unity-iPhone
//
//  Created by ESGame on 18/06/2021.
//

#import <Foundation/Foundation.h>

#import "UnityAppController.h"
#import "AppDelegateListener.h"
#import "ESGameSDK.h"

@interface ESGameApplication : UnityAppController
{
}
@end
IMPL_APP_CONTROLLER_SUBCLASS(ESGameApplication)
@implementation ESGameApplication


- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    NSLog(@"dfsdidFinishLaunchingWithOptions");
    // Override point for customization after application launch.
    [ESGameSDK application:application didFinishLaunchingWithOptions:launchOptions];
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}



- (void)applicationDidBecomeActive:(UIApplication *)application
{
    [ESGameSDK applicationDidBecomeActive:application];
    [super applicationDidBecomeActive:application];
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options {
    
    return [ESGameSDK application:application openURL:url options:options]&&[super application:application openURL:url options:options];
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation{
    return [ESGameSDK application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
}
 
@end
