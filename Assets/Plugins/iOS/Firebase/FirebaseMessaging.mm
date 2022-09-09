#import <FirebaseMessaging/FirebaseMessaging.h>
#import <UserNotifications/UserNotifications.h>
#import <UIKit/UIKit.h>
#import "AppDelegateListener.h"

@interface FIRMessagingDelegateListener : NSObject <AppDelegateListener>
@end

@implementation FIRMessagingDelegateListener

static FIRMessagingDelegateListener *_instance = nil;

+ (void)load {
    if (_instance) return;
    _instance = [[FIRMessagingDelegateListener alloc] init];
    UnityRegisterAppDelegateListener(_instance);
}

- (void)didBecomeActive:(NSNotification*)notification; {
    [UIApplication sharedApplication].applicationIconBadgeNumber = -1;
}

@end


#ifdef __cplusplus
extern "C" {
#endif

void _firebase_messaging_initialize() {
    if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_9_x_Max) {
        UIUserNotificationType allNotificationTypes =
        (UIUserNotificationTypeSound | UIUserNotificationTypeAlert | UIUserNotificationTypeBadge);
        UIUserNotificationSettings *settings =
        [UIUserNotificationSettings settingsForTypes:allNotificationTypes categories:nil];
        
        [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
    } else {
        // iOS 10 or later
#if defined(__IPHONE_10_0) && __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
        // For iOS 10 display notification (sent via APNS)
//        [UNUserNotificationCenter currentNotificationCenter].delegate = receiver;
        UNAuthorizationOptions authOptions =
        UNAuthorizationOptionAlert
        | UNAuthorizationOptionSound
        | UNAuthorizationOptionBadge;
        [[UNUserNotificationCenter currentNotificationCenter] requestAuthorizationWithOptions:authOptions completionHandler:^(BOOL granted, NSError *error) {
        }];
#endif
    }
    
    [[UIApplication sharedApplication] registerForRemoteNotifications];
}

#ifdef __cplusplus
}
#endif
