#import <FirebaseRemoteConfig/FirebaseRemoteConfig.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef void *_cs_action;
typedef void (*_cs_fetch_callback)(_cs_action, int);
    
void _firebase_remoteconfig_setDefaultValues(NSMutableDictionary *dictionary) {
    [[FIRRemoteConfig remoteConfig] setDefaults:dictionary];
}

void _firebase_remoteconfig_fetch(long long interval, _cs_action action, _cs_fetch_callback fetchCallback) {
    [[FIRRemoteConfig remoteConfig] fetchWithExpirationDuration:(NSTimeInterval)interval completionHandler:^(FIRRemoteConfigFetchStatus status, NSError *error) {
        if (status == FIRRemoteConfigFetchStatusSuccess) {
            const int FetchStatus_Success = 0;
            const int FetchStatus_Failure = 1;
            const int FetchStatus_Throttled = 2;
            
            int csStatus = FetchStatus_Failure;
            if (status == FIRRemoteConfigFetchStatusSuccess) {
                csStatus = FetchStatus_Success;
            } else if (status == FIRRemoteConfigFetchStatusThrottled) {
                csStatus = FetchStatus_Throttled;
            }
            [[FIRRemoteConfig remoteConfig] activateFetched];
            fetchCallback(action, csStatus);
        }
    }];
}

long long _firebase_remoteconfig_getLong(const char *key) {
    FIRRemoteConfigValue *configValue = [[FIRRemoteConfig remoteConfig] configValueForKey:[NSString stringWithUTF8String:key]];
    return [configValue numberValue].longValue;
}

double _firebase_remoteconfig_getDouble(const char *key) {
    FIRRemoteConfigValue *configValue = [[FIRRemoteConfig remoteConfig] configValueForKey:[NSString stringWithUTF8String:key]];
    return [configValue numberValue].doubleValue;
}

bool _firebase_remoteconfig_getBool(const char *key) {
    FIRRemoteConfigValue *configValue = [[FIRRemoteConfig remoteConfig] configValueForKey:[NSString stringWithUTF8String:key]];
    return [configValue boolValue];
}

const char *_firebase_remoteconfig_getString(const char *key) {
    FIRRemoteConfigValue *configValue = [[FIRRemoteConfig remoteConfig] configValueForKey:[NSString stringWithUTF8String:key]];
    const char *str = [configValue stringValue].UTF8String;
    char* retStr = (char*)malloc(strlen(str) + 1);
    strcpy(retStr, str);
    return retStr;
}

#ifdef __cplusplus
}
#endif
